using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Guilds;
using Server.Network;
using Server.Engines.Points;
using System.Collections.Generic;

namespace Server.Engines.VvV
{
    public class PvmRewardsGump : BaseRewardGump
    {
        public PvmRewardsGump(Mobile owner, PlayerMobile user)
            : base(owner, user, PvmRewards.Rewards, "Recompensas PvM")
        {
        }

        public override double GetPoints(Mobile m)
        {
            return PointsSystem.PontosPvm.GetPoints(m);
        }

        public override void OnConfirmed(CollectionItem citem, int index)
        {
            var item = Activator.CreateInstance(citem.Type) as Item;
            if(item == null)
            {
                User.SendMessage("Erro ao criar o item !");
                return;
            }

            if (User.Backpack == null || !User.Backpack.TryDropItem(User, item, false))
            {
                User.SendLocalizedMessage(1074361); // The reward could not be given.  Make sure you have room in your pack.
                item.Delete();
            }
            else
            {
                if (User.AccessLevel <= AccessLevel.VIP)
                    PointsSystem.PontosPvm.DeductPoints(User, citem.Points);

                User.SendLocalizedMessage(1073621); // Your reward has been placed in your backpack.
                User.PlaySound(0x5A7);
            }
          
        }
    }
}
