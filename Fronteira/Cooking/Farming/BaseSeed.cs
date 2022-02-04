using Server.Items;
using Server.Items.Crops;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Items.Crops
{
    public abstract class BaseSeed : HerdingBaseCrop
    {

        public BaseSeed(int itemID) : base(itemID)
        {

        }

        public abstract Item GetSeedling(Mobile from);

        public override void OnDoubleClick(Mobile from)
        {
            Point3D m_pnt = from.Location;
            Map m_map = from.Map;
            if (!IsChildOf(from.Backpack)) { from.SendLocalizedMessage(1042010); return; }
            else if (!CropHelper.CheckCanGrow(this, m_map, m_pnt.X, m_pnt.Y)) { from.SendMessage("Esta semente nao vai crescer aqui."); return; }
            ArrayList cropshere = CropHelper.CheckCrop(m_pnt, m_map, 0);
            if (cropshere.Count > 0) { from.SendMessage("Ja existe uma plantacao aqui."); return; }
            ArrayList cropsnear = CropHelper.CheckCrop(m_pnt, m_map, 1);
            if ((cropsnear.Count > 1)) { from.SendMessage("Muitas plantacoes perto."); return; }
            if (this.BumpZ) ++m_pnt.Z;
            from.PlayAttackAnimation();
            from.SendMessage("Voce plantou a semente.");
            this.Consume();
            Item item = GetSeedling(from);
            item.Location = m_pnt;
            item.Map = m_map;
        }
    }
}
