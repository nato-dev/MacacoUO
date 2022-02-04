using System;
using System.Collections.Generic;
using System.Text;
using Server.Items;

namespace Server.Engines.Craft
{

    public class DecoLoader
    {
        public static List<Type> Arties = new List<Type>();

        public static void Initialize()
        {
            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                var deco = ScriptCompiler.GetAllItemsOfBase(typeof(BaseDecorationArtifact));
                foreach(var de in deco)
                {
                    if(!(de is RandomDecoArt))
                        Arties.Add(de);
                }
            });
        }

        public static Item RandomArty()
        {
            try
            {
                var type = Arties[Utility.Random(Arties.Count)];
                return (Item)Activator.CreateInstance(type);
            } catch(Exception e)
            {
                return new BlankScroll();
            }
        }
    }

    public class RandomDecoArt : BaseDecorationArtifact
    {

        public override int ArtifactRarity
        {
            get { return 1; }
        }

        [Constructable]
        public RandomDecoArt()
            : base(0x1BF1)
        {
            Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
            {
                Shard.Debug("Criando UOF DEco");
                var arty = DecoLoader.RandomArty();
                this.ItemID = arty.ItemID;
                this.Name = arty.Name;
                if(this.Name == null)
                {
                    this.Name = "Artefato de Dungeon";
                }
                arty.Delete();
            });
        }

        public RandomDecoArt(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
    //**Stack of Logs

    //**Stack of Ingots
    [FlipableAttribute(0x1BF1, 0x1BF4)]
    public class StackofIngots : Item
    {
      

        [Constructable]
        public StackofIngots()
            : base(0x1BF1)
        {
            Name = "Pilha de lingotes";
            Weight = 5.0;
        }

        public StackofIngots(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
    //**Stack of Logs
    [FlipableAttribute(0x1BDF, 0x1BE2)]
    public class StackofLogs : Item
    {
        [Constructable]
        public StackofLogs()
            : base(0x1BDF)
        {
            Name = "Pilha de Madeiras";
            Weight = 5.0;
        }

        public StackofLogs(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    //DECORATIVE ARMOR
    public class AncientArmor : Item
    {
        [Constructable]
        public AncientArmor()
        {
            Name = "pedaco de armadura";
            switch (Utility.Random(7))
            {
                case 0: ItemID = 7026; break;
                case 1: ItemID = 5102; break;
                case 2: ItemID = 5104; break;
                case 3: ItemID = 5099; break;
                case 4: ItemID = 5100; break;
                case 5: ItemID = 5134; break;
                case 6: ItemID = 5139; break;
            }
            Hue = 2718;
            Weight = 5.0;
        }

        public AncientArmor(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class AncientArmorNew : BaseArmor
    {
        public override int InitMinHits { get { return 50; } }
        public override int InitMaxHits { get { return 65; } }


        public override int OldStrReq { get { return 30; } }

        public override int OldDexBonus { get { return -1; } }

        public override int ArmorBase { get { return 0; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

        [Constructable]
        public AncientArmorNew(int itemid)
            : base(itemid)
        {
            Name = "armadura antiga";
            Hue = 2718;
            Weight = 5.0;
            Identified = true;
        }

        public AncientArmorNew(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    //**Cannon*
    public class CannonBall2 : Item
    {
        [Constructable]
        public CannonBall2()
            : this(1)
        {
        }

        [Constructable]
        public CannonBall2(int amount)
            : base(0xE73)
        {
            Name = "bola de canhao";
            Stackable = true;
            Weight = 5.0;
            Amount = amount;
        }

        public CannonBall2(Serial serial)
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
    public class Gunpowder2 : Item
    {
        public override double DefaultWeight
        {
            get { return 0.1; }
        }

        [Constructable]
        public Gunpowder2()
            : this(1)
        {
        }

        [Constructable]
        public Gunpowder2(int amount)
            : base(3980)
        {
            Name = "polvora";
            Stackable = true;
            Hue = 243;
            Amount = amount;
        }

        public Gunpowder2(Serial serial)
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

    public class CannonFuse : Item
    {
        public override double DefaultWeight
        {
            get { return 0.1; }
        }

        [Constructable]
        public CannonFuse()
            : base(3613)
        {
            Name = "pavio de canhao";
            Stackable = true;
            Hue = 1172;
            Weight = 16;
        }

        public CannonFuse(Serial serial)
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

    //Singing Crystal Ball
    public class CrackedCrystalBall : Item
    {

        [Constructable]
        public CrackedCrystalBall() : base(0xE2E)
        {
            Name = "bola de cristal quebrada";
            Movable = true;
            Stackable = false;
            Hue = 2598;
            Weight = 16;
        }

        public CrackedCrystalBall(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SeersPowder : Item
    {

        [Constructable]
        public SeersPowder()
            : base(0x26B8)
        {
            Name = "pozinho do sabio";
            Movable = true;
            Stackable = true;
            Hue = 2598;
        }

        public SeersPowder(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
    //End Singing Crystal Ball

    //Begin Dimensional Gateway
    public class EnchantedMarble : Item
    {
        [Constructable]
        public EnchantedMarble()
            : base(13040)
        {
            Name = "perola encantanda";
            Movable = true;
            Stackable = false;
            Hue = 2947;
            Weight = 20;
        }

        public EnchantedMarble(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
    //End Dimensional Gateway
}

