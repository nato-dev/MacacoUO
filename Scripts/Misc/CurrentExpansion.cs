#region References
using System;

using Server.Accounting;
using Server.Network;
using Server.Services.TownCryer;
#endregion

namespace Server
{
	public class CurrentExpansion
	{
		public static readonly Expansion Expansion = Config.GetEnum("Expansion.CurrentExpansion", Expansion.EJ);

		[CallPriority(Int32.MinValue)]
		public static void Configure()
		{
			Core.Expansion = Expansion.LBR;

			AccountGold.Enabled = Core.TOL;
			AccountGold.ConvertOnBank = true;
			AccountGold.ConvertOnTrade = false;
			VirtualCheck.UseEditGump = true;
            
			TownCryerSystem.Enabled = Core.TOL;

			ObjectPropertyList.Enabled = true;
            PacketHandlers.SingleClickProps = false;

            Mobile.InsuranceEnabled = true;
			Mobile.VisibleDamageType = VisibleDamageType.None;
			Mobile.GuildClickMessage = !Core.AOS;
			Mobile.AsciiClickMessage = !Core.AOS;
            Mobile.EquipItemDelay = 500; // Core.TOL ? 500 : Core.AOS ? 1000 : 500;

            if (!Core.AOS)
			{
				return;
			}

			AOS.DisableStatInfluences();

			if (ObjectPropertyList.Enabled)
			{
				PacketHandlers.SingleClickProps = true; // single click for everything is overriden to check object property list
			}

           
			Mobile.AOSStatusHandler = AOS.GetStatus;
		}
	}
}
