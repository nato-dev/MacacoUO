using Server.Commands;
using Server.Engines.Craft;
using Server.Engines.Points;
using Server.Engines.VvV;
using Server.Gumps;
using Server.Mobiles;
using Server.Spells;
using System;

namespace Server.Ziden.Kills
{
    public class PontosTrabalho : PointsSystem
    {
        public override TextDefinition Name { get { return "Trabalho"; } }
        public override PointsType Loyalty { get { return PointsType.Trabalho; } }
        public override bool AutoAdd { get { return true; } }
        public override double MaxPoints { get { return double.MaxValue; } }
        public override bool ShowOnLoyaltyGump { get { return true; } }
        public static bool Enabled = true;

        public static void Initialize()
        {
            EventSink.CraftSuccess += Craft;
        }

        public static void Craft(CraftSuccessEventArgs e)
        {
            var pontos = 0;
            var craft = e.CraftItem as CraftItem;
            if(craft != null)
            {
                foreach(var i in craft.m_arCraftSkill)
                {
                    var skill = (CraftSkill)i;
                    pontos += (int)((skill.MinSkill - craft.MinSkillOffset) / 10);
                }
            }
            if(pontos != 0)
            {
                PointsSystem.PontosTrabalho.AwardPoints(e.Crafter, pontos, false, false);
            }
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
