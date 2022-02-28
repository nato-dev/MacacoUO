namespace Server.Items
{
    public class MasterChefsApron : FullApron
    {
        private int _Bonus;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Bonus { get { return _Bonus; } set { _Bonus = value; InvalidateProperties(); } }

        public override int LabelNumber { get { return 1157228; } } // Master Chef's Apron

        [Constructable]
        public MasterChefsApron()
        {
            Hue = 1990;
            Name = "Avental do Trabalhador";
            while (_Bonus == 0)
                _Bonus = Utility.Random(2, 13);
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add("+"+_Bonus+"% Crafting Exceptional"); // ~1_NAME~ Exceptional Bonus: ~2_val~%
        }

        public MasterChefsApron(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(_Bonus);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            _Bonus = reader.ReadInt();
        }
    }

    public class CarpenterApron : FullApron
    {
        private int _Bonus;
        private SkillName _Skill;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Bonus { get { return _Bonus; } set { _Bonus = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public SkillName Skill { get { return _Skill; } set { _Skill = value; InvalidateProperties(); } }

        [Constructable]
        public CarpenterApron()
        {
            Hue = 1990;
            Name = "Avental do Artesao";
            Bonus = Utility.Random(5, 30);
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add("+" + _Bonus + "% "+ _Skill.ToString() + " Exceptional"); // ~1_NAME~ Exceptional Bonus: ~2_val~%
        }

        public CarpenterApron(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write(_Bonus);
            writer.Write((int)_Skill);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            _Bonus = reader.ReadInt();
            if (version >= 1)
                _Skill = (SkillName)reader.ReadInt();
            else
                _Skill = SkillName.Carpentry;
        }
    }
}
