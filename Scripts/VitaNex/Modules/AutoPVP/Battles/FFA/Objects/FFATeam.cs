#region Header
//   Vorspire    _,-'/-'/  CTFTeam.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2018  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Server;
using Server.Mobiles;

using VitaNex.SuperGumps;
#endregion

namespace VitaNex.Modules.AutoPvP.Battles
{
    public class FFATeam : PvPTeam
    {

        public FFATeam(PvPBattle battle, string name = "Team", int minCapacity = 0, int maxCapacity = 1, int color = 12)
            : base(battle, name, minCapacity, maxCapacity, color)
        {

        }

        public FFATeam(PvPBattle battle, GenericReader reader)
            : base(battle, reader)
        { }



        public override void OnMemberRemoved(PlayerMobile pm)
        {
            if(Shard.DebugEnabled)
            {
                Shard.Debug("Member count: " + this.Members.Count());
            }
            base.OnMemberRemoved(pm);
            if (this.Members.Count() == 1 && this.Battle.State == PvPBattleState.Batalhando)
            {
                this.Battle.State = PvPBattleState.Terminando;
            }
            
        }
    }
}
