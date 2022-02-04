using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Guilds;
using Server.Network;
using System.Linq;
using Server.Misc;

namespace Server.Engines.VvV
{
    public class BattleWarningGump : Gump
    {
        public PlayerMobile User { get; set; }
        public Timer Timer { get; set; }

        public BattleWarningGump(PlayerMobile pm)
            : base(50, 50)
        {
            User = pm;

            AddBackground(0, 0, 500, 200, 83);

            AddHtml(0, 25, 500, 20, "Atencao", Engines.Quests.BaseQuestGump.C32216(0xFF0000), false, false);
            AddHtml(10, 55, 480, 100, "Voce esta em uma zona de Guerra Infinita e pode ser atacado !", 0xFFFF, false, false); // You are in an active Vice vs Virtue battle region!  If you do not leave the City you will be open to attack!

            AddButton(463, 168, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddHtml(250, 171, 250, 20, "Deseja voltar para Rhodes ?", 0xFFFF, false, false); // Teleport to nearest Moongate?

            Timer = Timer.DelayCall(TimeSpan.FromMinutes(1), () =>
                {
                    User.CloseGump(typeof(BattleWarningGump));
                    ViceVsVirtueSystem.AddTempParticipant(User, null);
                });
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (Timer != null)
            {
                Timer.Stop();
                Timer = null;
            }

            if (info.ButtonID == 1)
            {
                BaseCreature.TeleportPets(User, CharacterCreation.WSHALL, Map.Felucca);
                User.MoveToWorld(CharacterCreation.WSHALL, Map.Felucca);
            }
            else
            {
                User.SendLocalizedMessage("Voce podera ser atacado agora"); // You are now open to attack!
                ViceVsVirtueSystem.AddTempParticipant(User, null);
            }
        }
    }
}
