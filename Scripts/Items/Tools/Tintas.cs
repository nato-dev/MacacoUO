using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class TintaBranca : DyeTub
    {
        public static int COR = 1154;

        public void Setup()
        {
            this.Name = "Tinta Branca de Neve";
            this.Hue = 1154;
            this.DyedHue = 1154;
            this.charges = 6;
        }

        [Constructable]
        public TintaBranca() {
            Setup();
        }

        public TintaBranca(Serial serial)
           : base(serial)
        {
            Setup();
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

    public class TintaPreta : DyeTub
    {
        public static int COR = 1175;

        public void Setup()
        {
            this.Name = "Tinta Negra de Cinzas";
            this.Hue = 1175;
            this.DyedHue = 1175;
            this.charges = 6;
        }

        [Constructable]
        public TintaPreta()
        {
            Setup();
        }

        public TintaPreta(Serial serial)
           : base(serial)
        {
            Setup();
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
