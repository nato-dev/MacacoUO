using Server.Items;
using Server.Misc.Custom;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;
using System;

namespace Server.Ziden
{
    public class AnelNecro : BaseRing
    {
        [Constructable]
        public AnelNecro() : base(0x108a)
        {
            this.Name = "Anel do Necromante";
            this.Hue = TintaPreta.COR;
        }

        public AnelNecro(Serial s) : base(s) { }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Tem uma caveira de cristal");
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("");
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

    public class AnelChiv : BaseRing
    {
        [Constructable]
        public AnelChiv() : base(0x108a)
        {
            this.Name = "Anel do Paladino";
            this.Hue = TintaBranca.COR;
        }

        public AnelChiv(Serial s) : base(s) { }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Tem uma cruz de ouro");
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("");
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

    public class AnelNinja : BaseRing
    {
        [Constructable]
        public AnelNinja() : base(0x108a)
        {
            this.Name = "Anel do Ninja";
        }

        public AnelNinja(Serial s) : base(s) { }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Tem um simbolo no anel");
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("");
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
