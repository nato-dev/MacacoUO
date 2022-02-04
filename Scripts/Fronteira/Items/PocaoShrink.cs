using Leilaum.Interfaces;
using Server.Misc.Custom;
using Server.Mobiles;
using Server.Targeting;
using Shrink.ShrinkSystem;

namespace Server.Ziden
{
    public class PocaoShrink : Item, IShrinkTool
    {
        [Constructable]
        public PocaoShrink() : base(0x0EFC)
        {
            this.Name = "Pocao do Encolhimento";
            this.Stackable = true;
            ShrinkCharges = -1;
        }

        public PocaoShrink(Serial s) : base(s) { }

        public int ShrinkCharges { get; set; }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Selecione um animal para encolher");
            from.Target = new ShrinkTarget(from, this, false);
        }

        private class IT : Target
        {
            public IT() : base(3, false, TargetFlags.None) { }

            protected override void OnTarget(Mobile from, object targeted)
            {
                var bc = targeted as BaseCreature;
                if(bc == null)
                {
                    from.SendMessage("Voce precisa escolher uma criatura");
                    return;
                }
                   
            }
        }
    }
}
