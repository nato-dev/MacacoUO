#region Header
//   Vorspire    _,-'/-'/  Battle_Queue.cs
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
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.Engines.PartySystem;
using Server.Mobiles;
using Server.Regions;
#endregion

namespace VitaNex.Modules.AutoPvP
{
	public abstract partial class PvPBattle
	{
		public Dictionary<PlayerMobile, PvPTeam> Queue { get; private set; }

		[CommandProperty(AutoPvP.Access)]
		public virtual bool QueueAllowed { get; set; }

		public virtual IEnumerable<PlayerMobile> GetQueued(PvPTeam team)
		{
			foreach (var kvp in Queue.Where(kvp => kvp.Value == team))
			{
				yield return kvp.Key;
			}
		}

		public virtual void EnqueueParty(PlayerMobile pm, Party party, PvPTeam team = null)
		{
			if (pm == null || pm.Deleted || party == null || !party.Active || party.Leader != pm)
			{
				return;
			}

			var players = party.Members.Where(pmi => pmi != null && pmi.Mobile != pm)
							   .Select(pmi => pmi.Mobile as PlayerMobile)
							   .Where(m => m != null);

			foreach (var m in players)
			{
				Enqueue(m, team, false);
			}
		}

		public virtual void Enqueue(PlayerMobile pm, PvPTeam team = null, bool party = true)
		{
			if (pm == null || pm.Deleted)
			{
				return;
			}

			if (!CanQueue(pm))
			{
				OnQueueReject(pm);
				return;
			}

			if (team != null && team.Deleted)
			{
				team = null;
			}

			if (!IsQueued(pm))
			{
				Queue.Add(pm, team);
				OnQueueJoin(pm, team);
			}
			else
			{
				Queue[pm] = team;
				OnQueueUpdate(pm, team);
			}

			if (party)
			{
				EnqueueParty(pm, Party.Get(pm), team);
			}
		}

		public virtual void Dequeue(PlayerMobile pm)
		{
			if (pm == null || pm.Deleted)
			{
				return;
			}

			var team = Queue.GetValue(pm);

			if (Queue.Remove(pm))
			{
				OnQueueLeave(pm, team);
			}
		}

		public virtual bool CanQueue(PlayerMobile pm)
		{
			return !IsInternal && QueueAllowed && IsOnline(pm) && pm.Alive && !InCombat(pm) && !IsQueued(pm) &&
				   !InOtherBattle(pm) && !AutoPvP.IsDeserter(pm) && !pm.InRegion<Jail>();
		}

		public virtual bool IsQueued(PlayerMobile pm)
		{
			return pm != null && Queue.ContainsKey(pm);
		}

		protected virtual void OnQueueJoin(PlayerMobile pm, PvPTeam team)
		{
			if (pm == null || pm.Deleted)
			{
				return;
			}

			pm.SendMessage(
				"Voce entrou na fila para {0}{1}",
				Name,
				team != null ? ", seu time: " + team.Name : String.Empty);

			SendSound(pm, Options.Sounds.QueueJoin);

			AutoPvP.InvokeQueueJoin(this, team, pm);
		}

		protected virtual void OnQueueUpdate(PlayerMobile pm, PvPTeam team)
		{
			if (pm == null || pm.Deleted)
			{
				return;
			}

			pm.SendMessage(
				"Sua posicao na fila {0} mudou{1}",
				Name,
				team != null ? ", seu time: " + team.Name : String.Empty);

			SendSound(pm, Options.Sounds.QueueJoin);

			AutoPvP.InvokeQueueUpdate(this, team, pm);
		}

		protected virtual void OnQueueLeave(PlayerMobile pm, PvPTeam team)
		{
			if (pm == null || pm.Deleted || IsParticipant(pm))
			{
				return;
			}

			pm.SendMessage("Voce saiu da fila para {0}", Name);

			SendSound(pm, Options.Sounds.QueueLeave);

			AutoPvP.InvokeQueueLeave(this, team, pm);
		}

		protected virtual void OnQueueReject(PlayerMobile pm)
		{
			if (pm == null || pm.Deleted)
			{
				return;
			}

			if (IsQueued(pm))
			{
				pm.SendMessage("Voce ja esta na fila para {0}", Name);
			}
			else
			{
				pm.SendMessage("Nao pode entrar na fila para {0} agora.", Name);
			}
		}
	}
}
