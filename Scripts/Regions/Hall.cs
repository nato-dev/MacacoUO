using System;
using System.Xml;

namespace Server.Regions
{
    public class Hall : GuardedRegion
    {
        public Hall(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
        }

        public override bool AllowHarmful(Mobile from, IDamageable target)
        {
            return false;
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }

        /*
        public override bool OnBeginSpellCast(Mobile from, ISpell s)
        {
            if (from.IsPlayer())
            {
                from.SendLocalizedMessage(502629); // You cannot cast spells here.
                return false;
            }

            return base.OnBeginSpellCast(from, s);
        }
        */

        public override bool OnCombatantChange(Mobile from, IDamageable Old, IDamageable New)
        {
            return (from.IsStaff());
        }
    }
}
