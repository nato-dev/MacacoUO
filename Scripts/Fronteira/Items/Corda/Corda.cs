
using Server.Fronteira.Items.Corda;
using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;
using System;
using System.Collections.Generic;

namespace Server.Items
{

    public class Rope : Item
    {
        [Constructable]
        public Rope() : base(0x14F8)
        {
            Name = "Corda";
            Weight = 6;
        }

        public Rope(Serial s) : base(s)
        {

        }

        public static int ITEMID_PRESO = 0x268C;

        private static Dictionary<Mobile, CordaAmarrada> _presos = new Dictionary<Mobile, CordaAmarrada>();
        public PlayerMobile QuemPrendeu;
        private Timer timerPrisao;
        
        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Permite amarrar jogadores desmaiados");
        }

        public void Refresh()
        {

        }

        public override void OnDoubleClick(Mobile from)
        {
            if (this.RootParent != from)
            {
                from.SendMessage("Precisa estar em sua mochila");
                return;
            }
            from.SendMessage("Selecione o que deseja amarrar");
            from.Target = new AlvoDaCorda(this);
        }

        public static bool Preso(Mobile p)
        {
            if (p == null)
                return false;
            return _presos.ContainsKey(p);
        }

        public static CordaAmarrada GetCordaAmarrada(Mobile m)
        {
            if (Preso(m)) return _presos[m];
            return null;
        }

        private void TrySoltaMobile(Mobile mobile)
        {
            if (Preso(mobile))
            {
                if (mobile.Alive)
                {
                    mobile.OverheadMessage("* se soltou *");
                    mobile.SendMessage("Voce se soltou");
                }
                mobile.Frozen = false;
            }
            if (_presos.ContainsKey(mobile))
            {
                var i = _presos[mobile];
                if (i != null)
                    i.Delete();
                _presos.Remove(mobile);
                var corda = new Rope();
                corda.MoveToWorld(mobile.Location, mobile.Map);
            }
        }

        private void TrySoltaCorpo(Corpse corpo)
        {
            var dono = corpo.Owner;
            var i = _presos[dono];
            if (i != null)
                i.Delete();
            _presos.Remove(dono);
            var corda = new Rope();
            corda.MoveToWorld(corpo.Location, corpo.Map);
            corda.Visible = false;
        }

        public void TrySolta(object o)
        {
            var mobile = o as Mobile;

            if(mobile != null)
                TrySoltaMobile(mobile);
            else if(o is Corpse)
                TrySoltaCorpo(o as Corpse);
        }

        private void PrendeMobile(Mobile p)
        {
            if (p.Alive)
            {
                if (Preso(p))
                {
                    p.OverheadMessage("* apertado *");
                    p.SendMessage("Sua corda foi apertada.");
                }
                else
                {
                    p.OverheadMessage("* amarrado *");
                    p.SendMessage("Voce foi amarrado.");
                }
            }
            var tempo = TimeSpan.FromMinutes(26 - p.Str / 4);
            p.Freeze(tempo);
            if (timerPrisao != null && timerPrisao.Running)
                timerPrisao.Stop();

            timerPrisao = Timer.DelayCall(tempo - TimeSpan.FromMinutes(0.5), () =>
            {
                TrySolta(p);
            });
        }

        public void Prende(object o)
        {
            Mobile p = o as Mobile;
            if (p != null && p.Mounted)
            {
                return;
            }

            if (p != null && p is PlayerMobile)
            {
                PrendeMobile(p);
            }

            var cordaAmarrada = new CordaAmarrada(o, this);
            if (p != null)
            {
                cordaAmarrada.MoveToWorld(new Point3D(p.Location.X, p.Location.Y, p.Location.Z + 7), p.Map);
                _presos.Add(p, cordaAmarrada);
            }
            else if (o is Corpse)
            {
                var corpo = (Corpse)o;
                var dono = corpo.Owner;
                var loc = corpo.Location;
                cordaAmarrada.ItemID = 0;
                cordaAmarrada.MoveToWorld(new Point3D(loc.X, loc.Y, loc.Z + 2), corpo.Map);
                _presos.Add(dono, cordaAmarrada);
                corpo.PublicOverheadMessage("* amarrado *");
            } 
            this.Delete();
        }

        // Alvo de qnd usa a corda
        private class AlvoDaCorda : Target
        {
            private Rope corda;

            public AlvoDaCorda(Rope corda) : base(2, false, TargetFlags.None)
            {
                this.corda = corda;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                var pl = targeted as PlayerMobile;
                if (pl != null)
                {
                    if (!pl.Alive)
                        return;

                    if (pl.Mounted)
                        return;

                    corda.QuemPrendeu = from as PlayerMobile;
                    from.SendMessage("Tentando gentilmente amarrar " + pl.Name);
                    from.OverheadMessage("* estende uma corda *");
                    pl.SendGump(new ConfirmaPreso(corda));
                    return;
                }
            }
        }

        public class ConfirmaPreso : BaseConfirmGump
        {
            public PlayerMobile prendedor;
            private Rope corda;

            public override string LabelString { get { return "Deseja se render e ser amarrado ?"; } }
            public override string TitleString
            {
                get
                {
                    return "Rendicao";
                }
            }

            public ConfirmaPreso(Rope c)
            {
                this.corda = c;
                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;
            }

            public override void Confirm(Mobile from)
            {
                if (from.GetDistance(corda.QuemPrendeu) > 2)
                {
                    from.SendMessage("Muito longe");
                    corda.QuemPrendeu.SendMessage("Muito longe");
                    return;
                }
                if (from.Mounted)
                {
                    corda.QuemPrendeu.SendMessage("Voce nao consegue amarrar alguem montado");
                    return;
                }
                corda.Prende((PlayerMobile)from);
            }

            public override void Refuse(Mobile from)
            {
                from.OverheadMessage("* nega *");
                from.SendMessage("Voce recusou a ser amarrado(a)");
                corda.QuemPrendeu.SendMessage(from.Name + " se recusou a ser amarrado");
            }
        }
    }



}
