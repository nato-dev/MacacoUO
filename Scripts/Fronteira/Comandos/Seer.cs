using System;
using System.Collections;
using System.Collections.Generic;
using Server.Accounting;
using Server.Commands;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Regions;

namespace Server.Games
{
    public class CreaturePossession
    {
        public const AccessLevel FullAccessStaffLevel = AccessLevel.GameMaster;

        private static readonly List<BaseCreature> m_possessedMonsters = new List<BaseCreature>();
        internal static List<BaseCreature> PossessedMonsters { get { return m_possessedMonsters; } }

        public static void Initialize()
        {

            CommandSystem.Register("unpossess", AccessLevel.Player, new CommandEventHandler(Unpossess_Command));
            CommandSystem.Register("possess", AccessLevel.GameMaster, new CommandEventHandler(Possess_Command));

            CommandSystem.Register("otherpossess", AccessLevel.GameMaster, new CommandEventHandler(OtherPossess_Command));

            CommandSystem.Register("dungeons", AccessLevel.GameMaster, new CommandEventHandler(DungeonReport_Command));
        }


        public static PlayerMobile GetAssociatedPlayerMobile(Mobile m)
        {
            PlayerMobile player = m as PlayerMobile;
            if (player == null)
            {
                BaseCreature bc = m as BaseCreature;
                if (bc != null
                    && !bc.Deleted
                    && bc.NetState != null
                    && bc.NetState.Account != null)
                {
                    player = bc.NetState.Account.GetPseudoSeerLastCharacter() as PlayerMobile;
                }
            }
            return player;
        }

        public static void DungeonReport_Command(CommandEventArgs e)
        {
            PlayerMobile player = GetAssociatedPlayerMobile(e.Mobile);
            string message = "Jogadores por regiao: \n";
            foreach (Region region in Region.Regions)
            {
                if (region is DungeonRegion)
                {
                    message += region.Name + ": " + region.GetPlayerCount() + "\n";
                }
            }
            e.Mobile.SendMessage(message);
        }

        public static void OtherPossess_Command(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(OtherPossess_PlayerTarget));
            e.Mobile.SendMessage("Selecione um jogador.");

        }

        public static void OtherPossess_PlayerTarget(Mobile gm, object o)
        {
            if (o is PlayerMobile && ((PlayerMobile)o).AccessLevel == AccessLevel.Player)
            {
                gm.Target = new OtherPossessMonsterTarget(o as PlayerMobile);
                gm.SendMessage("Selecione um monstro.");
            }
            else
            {
                gm.SendMessage("Invalido.");
            }
        }

        private class OtherPossessMonsterTarget : Target
        {
            private PlayerMobile m_Player;

            public OtherPossessMonsterTarget(PlayerMobile player)
                : base(-1, false, TargetFlags.None)
            {
                if (player == null) { return; }
                m_Player = player;
            }

            protected override void OnTarget(Mobile gm, object target)
            {
                if (m_Player == null)
                {
                    gm.SendMessage("ERROR: nao achei a instancia do player!");
                    return;
                }

                if (target is BaseCreature)
                {
                    ForcePossessCreature(gm, m_Player, target as BaseCreature);
                }
                else
                {
                    gm.SendMessage("Monstro invalido.");
                }
            }
        }

        public static void Unpossess_Command(CommandEventArgs e)
        {
            if (e.Mobile is PlayerMobile) { return; }
            if (!AttemptReturnToOriginalBody(e.Mobile.NetState))
            {
                e.Mobile.NetState.Dispose();
            }
            if(e.Mobile.Account != null)
                e.Mobile.Account.SetPseudoSeerLastCharacter(null);
        }

        public static bool AttemptReturnToOriginalBody(NetState monsterNetState)
        {
            if (monsterNetState == null) { return false; }
            if (monsterNetState == null || monsterNetState.Account == null || monsterNetState.Account.GetPseudoSeerLastCharacter() == null)
            {
                return false;
            }
            PlayerMobile pseudoSeerLastCharacter = (PlayerMobile)monsterNetState.Account.GetPseudoSeerLastCharacter();
            Point3D newLocation = new Point3D(0, 0, 0);
            newLocation = monsterNetState.Mobile.Location;
            bool output = CreaturePossession.ConnectClientToPC(monsterNetState, pseudoSeerLastCharacter);
            // do it after they log back in
            if (!(newLocation.X == 0 && newLocation.Y == 0 && newLocation.Z == 0)) { pseudoSeerLastCharacter.Location = newLocation; }
            return output;
        }

        public static void Possess_Command(CommandEventArgs e)
        {
            OnPossessTargetRequest(e.Mobile);
        }

        public static void OnPossessTargetRequest(Mobile from)
        {
            if (CreaturePossession.HasAnyPossessPermissions(from))
            {
                from.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(OnPossessTarget));
                from.SendMessage("Selecione um monstro.");
            }
        }

        public static void OnPossessTarget(Mobile from, object o)
        {
            if (o is BaseCreature)
                CreaturePossession.TryPossessCreature(from, (BaseCreature)o);
            else
                from.SendMessage("Nao e um monstro valido.");
        }

        public static void TryPossessCreature(Mobile from, BaseCreature Subject)
        {
            if (HasPermissionsToPossess(from, Subject))
            {
                if (Subject.NetState != null)
                {
                    from.SendMessage("Ja esta controlado!");
                    return;
                }
                if (IsInHouseOrBoat(Subject.Location, Map.Felucca))
                {
                    from.SendMessage("Voce nao pde controlar isso em uma casa ou barco.");
                    return;
                }
                if (Subject.Controlled && !IsAuthorizedStaff(from))
                {
                    from.SendMessage("Nao pode controlar bixos tamados/summonados!");
                    return;
                }
                Subject.HasBeenPseudoseerControlled = true;
                if (Subject.Backpack == null)
                {
                    // create a backpack for things like animals that have no backpack
                    // ... this prevents client crash in case somebody has their pack auto-opening on login
                    Subject.PackItem(new Gold(1));
                }
                ConnectClientToNPC(from.NetState, Subject);
            }
            else
            {
                from.SendMessage("Voce naot em permissao.");
            }
        }

        public static void ForcePossessCreature(Mobile gm, PlayerMobile player, BaseCreature Subject)
        {
            if (Subject == null || player == null)
            {
                if (gm != null) gm.SendMessage("Erro NULL...");
                return;
            }
            if (Subject.NetState != null)
            {
                if (gm != null) gm.SendMessage("Criatura ja controlada!");
                return;
            }
            Subject.HasBeenPseudoseerControlled = true;
            ConnectClientToNPC(player.NetState, Subject);
        }

        public static bool HasPermissionsToPossess(Mobile from, BaseCreature Subject)
        {
            return from != null && (from.AccessLevel >= AccessLevel.Counselor || HasPermissionsToPossess(from.NetState, Subject));
        }

        public static bool HasPermissionsToPossess(NetState from, BaseCreature Subject)
        {
            return from != null && IsAuthorizedAccount(from.Account, Subject);
        }

        public static bool HasAnyPossessPermissions(Mobile from)
        {
            return from != null && (from.AccessLevel >= AccessLevel.Counselor || HasAnyPossessPermissions(from.NetState));
        }

        public static bool HasAnyPossessPermissions(NetState from)
        {
            return from != null &&
                (CreaturePossession.IsAuthorizedStaff(from));
        }

        static bool IsAuthorizedAccount(IAccount account, BaseCreature Subject)
        {
            return true;
        }

        internal static bool IsAuthorizedStaff(Mobile from)
        {
            return from.AccessLevel >= AccessLevel.GameMaster || (from != null && IsAuthorizedStaff(from.NetState));
        }

        internal static bool IsAuthorizedStaff(NetState from)
        {
            return from != null && (IsAuthorizedStaff(from.Account) || (from.Mobile != null && from.Mobile.AccessLevel >= AccessLevel.Counselor));
        }

        internal static bool IsAuthorizedStaff(IAccount account)
        {
            return account != null && account.AccessLevel >= FullAccessStaffLevel;
        }

        internal static bool ConnectClientToNPC(NetState client, BaseCreature Subject)
        {
            if (Subject.NetState == null)
            {
                Mobile clientMobile = client.Mobile;
                if (client.Account != null && clientMobile is PlayerMobile) client.Account.SetPseudoSeerLastCharacter(client.Mobile);

                Subject.NetState = client;

                if (clientMobile != null)
                {
                    clientMobile.NetState = null;
                }

                Subject.NetState.Mobile = Subject;

                PacketHandlers.DoLogin(Subject.NetState, Subject);

                PossessedMonsters.Add(Subject);
                return true;
            }
            return false;
        }

        internal static bool ConnectClientToPC(NetState client, PlayerMobile Subject)
        {
            if (Subject != null && Subject.NetState == null)
            {
                Mobile clientMobile = client.Mobile;

                Subject.NetState = client;

                if (clientMobile != null)
                    clientMobile.NetState = null;

                Subject.NetState.Mobile = Subject;

                PacketHandlers.DoLogin(Subject.NetState, Subject);
                return true;
            }
            return false;
        }

        internal static void BootAllPossessions()
        {
            for (int i = 0; i < PossessedMonsters.Count; i++)
            {
                if (PossessedMonsters[i].Deleted || PossessedMonsters[i].NetState == null)
                    continue;

                //boot any player controlled monsters, attempting to return them to their original body if possible
                if (AttemptReturnToOriginalBody(PossessedMonsters[i].NetState) == false)
                {
                    PossessedMonsters[i].NetState.Dispose();
                }
            }
            PossessedMonsters.Clear();
        }

        // from Derrick
        public static bool IsInHouseOrBoat(Point3D location, Map map)
        {
            Region region = Region.Find(location, map);
            return (region != null && region.GetRegion(typeof(Regions.HouseRegion)) != null)
                || Server.Multis.BaseBoat.FindBoatAt(location, map) != null;
        }

        static void LogoutIfPlayer(Mobile from)
        {
            if (from is PlayerMobile && from.Map != Map.Internal)
                EventSink.InvokeLogout(new LogoutEventArgs(from));
        }
    }
}
