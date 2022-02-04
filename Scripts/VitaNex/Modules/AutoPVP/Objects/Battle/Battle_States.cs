#region Header
//   Vorspire    _,-'/-'/  Battle_States.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2018  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;
using System.Linq;

using Server;
#endregion

namespace VitaNex.Modules.AutoPvP
{
    public enum PvPBattleState
    {
        Internal,
        EmFila,
        Preparando,
        Batalhando,
        Terminando
    }

    public abstract partial class PvPBattle
    {
        private bool _StateTransition;

        private PvPBattleState _State = PvPBattleState.Internal;

        [CommandProperty(AutoPvP.Access)]
        public virtual PvPBattleState State
        {
            get { return _State; }
            set
            {
                if (_State == value)
                {
                    return;
                }

                var oldState = _State;

                _State = value;

                if (!Deserializing)
                {
                    OnStateChanged(oldState);
                }
            }
        }

        [CommandProperty(AutoPvP.Access, true)]
        public virtual PvPBattleState LastState { get; private set; }

        [CommandProperty(AutoPvP.Access, true)]
        public virtual DateTime LastStateChange { get; private set; }

        [CommandProperty(AutoPvP.Access)]
        public bool IsInternal { get { return State == PvPBattleState.Internal; } }

        [CommandProperty(AutoPvP.Access)]
        public bool IsQueueing { get { return State == PvPBattleState.EmFila; } }

        [CommandProperty(AutoPvP.Access)]
        public bool IsPreparing { get { return State == PvPBattleState.Preparando; } }

        [CommandProperty(AutoPvP.Access)]
        public bool IsRunning { get { return State == PvPBattleState.Batalhando; } }

        [CommandProperty(AutoPvP.Access)]
        public bool IsEnded { get { return State == PvPBattleState.Terminando; } }

        public void InvalidateState()
        {

            if (_StateTransition || Deleted || Hidden || IsInternal)
            {
                return;
            }

            var time = GetStateTimeLeft(State);

            if (time > TimeSpan.Zero)
            {
                return;
            }

            var state = State;
            var reset = false;

            switch (state)
            {
                case PvPBattleState.EmFila:
                    {
                        if (CanPrepareBattle(ref reset))
                        {
                            state = PvPBattleState.Preparando;
                        }
                    }
                    break;
                case PvPBattleState.Preparando:
                    {
                        if (CanStartBattle(ref reset))
                        {
                            state = PvPBattleState.Batalhando;
                        }
                        else if (!reset)
                        {
                            state = PvPBattleState.Terminando;
                        }
                    }
                    break;
                case PvPBattleState.Batalhando:
                    {
                        if (CanEndBattle(ref reset))
                        {
                            state = PvPBattleState.Terminando;
                        }
                    }
                    break;
                case PvPBattleState.Terminando:
                    {
                        if (CanOpenBattle(ref reset))
                        {
                            state = PvPBattleState.EmFila;
                        }
                    }
                    break;
            }

            if (reset)
            {
                Options.Timing.SetTime(State, DateTime.UtcNow);
            }

            State = state;
        }

        protected virtual void OnStateChanged(PvPBattleState oldState)
        {
            _StateTransition = true;

            LastState = oldState;

            if (IsInternal)
            {
                Options.Timing.SetAllTimes(DateTime.MinValue);

                OnBattleInternalized();
            }
            else
            {
                if (LastState == PvPBattleState.Internal)
                {
                    InvalidateRegions();
                }

                Options.Timing.SetTime(State, DateTime.UtcNow);

                switch (State)
                {
                    case PvPBattleState.EmFila:
                        OnBattleOpened();
                        break;
                    case PvPBattleState.Preparando:
                        OnBattlePreparing();
                        break;
                    case PvPBattleState.Batalhando:
                        OnBattleStarted();
                        break;
                    case PvPBattleState.Terminando:
                        OnBattleEnded();
                        break;
                }
            }

            LastStateChange = DateTime.UtcNow;

            _StateTransition = false;

            AutoPvP.InvokeBattleStateChanged(this);

            PvPBattlesUI.RefreshAll(this);
        }

        protected virtual bool CanOpenBattle(ref bool timeReset)
        {
            return true;
        }

        protected virtual bool CanPrepareBattle(ref bool timeReset)
        {
            if (!RequireCapacity || Queue.Count >= MinCapacity)
            {
                return true;
            }

            if (Schedule == null || !Schedule.Enabled || Schedule.NextGlobalTick == null)
            {
                timeReset = true;
            }

            return false;
        }

        protected virtual bool CanStartBattle(ref bool timeReset)
        {
            if (Teams.All(t => t.IsReady()))
            {
                return true;
            }

            if (Schedule == null || !Schedule.Enabled || Schedule.NextGlobalTick == null)
            {
                timeReset = true;
            }

            return false;
        }

        protected virtual bool CanEndBattle(ref bool timeReset)
        {
            Shard.Debug("Can end battle?");
            if (!InviteWhileRunning && !HasCapacity())
            {
                return true;
            }

            if (Teams.All(t => !t.RespawnOnDeath && !t.IsAlive))
            {
                return true;
            }

            if (CheckMissions())
            {
                return true;
            }

            return false;
        }

        protected virtual void OnBattleInternalized()
        {
            if (LastState == PvPBattleState.Batalhando)
            {
                OnBattleCancelled();
            }

            Reset();

            foreach (var p in AutoPvP.Profiles.Values.Where(p => p != null && !p.Deleted && p.IsSubscribed(this)))
            {
                p.Unsubscribe(this);
            }

            PvPBattlesUI.RefreshAll(this);
        }

        protected virtual void OnBattleOpened()
        {
            Hidden = false;

            if (Options.Broadcasts.World.OpenNotify)
            {
                WorldBroadcast("{0} esta aberto para inscricoes ! Use .pvp para se inscrever !", Name);
            }

            if (Options.Broadcasts.Local.OpenNotify)
            {
                LocalBroadcast("{0} esta aberto para inscricoes ! Use .pvp para se inscrever !", Name);
            }

            SendGlobalSound(Options.Sounds.BattleOpened);

            Reset();

            ForEachTeam(t => t.OnBattleOpened());

            if (LastState == PvPBattleState.Internal)
            {
                foreach (var p in AutoPvP.Profiles.Values.Where(p => p != null && !p.Deleted && !p.IsSubscribed(this)))
                {
                    p.Subscribe(this);
                }
            }

            PvPBattlesUI.RefreshAll(this);
        }

        protected virtual void OnBattlePreparing()
        {
            Hidden = false;

            if (Options.Broadcasts.World.StartNotify)
            {
                WorldBroadcast("{0} esta preparando!", Name);
            }

            if (Options.Broadcasts.Local.StartNotify)
            {
                LocalBroadcast("{0} esta preparando!", Name);
            }

            SendGlobalSound(Options.Sounds.BattlePreparing);

            ForEachTeam(t => t.OnBattlePreparing());

            PvPBattlesUI.RefreshAll(this);
        }

        protected virtual void OnBattleStarted()
        {
            Hidden = false;

            if (Options.Broadcasts.World.StartNotify)
            {
                WorldBroadcast("{0} comecou!", Name);
            }

            if (Options.Broadcasts.Local.StartNotify)
            {
                LocalBroadcast("{0} comecou!", Name);
            }

            SendGlobalSound(Options.Sounds.BattleStarted);

            OpendDoors(true);

            ForEachTeam(t => t.OnBattleStarted());

            PvPBattlesUI.RefreshAll(this);
        }

        protected virtual void OnBattleEnded()
        {
            if (LastState == PvPBattleState.Preparando)
            {
                OnBattleCancelled();
            }

            Hidden = false;

            if (Options.Broadcasts.World.EndNotify)
            {
                WorldBroadcast("{0} terminou!", Name);
            }

            if (Options.Broadcasts.Local.EndNotify)
            {
                LocalBroadcast("{0} terminou!", Name);
            }

            SendGlobalSound(Options.Sounds.BattleEnded);

            CloseDoors(true);

            ProcessRanks();

            ForEachTeam(t => t.OnBattleEnded());

            TransferStatistics();

            PvPBattlesUI.RefreshAll(this);
        }

        protected virtual void OnBattleCancelled()
        {
            Hidden = false;

            if (Options.Broadcasts.World.EndNotify)
            {
                WorldBroadcast("{0} cancelado!", Name);
            }

            if (Options.Broadcasts.Local.EndNotify)
            {
                LocalBroadcast("{0} cancelado!", Name);
            }

            SendGlobalSound(Options.Sounds.BattleCanceled);

            CloseDoors(true);

            if (!Deleted && !IsInternal && QueueAllowed)
            {
                ForEachTeam(t => t.ForEachMember(o => Queue[o] = t));
            }

            ForEachTeam(t => t.OnBattleCancelled());
        }

        public TimeSpan GetStateTimeLeft()
        {
            return GetStateTimeLeft(DateTime.UtcNow);
        }

        public TimeSpan GetStateTimeLeft(DateTime when)
        {
            return GetStateTimeLeft(when, State);
        }

        public TimeSpan GetStateTimeLeft(PvPBattleState state)
        {
            return GetStateTimeLeft(DateTime.UtcNow, state);
        }

        public virtual TimeSpan GetStateTimeLeft(DateTime when, PvPBattleState state)
        {
            var time = 0.0;

            switch (state)
            {
                case PvPBattleState.EmFila:
                    {
                        time = (Options.Timing.OpenedWhen.Add(Options.Timing.QueuePeriod) - when).TotalSeconds;

                        if (Schedule != null && Schedule.Enabled)
                        {
                            if (Schedule.CurrentGlobalTick != null)
                            {
                                time = (Schedule.CurrentGlobalTick.Value - when).TotalSeconds;
                            }
                            else if (Schedule.NextGlobalTick != null)
                            {
                                time = (Schedule.NextGlobalTick.Value - when).TotalSeconds;
                            }
                        }
                    }
                    break;
                case PvPBattleState.Preparando:
                    time = (Options.Timing.PreparedWhen.Add(Options.Timing.PreparePeriod) - when).TotalSeconds;
                    break;
                case PvPBattleState.Batalhando:
                    time = (Options.Timing.StartedWhen.Add(Options.Timing.RunningPeriod) - when).TotalSeconds;
                    break;
                case PvPBattleState.Terminando:
                    time = (Options.Timing.EndedWhen.Add(Options.Timing.EndedPeriod) - when).TotalSeconds;
                    break;
            }

            if (time > 0)
            {
                return TimeSpan.FromSeconds(time);
            }

            return TimeSpan.Zero;
        }
    }
}
