using Server.Targeting;

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
            list.Add("Uma bola feita de neve");
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.Target = new IT(this);
            from.SendMessage("Selecione um alvo");

            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("Precisa estar em sua mochila"); // The BolaDeNeve must be in your pack to use it.
            }
            else if (from.Target is IT)
            {
                from.OverheadMessage("* pegou bola de neve *");
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
                if (!(targeted is Mobile))
                {
                    from.SendMessage("Isto nao eh um Mobile");
                    return;
                }

                if (targeted is Mobile)
                {
                    Mobile to = (Mobile)targeted;

                    if (!this.BolaDeNeve.IsChildOf(from.Backpack))
                    {
                        from.OverheadMessage("* A Bola De Neve deve estar na sua mochila *"); // The bola must be in your pack to use it.
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

                        var target = targeted as Mobile;

                        BolaDeNeve.Consume();

                        from.Animate(AnimationType.Attack, 4);
                        from.PlaySound(0x13C);
                        from.OverheadMessage("* jogou *");
                        from.MovingEffect(target, 0x3729, 10, 0, false, false);

                    }
                }
            }
        }
    }
}
