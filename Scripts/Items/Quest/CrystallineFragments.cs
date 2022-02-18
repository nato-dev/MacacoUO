using System;

namespace Server.Items
{
    public class CrystallineFragments : Item
    {
        [Constructable]
        public CrystallineFragments()
            : base(0x223B)
        {
            this.Hue = 0x47E;
            this.Name = "Fragmentos de Essencia Elemental";
        }

        public CrystallineFragments(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!this.Movable)
                return;

            if(Utility.RandomDouble() < 0.05)
            {
                from.Backpack.AddItem(BaseEssencia.RandomEssencia());
                from.SendMessage("Voce encontrou uma essencia elemental dentro do cristal");
            } else
            {
                from.SendMessage("Voce quebrou o cristal, e nao encontrou nada dentro.");
            }
            Consume();
        }

        public override int LabelNumber
        {
            get
            {
                return 1073160;
            }
        }// Crystalline Fragments
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
