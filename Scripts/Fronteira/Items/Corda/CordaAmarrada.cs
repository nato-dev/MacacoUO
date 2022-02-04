using Server.Gumps;
using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Server.Items
{
    public class CordaAmarrada : Item, IScissorable
    {
        public static Dictionary<PlayerMobile, CordaAmarrada> Arrastando = new Dictionary<PlayerMobile, CordaAmarrada>();

        private object Preso;
        private Rope Corda;

        private static int ITEMID = 0x268C;

        public CordaAmarrada(object preso, Rope corda) : base(ITEMID)
        {
            Corda = corda;
            Preso = preso;
            Movable = false;
            Name = "Corda Amarrada";
        }

        public CordaAmarrada(Serial s) : base(s) { }

        public override void OnDoubleClick(Mobile from)
        {
            var pl = from as PlayerMobile;
            if (Preso is PlayerMobile)
            {
                
                from.SendGump(new GumpOpcoes("O que deseja fazer ?", (opt) =>
                {
                    if (opt == 0)
                    {
                        from.SendMessage("Voce apertou a corda");
                        Corda.Refresh();
                    }
                    else if (opt == 1)
                        Corta(pl, Preso as PlayerMobile);
                    else if (opt == 2)
                        Desamarra(pl, Preso as PlayerMobile);
                    else if (opt == 3)
                        Arrasta(pl, Preso);
                }, 0x14F8, 0, "Apertar o no", "Cortar Corda", "Desamarrar", Arrastando.ContainsKey(pl) ? "Largar" : "Arrastar"));
            } else if(Preso is Item)
            {
                from.SendGump(new GumpOpcoes("O que deseja fazer ?", (opt) =>
                {
                    if (opt == 1)
                        Arrasta(pl, Preso);
                    else if(opt == 0)
                        Desamarra(pl, Preso);
                }, 0x14F8, 0, "Desamarrar", Arrastando.ContainsKey(pl) ? "Largar" : "Arrastar"));
            }

        }

        public bool Scissor(Mobile from, Scissors scissors)
        {
            if (Preso is PlayerMobile)
                Corta(from as PlayerMobile, Preso as PlayerMobile, false);
            return true;
        }

        public void Arrasta(PlayerMobile from, object target)
        {
            var mob = target as Mobile;
            if (!mob.Alive && mob.Corpse != null)
                target = mob.Corpse;

            if (from.Str < 90)
            {
                from.SendMessage("Voce nao e forte suficiente para arrastar isto");
                return;
            }


            if (from.GetDistance((IPoint3D)target) > 2)
            {
                from.SendMessage("Muito longe");
                return;
            }

            if (!Arrastando.ContainsKey(from))
            {
                Arrastando.Add(from, this);
                from.SendMessage("Voce agarrou o alvo e esta pronto para arrasta-lo");
                from.OverheadMessage("* segurou firme *");
                from.Arrastando = target;
            }
            else
            {
                Arrastando.Remove(from);
                from.SendMessage("Voce soltou o alvo");
                from.OverheadMessage("* largou *");
                from.Arrastando = null;
            }
        }

        public void Desamarra(PlayerMobile from, object target)
        {
            if(target is PlayerMobile)
            {
                from.OverheadMessage("* desamarrando *");
                Timer.DelayCall(TimeSpan.FromSeconds(3), () =>
                {
                    DesamarraTick(0, from, (PlayerMobile)target);
                });
            } else if(target is Item)
            {
                from.OverheadMessage("* desamarrou *");
                from.Arrastando = null;
                this.Corda.TrySolta(target);
            }
           
        }

        private void DesamarraTick(int tick, PlayerMobile from, PlayerMobile target)
        {
            if (!from.Alive || !target.Alive)
                return;

            if (from.GetDistance(target) > 2)
                return;


            var chanceSucesso = from.Dex / 5 + tick * from.Str / 10;
            if (Utility.Random(100) < chanceSucesso)
            {
                Corda.TrySolta(target);
                return;
            }
            else
            {
                tick++;
                Timer.DelayCall(TimeSpan.FromSeconds(3), () =>
                {
                    DesamarraTick(tick, from, target);
                });
                from.OverheadMessage("* tentando desamarrar *");
            }
        }

        public void Corta(PlayerMobile from, PlayerMobile target, bool checaArma = true)
        {
            if (checaArma)
            {
                BaseWeapon arma = from.Backpack.FindItemByType<BaseKnife>();
                if (arma == null)
                    arma = from.Backpack.FindItemByType<BaseAxe>();
                if (arma == null)
                    arma = from.Backpack.FindItemByType<BaseSword>();

                if (arma == null || !from.SmoothForceEquip(arma))
                {
                    from.SendMessage("Voce precisa de alguma arma cortante para cortar a corda");
                    return;
                }
             
            }
            from.PlayAttackAnimation();
            from.OverheadMessage("* cortou a corda *");
            this.Delete();
            Corda.TrySolta(target);

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(Preso as PlayerMobile);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var mob = reader.ReadMobile();
            if (mob != null)
            {
                Preso = mob;
                if (mob.Frozen)
                {
                    mob.Frozen = false;
                }
                Delete();
            }
        }
    }

}
