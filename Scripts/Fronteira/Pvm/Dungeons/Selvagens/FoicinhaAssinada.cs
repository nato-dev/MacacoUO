using System;
using Server.Network;
using Server.Targeting;
using Server.Items;

namespace Server.Items
{
    public class FoicinhaAssinada : BaseBoline
    {
        public override WeaponAbility PrimaryAbility { get { return WeaponAbility.InfectiousStrike; } }
        public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ShadowStrike; } }

        public override int AosStrengthReq { get { return 10; } }
        public override int AosMinDamage { get { return 6; } }
        public override int AosMaxDamage { get { return 8; } }
        public override int AosSpeed { get { return 56; } }

        public override int InitMinHits { get { return 31; } }
        public override int InitMaxHits { get { return 40; } }

        public override SkillName DefSkill { get { return SkillName.Swords; } }
        public override WeaponType DefType { get { return WeaponType.Slashing; } }
        public override WeaponAnimation DefAnimation { get { return WeaponAnimation.Slash1H; } }

        [Constructable]
        public FoicinhaAssinada() : this(0x26BB, 50)
        {
            this.Name = "foicinha assinada";
            this.Hue = 200;
        }

        [Constructable]
        public FoicinhaAssinada(int usesremaining) : this(0x26BB, usesremaining)
        {
            this.Name = "foicinha assinada";
            this.Hue = 200;
        }

        [Constructable]
        public FoicinhaAssinada(int itemid, int usesremaining) : base(itemid, usesremaining)
        {
            this.Name = "foicinha assinada";
            this.Hue = 200;
        }

        public FoicinhaAssinada(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }

        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }
}
