using Server.Items;
using Server.Misc.Custom;
using Server.Targeting;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells.Ninjitsu;
using System.IO;
using System;
using System.Collections.Generic;

namespace Server.Ziden
{
    public class BolaDeNeve : Item
    {
        [Constructable]
        public BolaDeNeve() : base(3614)
        {
            this.Name = "Bola De Neve";
            this.Stackable = true;
            this.Weight = 1.0;
            this.Hue = (0x810);
        }

        public BolaDeNeve(Serial s) : base(s) { }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Você pode congelar monstros com isso");
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.Target = new IT(this);
            from.SendMessage("Selecione um alvo");

            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("Precisa estar em sua mochila"); // The BolaDeNeve must be in your pack to use it.
            }

        }

        private class IT : Target
        {
            private Item BolaDeNeve;

            public IT(Item BolaDeNeve) : base(10, false, TargetFlags.None)
            {
                this.BolaDeNeve = BolaDeNeve;

            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                var target = targeted as Mobile;


                if (!(targeted is Mobile))
                {
                    from.SendMessage("Você não pode jogar ai");
                    return;
                }

                if (targeted is Mobile)
                {
                    Mobile to = (Mobile)targeted;

                    if (!this.BolaDeNeve.IsChildOf(from.Backpack))
                    {
                        from.PrivateOverheadMessage("Deve estar na mochila"); // The bola must be in your pack to use it.
                    }

                    else if (from == to)
                    {
                        from.SendMessage("Você não pode jogar em você mesmo"); // You can't throw this at yourself.
                    }

                    else
                    {
                        Item one = from.FindItemOnLayer(Layer.OneHanded);
                        Item two = from.FindItemOnLayer(Layer.TwoHanded);

                        if (one != null)
                            from.AddToBackpack(one);

                        if (two != null)
                            from.AddToBackpack(two);

                        if (!(from.IsCooldown("BolaDeNeve")))
                        {
                            if (target is BaseCreature && !target.IsCooldown("BolaDeNeve") || target.Player)
                            {
                                BolaDeNeve.Consume();

                                from.Animate(AnimationType.Attack, 4);
                                from.PlaySound(0x13C);
                                from.OverheadMessage("* Jogou *");
                                from.MovingEffect(target, 0x3729, 10, 0, false, false);
                                from.SetCooldown("BolaDeNeve", TimeSpan.FromSeconds(3));

                                if (target.Player)
                                {
                                    target.SendMessage("Você não pode congelar Players");
                                }

                                else if (target is BaseCreature && !target.IsCooldown("BolaDeNeve"))
                                {
                                    target.Paralyze(TimeSpan.FromSeconds(3));
                                    target.SetCooldown("BolaDeNeve", TimeSpan.FromSeconds(12));
                                    target.OverheadMessage("* Urghhhh *");
                                }
                            }

                            else if (target.IsCooldown("BolaDeNeve"))
                            {
                                from.SendMessage("Aguarde alguns segundos");
                                return;
                            }
                        }

                        else if (from.IsCooldown("BolaDeNeve"))
                        {
                            from.SendMessage("Aguarde alguns segundos");
                            return;
                        }
                    }
                }
            }
        }
    }
}
