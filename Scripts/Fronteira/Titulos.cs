using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ziden
{
    public class Titulos
    {
        public static void Initialize()
        {
            CommandSystem.Register("titulo", AccessLevel.Player, OnAction);
        }

        [Usage("Action")]
        private static void OnAction(CommandEventArgs e)
        {
            e.Mobile.CloseGump(typeof(TitlesGump));
            e.Mobile.SendGump(new TitlesGump((PlayerMobile)e.Mobile));
        }

    }


    public class LichKiller : BaseRewardTitleDeed
    {
        public override TextDefinition Title { get { return new TextDefinition("Matador de Liches"); } } // Naughty

        [Constructable]
        public LichKiller()
        {
        }

        public LichKiller(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }

    public class BetaTester : BaseRewardTitleDeed
    {
        public override TextDefinition Title { get { return new TextDefinition("Beta Tester"); } } // Naughty

        [Constructable]
        public BetaTester()
        {
        }

        public BetaTester(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }


}
