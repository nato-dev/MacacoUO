using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [FlipableAttribute(0x13E3, 0x13E4)]
    public class FerramentasJoalheiro : BaseTool
    {
        [Constructable]
        public FerramentasJoalheiro()
            : base(0x0FB7)
        {
            Name = "Ferramenta do Joalheiro";
            this.Weight = 8.0;
            this.Layer = Layer.Invalid;
        }

        [Constructable]
        public FerramentasJoalheiro(int uses)
            : base(uses, 0x0FB7)
        {
            this.Weight = 8.0;
            this.Layer = Layer.Invalid;
        }

        public FerramentasJoalheiro(Serial serial)
            : base(serial)
        {
        }

        public override CraftSystem CraftSystem
        {
            get
            {
                return DefJewelcrafting.CraftSystem;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
