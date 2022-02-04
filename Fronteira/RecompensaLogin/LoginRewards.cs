using Server.Engines.Points;
using Server.Gumps;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ziden.RecompensaLogin
{
    public class LoginRewardsGump : BaseRewardGump
    {
        public LoginRewardsGump(Mobile owner, PlayerMobile user)
            : base(owner, user, RecompensasLogin.Rewards, "Recompensas de Atividade")
        {
        }

        public override double GetPoints(Mobile m)
        {
            return PointsSystem.PontosLogin.GetPoints(m);
        }

        public override void OnConfirmed(CollectionItem citem, int index)
        {
            Item item;

            item = Activator.CreateInstance(citem.Type) as Item;

            if (item != null)
            {
                if (User.Backpack == null || !User.Backpack.TryDropItem(User, item, false))
                {
                    User.SendMessage("Sem espaco na mochila"); // The reward could not be given.  Make sure you have room in your pack.
                    item.Delete();
                }
                else
                {
                    PointsSystem.PontosLogin.DeductPoints(User, citem.Points);

                    User.SendMessage("Pegou a recompensa"); // Your reward has been placed in your backpack.
                    User.PlaySound(0x5A7);
                }
            }
        }
    }
}
