using System;
using Server;
using Server.Items;

namespace Server.Engines.Craft
{
	public class DefFenceCrafting : CraftSystem
	{
        public override SkillName MainSkill { get { return SkillName.Carpentry; } }

		public override int GumpTitleNumber { get { return 0; } }

        public override string GumpTitleString { get { return "<basefont color=#FFFFFF><CENTER>Cercas</CENTER></basefont>"; } }

		private static CraftSystem m_CraftSystem;

        public static CraftSystem CraftSystem { get { if (m_CraftSystem == null) m_CraftSystem = new DefFenceCrafting(); return m_CraftSystem; } }

		public override double GetChanceAtMin( CraftItem item ) { return 0.5; }

        private DefFenceCrafting() : base(1, 1, 1.25) { }

        public override int CanCraft(Mobile from, ITool tool, Type itemType)
        {
			if ( ((Item)tool).Deleted || tool.UsesRemaining < 0 ) return 1044038;
			return 0;
		}

		public override void PlayCraftEffect( Mobile from )
		{
			from.PlaySound( 0x241 );
		}

		public override int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item )
		{
			if ( toolBroken ) from.SendLocalizedMessage( 1044038 );

			if ( failed )
			{
				if ( lostMaterial ) return 1044043;
				else return 1044157;
			}
			else
			{
				if ( quality == 0 ) return 502785;
				else if ( makersMark && quality == 2 ) return 1044156;
				else if ( quality == 2 ) return 1044155;
				else return 1044154;
			}
		}

		public override void InitCraftList()
		{
			int index = -1;
            index = AddCraft(typeof(Fence), "Cerca", "Cerca", 30.5, 70.5, typeof(Board), "Tabua", 4, "Faltam tabuas.");
            AddRes(index, typeof(FencePost), "Pilar da Cerca", 2, "Voce precisa de pilares");

            index = AddCraft(typeof(FenceCorner), "Cerca", "Cerca de Quina", 35.0, 75.0, typeof(Board), "Board", 8, "Faltam Tabuas.");
            AddRes(index, typeof(FencePost), "Pilar da Cerca", 1, "Voce precisa de pilares");

            index = AddCraft(typeof(FencePost), "Cerca", "Pilar de Cerca", 10.0, 25.0, typeof(Log), "Tora", 1, "Voce precisa de uma tora.");

            index = AddCraft(typeof(NorthGate), "Cerca", "Portao Norte", 50.0, 90.0, typeof(Board), "Board", 7, "Faltam tabuas.");
            AddRes(index, typeof(Hinge), "Dobradica", 2, "Faltam dobradicas");

            index = AddCraft(typeof(WestGate), "Cerca", "Portao Oeste", 50.0, 90.0, typeof(Board), "Board", 7, "Faltam tabuas.");
            AddRes(index, typeof(Hinge), "Dobradica", 2, "Faltam dobradicas");

			MarkOption = true;
			Repair = false;
		}
	}
}
