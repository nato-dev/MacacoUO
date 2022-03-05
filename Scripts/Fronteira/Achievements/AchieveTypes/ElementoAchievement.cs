
using Server;
using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Mythik.Systems.Achievements.AchieveTypes
{
    /// <summary>
    /// Achievement to handle hitting X in skill Y
    /// Comment out if you are missing the SkillGain eventsink
    /// </summary>
    class ElementoAchievement : BaseAchievement
    {
        private ElementoPvM e;
        public ElementoAchievement(int id, int catid, int itemIcon, bool hiddenTillComplete, BaseAchievement prereq, int total, string title, string desc, ElementoPvM elemento, short RewardPoints, params Type[] rewards)
            : base(id, catid, itemIcon, hiddenTillComplete, prereq, title, desc, RewardPoints, total, rewards)
        {
            e = elemento;
            EventSink.OnKilledBy += EventSink_OnKilledBy;
        }

        private void EventSink_OnKilledBy(OnKilledByEventArgs e)
        {
            var player = e.KilledBy as PlayerMobile;
            if (player != null && player.Elemento == this.e && player.Elementos.GetNivel(this.e) >= CompletionTotal)
            {
                AchievementSystem.SetAchievementStatus(player, this, CompletionTotal);
            }
        }


    }
}
