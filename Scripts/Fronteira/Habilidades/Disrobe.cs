using System;
using Server.Fronteira.Talentos;
using Server.Mobiles;

namespace Server.Items
{
    /// <summary>
    /// This attack allows you to disrobe your foe.
    /// </summary>
    public class Disrobe : WeaponAbility
    {
        public static readonly TimeSpan BlockEquipDuration = TimeSpan.FromSeconds(5.0);
        public Disrobe()
        {
        }

        public override int BaseMana
        {
            get
            {
                return 20;
            }
        }// Not Sure what amount of mana a creature uses.

        public override Talento TalentoParaUsar { get { return Talento.Hab_Disrobe; } }

        public override void OnHit(Mobile attacker, Mobile defender, int damage)
        {
            if (!this.Validate(attacker))
                return;

            ClearCurrentAbility(attacker);
            Item toDisrobe = defender.FindItemOnLayer(Layer.InnerTorso);			

            if (toDisrobe == null || !toDisrobe.Movable)
                toDisrobe = defender.FindItemOnLayer(Layer.OuterTorso);

            Container pack = defender.Backpack;

            if (pack == null || toDisrobe == null || !toDisrobe.Movable)
            {
                attacker.SendLocalizedMessage("Voce nao pode desarmar isto"); // You cannot disarm your opponent.
            }
            else if (this.CheckMana(attacker, true))
            {
                //attacker.SendLocalizedMessage( 1060092 ); // You disarm their weapon!
                defender.SendLocalizedMessage("Voce nao pode mais usar sua armadura"); // You can no longer wear your ~1_ARMOR~

                defender.PlaySound(0x3B9);
                //defender.FixedParticles( 0x37BE, 232, 25, 9948, EffectLayer.InnerTorso );

                pack.DropItem(toDisrobe);

                BaseWeapon.BlockEquip(defender, BlockEquipDuration);
            }
        }
    }
}
