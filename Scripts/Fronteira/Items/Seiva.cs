using Server.Items;
using Server.Misc.Custom;
using Server.Targeting;

namespace Server.Ziden
{
    public class Seiva : Item
    {
        [Constructable]
        public Seiva() : base(10316)
        {
            this.Name = "Seiva";
            this.Stackable = true;
        }

        public Seiva(Serial s) : base(s) { }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Cozinheiros podem colocar isto em tubos de tinta");
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.Target = new IT(this);
            from.SendMessage("Selecione um tubo de tinta para misturar a seiva");
        }

        private class IT : Target
        {
            private Item seiva;

            public IT(Item seiva) :base(10, false, TargetFlags.None)
            {
                this.seiva = seiva;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if(!(targeted is DyeTub))
                {
                    from.SendMessage("Isto nao eh um tubo de tintas");
                    return;
                }
                if (from.Skills[SkillName.Cooking].Value < 70)
                {
                    from.SendMessage("Voce precisa de pelo menos 70 cooking para isto");
                    return;
                }
                var dyetub = targeted as DyeTub;
                dyetub.Hue = seiva.Hue;
                dyetub.DyedHue = seiva.Hue;
                dyetub.charges = 2;
                from.PlaySound(0x242);
                from.OverheadMessage("* misturou seiva *");
                seiva.Consume();
                from.SendMessage("Voce misturou a seiva no tubo de tintas");
            }
        }
    }
}
