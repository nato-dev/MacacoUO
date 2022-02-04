
#region References
using Server;

using VitaNex.Items;
using VitaNex.Mobiles;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Gumps;
using Server.ContextMenus;
using System.Collections.Generic;
using Server.Items.Functional.Pergaminhos;
#endregion

namespace VitaNex.Modules.BlackMarketVendor
{
    public class VendedorMagico : AdvancedVendor
    {
        [Constructable]
        public VendedorMagico()
            : base("Vendedor Magico", typeof(MoedaMagica), "Moedas Magicas", "MM", true)

        { }

        public VendedorMagico(Serial serial)
            : base(serial)
        { }

        protected override void InitBuyInfo()
        {

            AddStock<EtherealLlama>(300);
            AddStock<PergaminhoSagrado>(100);
            //Player Weapons/Armor/Clothing
            //AddStock<RAD>(25);
            //AddStock<RandomArtifactWeaponDeed>(50);
            //AddStock<RandomArtifactArmorDeed>(50);
            //AddStock<RandomArtifactClothingDeed>(50);
            //AddStock<RandomArtifactJeweleryDeed>(50);
            //Player buffs/Utilities
            //AddStock<ScrollOfResurrection>(5);
            //Animal Buffs/Utilities
            //AddStock<PetBondingPotion>(5);
            //Champs
            //AddStock<GoldenSkull>(500);
            //PvP/Pk
            //AddStock<TenMurdererKillRemoverDeed>(100);
            //Player Customization

            AddStock<HoodedShroudOfShadows>(5);
            //AddStock<ShdroudOfTheCondemned>(35);
            //Mounts
            //AddStock<EtherealAncientHellHound>(500);
            //AddStock<EtherealCuSidhe>(500);
            //AddStock<EtherealLasher>(500);
            //AddStock<EtherealTiger>(500);

            //Decorations



        }



        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            var version = writer.SetVersion(0);

            switch (version)
            {
                case 0:
                    break;
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.GetVersion();

            switch (version)
            {
                case 0:
                    break;
            }
        }
    }
}
