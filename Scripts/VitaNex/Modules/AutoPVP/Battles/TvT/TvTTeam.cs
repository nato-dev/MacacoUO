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
    public class TvTTeam : PvPTeam
    {

        public TvTTeam(PvPBattle battle, string name = "Team", int minCapacity = 0, int maxCapacity = 1, int color = 12)
            : base(battle, name, minCapacity, maxCapacity, color)
        {

        }

        public TvTTeam(PvPBattle battle, GenericReader reader)
            : base(battle, reader)
        { }

        public override void OnMemberRemoved(PlayerMobile pm)
        {
            base.OnMemberRemoved(pm);
            if (this.Members.Count() == 0 && this.Battle.State == PvPBattleState.Batalhando)
            {
                this.Battle.State = PvPBattleState.Terminando;
            }
            
        }
    }
}
