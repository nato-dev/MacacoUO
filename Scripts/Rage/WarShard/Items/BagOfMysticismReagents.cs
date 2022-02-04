namespace Server.Items
{
    public class BagOfMysticismReagents : Bag
    {
        [Constructable]
        public BagOfMysticismReagents()
            : this(50)
        {
            this.Name = "Sacola de Reagentes Mysticism";
        }

        [Constructable]
        public BagOfMysticismReagents(int amount)
        {
            this.DropItem(new Bone(amount));
            this.DropItem(new DragonBlood(amount));
            this.DropItem(new FertileDirt(amount));
            this.DropItem(new DaemonBone(amount));
        }

        public BagOfMysticismReagents(Serial serial)
            : base(serial)
        {
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
