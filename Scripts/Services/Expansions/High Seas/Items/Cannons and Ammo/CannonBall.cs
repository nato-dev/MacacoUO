using System;
using Server;

namespace Server.Items
{
    public class CannonBallHS : Item, ICommodity, ICannonAmmo
    {
        public override int LabelNumber { get { return 1116266; } }
        public override double DefaultWeight { get { return 1.0; } }

        TextDefinition ICommodity.Description { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }

        public AmmoType AmmoType { get { return AmmoType.Cannonball; } }

        [Constructable]
        public CannonBallHS() : this(1)
        {
        }

        [Constructable]
        public CannonBallHS(int amount) : this(amount, 16932)
        {
        }

        [Constructable]
        public CannonBallHS(int amount, int itemid)
            : base(itemid)
        {
            Stackable = true;
            Amount = amount;
        }

        public CannonBallHS(Serial serial) : base(serial) { }

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

    public class HeavyCannonball : Item, ICommodity, ICannonAmmo
    {
        public override int LabelNumber { get { return 1116267; } }
        public override double DefaultWeight { get { return 1.0; } }

        TextDefinition ICommodity.Description { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }

        public AmmoType AmmoType { get { return AmmoType.Cannonball; } }

        [Constructable]
        public HeavyCannonball() : this(1)
        {
        }

        [Constructable]
        public HeavyCannonball(int amount) : this(amount, 16932)
        {
        }

        public HeavyCannonball(int amount, int itemID) : base(itemID)
        {
            Stackable = true;
            Amount = amount;
        }

        public HeavyCannonball(Serial serial) : base(serial) { }

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

    public class LightFlameCannonball : CannonBallHS, ICommodity
    {
        public override int LabelNumber { get { return 1116759; } }

        TextDefinition ICommodity.Description { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }

        [Constructable]
        public LightFlameCannonball() : this(1)
        {
        }

        [Constructable]
        public LightFlameCannonball(int amount) : base(amount, 17601)
        {
        }

        public LightFlameCannonball(Serial serial) : base(serial) { }

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

    public class HeavyFlameCannonball : HeavyCannonball, ICommodity
    {
        public override int LabelNumber { get { return 1116267; } }

        TextDefinition ICommodity.Description { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }

        [Constructable]
        public HeavyFlameCannonball()
            : this(1)
        {
        }

        [Constructable]
        public HeavyFlameCannonball(int amount)
            : base(amount, 17601)
        {
        }

        public HeavyFlameCannonball(Serial serial) : base(serial) { }

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

    public class LightFrostCannonball : CannonBallHS, ICommodity
    {
        public override int LabelNumber { get { return 1116759; } }

        TextDefinition ICommodity.Description { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }

        [Constructable]
        public LightFrostCannonball()
            : this(1)
        {
        }

        [Constructable]
        public LightFrostCannonball(int amount)
            : base(amount, 16939)
        {
        }

        public LightFrostCannonball(Serial serial) : base(serial) { }

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

    public class HeavyFrostCannonball : HeavyCannonball, ICommodity
    {
        public override int LabelNumber { get { return 1116267; } }

        TextDefinition ICommodity.Description { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return true; } }

        [Constructable]
        public HeavyFrostCannonball()
            : this(1)
        {
        }

        [Constructable]
        public HeavyFrostCannonball(int amount)
            : base(amount, 16939)
        {
        }

        public HeavyFrostCannonball(Serial serial) : base(serial) { }

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
}
