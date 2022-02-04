namespace Server.Items
{
    public class BagOfReagents : Bag
    {
        [Constructable]
        public BagOfReagents()
            : this(50)
        {
            this.Name = "Sacola de Reagentes de Mago";
        }

        [Constructable]
        public BagOfReagents(int amount)
        {
            this.DropItem(new BlackPearl(amount));
            this.DropItem(new Bloodmoss(amount));
            this.DropItem(new Garlic(amount));
            this.DropItem(new Ginseng(amount));
            this.DropItem(new MandrakeRoot(amount));
            this.DropItem(new Nightshade(amount));
            this.DropItem(new SulfurousAsh(amount));
            this.DropItem(new SpidersSilk(amount));          
        }

        public BagOfReagents(Serial serial)
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

    public class BagOfArrows : Bag
    {
        [Constructable]
        public BagOfArrows()
        {
            this.Name = "Sacola de Flechas";
            this.AddItem(new Arrow(100));
        }



        public BagOfArrows(Serial serial)
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

    public class KegGH : PotionKeg
    {
        [Constructable]
        public KegGH(): base()
        {
            Type = PotionEffect.CuraMaior;
            Held = 100;
            
        }

        public KegGH(Serial serial)
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

    public class KegCure : PotionKeg
    {
        [Constructable]
        public KegCure() : base()
        {
            Type = PotionEffect.CuraMaior;
            Held = 100;

        }

        public KegCure(Serial serial)
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

    public class KegMana : PotionKeg
    {
        [Constructable]
        public KegMana() : base()
        {
            Type = PotionEffect.Mana;
            Held = 100;

        }

        public KegMana(Serial serial)
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

    public class KegManaMaior : PotionKeg
    {
        [Constructable]
        public KegManaMaior() : base()
        {
            Type = PotionEffect.ManaForte;
            Held = 100;

        }

        public KegManaMaior(Serial serial)
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

    public class KegStamina : PotionKeg
    {
        [Constructable]
        public KegStamina() : base()
        {
            Type = PotionEffect.Stamina;
            Held = 100;
        }

        public KegStamina(Serial serial)
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

    public class BagOfBolts : Bag
    {
        [Constructable]
        public BagOfBolts()
        {
            this.Name = "Sacola de Dardos";
            this.AddItem(new Bolt(100));
        }



        public BagOfBolts(Serial serial)
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
