using Server.Mobiles;
using Server.Regions;
using System;

namespace Server.Items
{
    public class ExodusChest : DecorativeBox
    {
        public static void Initialize()
        {
            TileData.ItemTable[0x2DF3].Flags = TileFlag.None;
        }

        public override int DefaultGumpID { get { return 0x10C; } }

        private Timer m_Timer;

        public override bool IsDecoContainer { get { return false; } }

        [Constructable]
        public ExodusChest()
            : base()
        {
            Visible = true;
            Locked = false;
            Hue = 2700;
            Movable = true;
            GenerateTreasure();
        }

        public ExodusChest(Serial serial) : base(serial)
        {
        }

        public void StartDeleteTimer()
        {

        }

        protected virtual void GenerateTreasure()
        {
            DropItem(new Gold(500, 1000));

            Item item = null;

            for (int i = 0; i < Loot.GemTypes.Length; i++)
            {
                item = Activator.CreateInstance(Loot.GemTypes[i]) as Item;
                item.Amount = Utility.Random(1, 6);
                DropItem(item);
            }

            if (0.25 > Utility.RandomDouble())
            {
                item = new SmokeBomb(Utility.Random(3, 6));
                DropItem(item);
            }

            if (0.25 > Utility.RandomDouble())
            {
                switch (Utility.Random(2))
                {
                    case 0:
                        item = new ParasiticPotion(Utility.Random(1, 3)); break;
                    case 1:
                        item = new InvisibilityPotion(Utility.Random(1, 3)); break;
                }

                DropItem(item);
            }

            if (0.2 > Utility.RandomDouble())
            {
                item = Loot.RandomEssence();
                item.Amount = Utility.Random(3, 6);
                DropItem(item);
            }

            switch (Utility.Random(4))
            {
                case 0: DropItem(new Taint()); break;
                case 1: DropItem(new Corruption()); break;
                case 2: DropItem(new Blight()); break;
                case 3: DropItem(new LuminescentFungi()); break;
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
