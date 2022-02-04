using Server.Mobiles;

namespace Server.Fronteira.Animations
{
    public class CustomAnimations
    {
        public static void GritarDeDorAnimation(PlayerMobile pm)
        {
            pm.Animate(32, 5, 1, true, false, 0);
            pm.PlaySound(pm.Female ? 0x316 : 0x426);
        }
    }
}
