using System;
using System.IO;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Linq;
using System.Collections.Generic;
using Server.Engines.CityLoyalty;
using Server.Engines.VvV;
using Server.Engines.ArenaSystem;
using Server.Engines.SorcerersDungeon;
using Server.Ziden.Kills;

namespace Server.Engines.Points
{
    public enum PointsType
    {
        None = -1,

        QueensLoyalty,
        VoidPool,
        DespiseCrystals,
        ShameCrystals,
        CasinoData,

        //City Trading
        CityTrading,

        // City Loyalty System
        Moonglow,
        Britain,
        Jhelom,
        Yew,
        Minoc,
        Trinsic,
        SkaraBrae,
        NewMagincia,
        Vesper,

        Blackthorn,
        CleanUpBritannia,
        ViceVsVirtue,

        TreasuresOfKotlCity,
        PVPArena,

        Khaldun,
        Doom,
        SorcerersDungeon,

        GuerraTotal,
        Kills,
        Login,
        RP,

        Rhodes,
        CaboDaTormenta,

        PvM,
        PvMEterno,

        Trabalho,

        Exp,
        Taming,

        Ferreiro,
        Alfaiate,
        Carpinteiro,
        Pescador,
        Minerador,
        Lenhador,
        Alquimista,
        Cozinha,
        Ouro
    }

    public abstract class PointsSystem
    {
        public static string FilePath = Path.Combine("Saves/PointsSystem", "Persistence.bin");

        public List<PointsEntry> PlayerTable { get { return Players.Values.ToList(); } }
        public Dictionary<Serial, PointsEntry> Players { get; set; }

        public void Clear()
        {
            Players = new Dictionary<Serial, PointsEntry>();
            calculated = DateTime.MinValue;
        }

        public abstract TextDefinition Name { get; }
        public abstract PointsType Loyalty { get; }
        public abstract bool AutoAdd { get; }
        public abstract double MaxPoints { get; }

        public virtual bool ShowOnLoyaltyGump { get { return true; } }

        public PointsSystem()
        {
            Players = new Dictionary<Serial, PointsEntry>();

            AddSystem(this);
        }

        private static void AddSystem(PointsSystem system)
        {
            if (Systems.FirstOrDefault(s => s.Loyalty == system.Loyalty) != null)
                return;

            Systems.Add(system);
        }

        public virtual void ProcessKill(BaseCreature victim, Mobile damager, int index)
        {
        }

        public virtual void ProcessQuest(Mobile from, Type quest)
        {
        }

        public virtual void ConvertFromOldSystem(PlayerMobile from, double points)
        {
            PointsEntry entry = GetEntry(from, false);

            if (entry == null)
            {
                if (points > MaxPoints)
                    points = MaxPoints;

                AddEntry(from, true);
                GetEntry(from).Points = points;

                Utility.PushColor(ConsoleColor.Green);
                Console.WriteLine("Converted {0} points for {1} to {2}!", (int)points, from.Name, this.GetType().Name);
                Utility.PopColor();
            }
        }

        public virtual double AwardPoints(Mobile from, double points, bool quest = false, bool message = true)
        {
            if (!(from is PlayerMobile) || points <= 0)
                return 0;

            PointsEntry entry = GetEntry(from);
            var old = 0d;
            if (entry != null)
            {
                SetPoints((PlayerMobile)from, Math.Min(MaxPoints, entry.Points + points));

                old = entry.Points;

                if (message)
                    SendMessage((PlayerMobile)from, old, points, quest);
            }
            return old;
        }

        public void SetPoints(PlayerMobile pm, double points)
        {
            PointsEntry entry = GetEntry(pm);

            if (entry != null)
                entry.Points = points;
        }

        public virtual void SendMessage(PlayerMobile from, double old, double points, bool quest)
        {
                from.SendLocalizedMessage(78, String.Format("+{0}\t {1}", ((int)points).ToString(), Name.ToString()));  // Your loyalty to ~1_GROUP~ has increased by ~2_AMOUNT~;Original
        }

        public virtual bool DeductPoints(Mobile from, double points, bool message = false)
        {
            PointsEntry entry = GetEntry(from);

            if (entry == null || entry.Points < points)
            {
                return false;
            }
            else
            {
                entry.Points -= points;

                if (message)
                    from.SendLocalizedMessage(String.Format("Sua lealdade a {0} diminuiu \t{1}", Name.ToString(), ((int)points).ToString()));  // Your loyalty to ~1_GROUP~ has decreased by ~2_AMOUNT~;Original
            }

            return true;
        }

        public virtual void OnPlayerAdded(PlayerMobile pm)
        {
        }

        public virtual PointsEntry AddEntry(PlayerMobile pm, bool existed = false)
        {
            PointsEntry entry = GetSystemEntry(pm);

            if (!Players.ContainsKey(pm.Serial))
            {
                Players.Add(pm.Serial, entry);

                if (!existed)
                    OnPlayerAdded(pm);
            }

            return entry;
        }

        public double GetPoints(Mobile from)
        {
            PointsEntry entry = GetEntry(from);

            if (entry != null)
                return entry.Points;

            return 0.0;
        }

        public virtual TextDefinition GetTitle(PlayerMobile from)
        {
            return null;
        }

        public PointsEntry GetEntry(Mobile from, bool create = false)
        {
            PlayerMobile pm = from as PlayerMobile;

            if (pm == null)
                return null;



            PointsEntry entry = null;
            Players.TryGetValue(pm.Serial, out entry);

            if (entry == null && (create || AutoAdd))
                entry = AddEntry(pm);

            return entry;
        }

        List<PointsEntry> _cachedRank;
        public DateTime calculated = DateTime.MinValue;

        public int GetPlayerRank(PlayerMobile pm)
        {
            return GetPlayerEntry<PointsEntry>(pm).CachedRank;
        }

        public List<PointsEntry> GetOrCalculateRank()
        {
            var now = DateTime.UtcNow;
            var hoursSinceLastEval = (now - calculated).TotalHours;
            if (hoursSinceLastEval > 1)
            {
                calculated = now;

                var PlayerTable = Players.Values;
                //_cachedRank = PlayerTable.OrderByDescending(e => e.Points);//.Select((e, index) => { e.CachedRank = index;  return e; });
                _cachedRank = PlayerTable.Where(p => p.Player.IsPlayer()).OrderByDescending(e => e.Points).Select((e, index) => { e.CachedRank = index; return e; }).ToList();
                Shard.Debug("TAMANHO RANK: " + _cachedRank.Count());
                var took = (DateTime.UtcNow - now).TotalMilliseconds;
                Console.WriteLine("Calculando ranks de " + this.Loyalty + " durou " + took + "ms");
            }
            return _cachedRank;
        }

        public TEntry GetPlayerEntry<TEntry>(Mobile mobile, bool create = false) where TEntry : PointsEntry
        {
            PlayerMobile pm = mobile as PlayerMobile;

            if (pm == null)
                return null;

            PointsEntry e;// = PlayerTable.FirstOrDefault(p => p.Player == pm) as TEntry;

            Players.TryGetValue(mobile.Serial, out e);

            if (e == null && (AutoAdd || create))
                e = AddEntry(pm);

            return e as TEntry;
        }

        /// <summary>
        /// Override this if you are going to derive Points Entry into a bigger and badder class!
        /// </summary>
        /// <param name="pm"></param>
        /// <returns></returns>
        public virtual PointsEntry GetSystemEntry(PlayerMobile pm)
        {
            return new PointsEntry(pm);
        }

        public int Version { get; set; }

        public virtual void Serialize(GenericWriter writer)
        {
            writer.Write((int)3);

            writer.Write(Players.Count);

            foreach (var key in Players.Keys)
            {
                var entry = Players[key];
                writer.Write(entry.Player);
                Players[key].Serialize(writer);
            }

            /*
            Players.ForEach(entry =>
                {
                    writer.Write(entry.Player);
                    entry.Serialize(writer);
                });
                */
        }

        public virtual void Deserialize(GenericReader reader)
        {
            Version = reader.ReadInt();

            switch (Version)
            {
                case 3:
                case 2: // added serialize/deserialize in all base classes. Poor implementation on my part, should have had from the get-go
                case 1:
                case 0:
                    {
                        int count = reader.ReadInt();


                        for (int i = 0; i < count; i++)
                        {
                            PlayerMobile player = reader.ReadMobile() as PlayerMobile;
                            PointsEntry entry = GetSystemEntry(player);

                            if (Version > 0)
                                entry.Deserialize(reader);
                            else
                                entry.Points = reader.ReadDouble();

                            if (player != null)
                            {
                                if (!Players.ContainsKey(player.Serial))
                                {
                                    Players.Add(player.Serial, entry);
                                }
                            }
                        }


                    }
                    break;
            }
        }

        #region Static Methods and Accessors
        public static PointsSystem GetSystemInstance(PointsType t)
        {
            return Systems.FirstOrDefault(s => s.Loyalty == t);
        }

        public static void OnSave(WorldSaveEventArgs e)
        {
            Persistence.Serialize(
                FilePath,
                writer =>
                {
                    writer.Write((int)3);

                    writer.Write(Systems.Count);
                    Systems.ForEach(s =>
                    {
                        writer.Write((int)s.Loyalty);
                        s.Serialize(writer);
                    });
                });
        }

        public static void OnLoad()
        {
            Persistence.Deserialize(
                FilePath,
                reader =>
                {
                    int version = reader.ReadInt();

                    if (version < 2)
                        reader.ReadBool();

                    PointsType loaded = PointsType.None;

                    int count = reader.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            PointsType type = (PointsType)reader.ReadInt();
                            loaded = type;
                            PointsSystem s = GetSystemInstance(type);

                            s.Deserialize(reader);
                        }
                        catch
                        {
                            throw new Exception(String.Format("Points System Failed Load: {0} Last Loaded...", loaded.ToString()));
                        }
                    }
                });
        }

        public static List<PointsSystem> Systems { get; set; }

        public static QueensLoyalty QueensLoyalty { get; set; }
        public static VoidPool VoidPool { get; set; }
        public static DespiseCrystals DespiseCrystals { get; set; }
        public static ShameCrystals ShameCrystals { get; set; }
        public static CasinoData CasinoData { get; set; }
        public static BlackthornData Blackthorn { get; set; }
        public static CleanUpBritanniaData CleanUpBritannia { get; set; }
        public static ViceVsVirtueSystem ViceVsVirtue { get; set; }
        public static KotlCityData TreasuresOfKotlCity { get; set; }
        public static PVPArenaSystem ArenaSystem { get; set; }
        public static KhaldunData Khaldun { get; set; }
        public static DoomData TreasuresOfDoom { get; set; }
        public static SorcerersDungeonData SorcerersDungeon { get; set; }
        public static PontosKills PontosKills { get; set; }
        public static PontosLogin PontosLogin { get; set; }
        public static PontosRP PontosRP { get; set; }
        public static PontosPvm PontosPvm { get; set; }
        public static Exp Exp { get; set; }
        public static PontoTaming PontosTaming { get; set; }
        public static PontosPvmEternos PontosPvmEterno { get; set; }
        public static PontosTrabalho PontosTrabalho { get; set; }
        public static PontosFerreiro PontosFerreiro { get; set; }
        public static PontosAlfaiate PontosAlfaiate { get; set; }
        public static PontosCarpinteiro PontosCarpinteiro { get; set; }
        public static PontosPescador PontosPescador { get; set; }
        public static PontosMinerador PontosMinerador { get; set; }
        public static PontosAlquimista PontosAlquimista { get; set; }
        public static PontosCozinha PontosCozinha { get; set; }
        public static PontosLenhador PontosLenhador { get; set; }
        public static PontosOuro PontosOuro { get; set; }

        public static void Configure()
        {
            EventSink.WorldSave += OnSave;
            EventSink.WorldLoad += OnLoad;
            EventSink.QuestComplete += CompleteQuest;

            Systems = new List<PointsSystem>();

            QueensLoyalty = new QueensLoyalty();
            VoidPool = new VoidPool();
            DespiseCrystals = new DespiseCrystals();
            ShameCrystals = new ShameCrystals();
            CasinoData = new CasinoData();
            Blackthorn = new BlackthornData();
            CleanUpBritannia = new CleanUpBritanniaData();
            ViceVsVirtue = new ViceVsVirtueSystem();
            TreasuresOfKotlCity = new KotlCityData();

            CityLoyaltySystem.ConstructSystems();
            ArenaSystem = new PVPArenaSystem();
            Khaldun = new KhaldunData();
            TreasuresOfDoom = new DoomData();
            SorcerersDungeon = new SorcerersDungeonData();
            PontosKills = new PontosKills();
            PontosLogin = new PontosLogin();
            PontosRP = new PontosRP();
            PontosPvm = new PontosPvm();
            PontosPvmEterno = new PontosPvmEternos();
            PontosTrabalho = new PontosTrabalho();
            Exp = new Exp();
            PontosTaming = new PontoTaming();

            PontosFerreiro = new PontosFerreiro();
            PontosAlfaiate = new PontosAlfaiate();
            PontosCarpinteiro = new PontosCarpinteiro();
            PontosMinerador = new PontosMinerador();
            PontosAlquimista = new PontosAlquimista();
            PontosCozinha = new PontosCozinha();
            PontosLenhador = new PontosLenhador();
            PontosOuro = new PontosOuro();
            PontosPescador = new PontosPescador();
        }

        public static void HandleKill(BaseCreature victim, Mobile damager, int index)
        {
            Systems.ForEach(s => s.ProcessKill(victim, damager, index));
        }

        public static void CompleteQuest(QuestCompleteEventArgs e)
        {
            Systems.ForEach(s => s.ProcessQuest(e.Mobile, e.QuestType));
        }
        #endregion
    }

    public class PointsEntry
    {
        public PlayerMobile Player { get; set; }
        public double Points { get; set; }
        public int CachedRank { get; set; }

        public PointsEntry(PlayerMobile pm)
        {
            Player = pm;
        }

        public PointsEntry(PlayerMobile pm, double points)
        {
            Player = pm;
            Points = points;
        }

        public override bool Equals(object o)
        {
            if (o == null || !(o is PointsEntry))
            {
                return false;
            }

            return ((PointsEntry)o).Player == Player;
        }

        public override int GetHashCode()
        {
            if (Player != null)
                return Player.GetHashCode();

            return base.GetHashCode();
        }

        public virtual void Serialize(GenericWriter writer)
        {
            writer.Write(0);
            writer.Write(Player);
            writer.Write(Points);
        }

        public virtual void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();
            Player = reader.ReadMobile() as PlayerMobile;
            Points = reader.ReadDouble();
        }
    }
}
