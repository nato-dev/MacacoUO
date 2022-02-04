using System;
using Server.Mobiles;

namespace Server.Items
{ 
	public class MasterCraftsmanTalisman : BaseTalisman
    {
        public override bool IsArtifact { get { return true; } }
    
        private int _Type;
        public virtual int Type { get { return _Type; } }

        [Constructable]
        public MasterCraftsmanTalisman(int charges, int itemID, TalismanSkill skill)
            : base(itemID)
        {
            Skill = skill;

            SuccessBonus = GetRandomSuccessful();
            ExceptionalBonus = BaseTalisman.GetRandomExceptional();
            Blessed = GetRandomBlessed();

            _Type = charges;
			Charges = charges;
		}

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add("Protecao contra falhar Craftar"); // Crafting Failure Protection

            if (Charges > 0)
                list.Add(1049116, Charges.ToString()); // [ Charges: ~1_CHARGES~ ]
        }

        public MasterCraftsmanTalisman(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName {  get { return "Talisman de " + Skill.ToString(); ; } }
        public override bool ForceShowName
        {
            get
            {
                return true;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write(_Type);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    _Type = reader.ReadInt();
                    break;
                case 0:
                    _Type = 10;
                    break;
            }
        }
    }
}
