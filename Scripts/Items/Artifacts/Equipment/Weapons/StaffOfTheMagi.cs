using System;

namespace Server.Items
{
    public class StaffOfTheMagi : BlackStaff
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public StaffOfTheMagi()
        {
            Hue = 0x481;
            WeaponAttributes.MageWeapon = 30;
            Attributes.SpellChanneling = 1;
            Attributes.Resistence = 1;
            Attributes.WeaponDamage = 50;
        }

        public StaffOfTheMagi(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1061600;
            }
        }// Staff of the Magi
        public override int ArtifactRarity
        {
            get
            {
                return 11;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 255;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 255;
            }
        }
        public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
        {
            phys = fire = cold = pois = chaos = direct = 0;
            nrgy = 100;
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
}