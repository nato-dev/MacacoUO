#region References
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using Server.Commands;
using Server.Items;
using Server.Mobiles;
#endregion

namespace Server.Regions
{
	public class DungeonProtectedRegion : DungeonRegion
	{
		private static readonly object[] m_GuardParams = new object[1];

		private readonly Type m_GuardType;

		private bool m_Disabled;


		public DungeonProtectedRegion(XmlElement xml, Map map, Region parent)
			: base(xml, map, parent)
		{
	
		}

        public override bool AllowHarmful(Mobile from, IDamageable target)
        {
            if (from is PlayerMobile && target is PlayerMobile)
                return false;
            return true;
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }
    }
}
