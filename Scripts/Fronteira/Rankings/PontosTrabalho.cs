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
                    var pts = (int)((skill.MinSkill - craft.MinSkillOffset) / 10);
                    pontos += pts;
                    if(skill.SkillToMake==SkillName.Blacksmith)
                        PointsSystem.PontosFerreiro.AwardPoints(e.Crafter, pts, false, false);
                    else if (skill.SkillToMake == SkillName.Carpentry)
                        PointsSystem.PontosCarpinteiro.AwardPoints(e.Crafter, pts, false, false);
                    else if (skill.SkillToMake == SkillName.Tailoring)
                        PointsSystem.PontosAlfaiate.AwardPoints(e.Crafter, pts, false, false);
                    else if (skill.SkillToMake == SkillName.Alchemy)
                        PointsSystem.PontosAlquimista.AwardPoints(e.Crafter, pts, false, false);
                    else if (skill.SkillToMake == SkillName.Cooking)
                        PointsSystem.PontosCozinha.AwardPoints(e.Crafter, pts, false, false);
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

    public class PontosFerreiro : PointsSystem
    {
        public override TextDefinition Name { get { return "Pts Ferreiro"; } }
        public override PointsType Loyalty { get { return PointsType.Ferreiro; } }
        public override bool AutoAdd { get { return true; } }
        public override double MaxPoints { get { return double.MaxValue; } }
        public override bool ShowOnLoyaltyGump { get { return true; } }
        public static bool Enabled = true;
    }

    public class PontosAlfaiate : PointsSystem
    {
        public override TextDefinition Name { get { return "Pts Alfaiate"; } }
        public override PointsType Loyalty { get { return PointsType.Alfaiate; } }
        public override bool AutoAdd { get { return true; } }
        public override double MaxPoints { get { return double.MaxValue; } }
        public override bool ShowOnLoyaltyGump { get { return true; } }
        public static bool Enabled = true;
    }

    public class PontosLenhador : PointsSystem
    {
        public override TextDefinition Name { get { return "Pts Lenhador"; } }
        public override PointsType Loyalty { get { return PointsType.Lenhador; } }
        public override bool AutoAdd { get { return true; } }
        public override double MaxPoints { get { return double.MaxValue; } }
        public override bool ShowOnLoyaltyGump { get { return true; } }
        public static bool Enabled = true;
    }

    public class PontosPescador : PointsSystem
    {
        public override TextDefinition Name { get { return "Pts Pescador"; } }
        public override PointsType Loyalty { get { return PointsType.Pescador; } }
        public override bool AutoAdd { get { return true; } }
        public override double MaxPoints { get { return double.MaxValue; } }
        public override bool ShowOnLoyaltyGump { get { return true; } }
        public static bool Enabled = true;
    }

    public class PontosMinerador : PointsSystem
    {
        public override TextDefinition Name { get { return "Pts Minerador"; } }
        public override PointsType Loyalty { get { return PointsType.Minerador; } }
        public override bool AutoAdd { get { return true; } }
        public override double MaxPoints { get { return double.MaxValue; } }
        public override bool ShowOnLoyaltyGump { get { return true; } }
        public static bool Enabled = true;
    }

    public class PontosCarpinteiro : PointsSystem
    {
        public override TextDefinition Name { get { return "Pts Carpinteiro"; } }
        public override PointsType Loyalty { get { return PointsType.Carpinteiro; } }
        public override bool AutoAdd { get { return true; } }
        public override double MaxPoints { get { return double.MaxValue; } }
        public override bool ShowOnLoyaltyGump { get { return true; } }
        public static bool Enabled = true;
    }

    public class PontosAlquimista : PointsSystem
    {
        public override TextDefinition Name { get { return "Pts Alquimista"; } }
        public override PointsType Loyalty { get { return PointsType.Alquimista; } }
        public override bool AutoAdd { get { return true; } }
        public override double MaxPoints { get { return double.MaxValue; } }
        public override bool ShowOnLoyaltyGump { get { return true; } }
        public static bool Enabled = true;
    }

    public class PontosCozinha : PointsSystem
    {
        public override TextDefinition Name { get { return "Pts Cozinha"; } }
        public override PointsType Loyalty { get { return PointsType.Cozinha; } }
        public override bool AutoAdd { get { return true; } }
        public override double MaxPoints { get { return double.MaxValue; } }
        public override bool ShowOnLoyaltyGump { get { return true; } }
        public static bool Enabled = true;
    }

    public class PontosOuro : PointsSystem
    {
        public override TextDefinition Name { get { return "Pts Ouro"; } }
        public override PointsType Loyalty { get { return PointsType.Ouro; } }
        public override bool AutoAdd { get { return true; } }
        public override double MaxPoints { get { return double.MaxValue; } }
        public override bool ShowOnLoyaltyGump { get { return true; } }
        public static bool Enabled = true;
    }
}
