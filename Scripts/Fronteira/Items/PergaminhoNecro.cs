using Server.Targeting;
using System;

namespace Server.Items.Functional.Pergaminhos
{
    public class PergaminhoNecro : Item
    {

        [CommandProperty(AccessLevel.GameMaster)]
        public int Dias { get; set; }

        [Constructable]
        public PergaminhoNecro()
            : this(0x14F0)
        {
            this.Dias = 30;
            this.Hue = 356;
            this.Name = "Pergaminho do Necromante";
        }

        public PergaminhoNecro(int itemID)
           : base(itemID)
        {
            this.Dias = 30;
            this.Hue = 356;
            this.Name = "Pergaminho do Necromante";
        }

        public PergaminhoNecro(Serial serial)
            : base(serial)
        {
            this.Dias = 30;
            this.Hue = 356;
            this.Name = "Pergaminho do Necromante";
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Escolha um item para abencoar");
            from.Target = new InternalTarget(from, this);
        }

        public class InternalTarget : Target
        {
            private Mobile from;
            private PergaminhoNecro scroll;
            public InternalTarget(Mobile from, PergaminhoNecro scroll) : base(1, false, TargetFlags.None)
            {
                this.from = from;
                this.scroll = scroll;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is NecromancerSpellbook)
                {
                    var item = (Item)targeted;
                    if (item.LootType == LootType.Blessed)
                    {
                        item.PrivateMessage("Este item ja esta abencoado", from);
                    }
                    else
                    {
                        item.LootType = LootType.Blessed;
                        from.FixedEffect(0x37C4, 87, 2000, 4, 3);
                        from.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                        from.PlaySound(0x202);
                        item.PrivateMessage("* pertence pessoal *", from);
                        from.SendMessage("Voce embrulha o item no pergaminho, desfazendo o pergaminho e envolvendo o item em energia sombria");
                        scroll.Consume();
                    }
                }
                else
                {
                    from.SendMessage("Voce apenas pode usar isto em roupas ou runebooks");
                }
            }
        }

        public virtual void AddNameProperties(ObjectPropertyList list)
        {
            list.Add("Pergaminho Do Necromante");
            list.Add("Torna um Livro de Necromancia um Pertence Pessoal");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(Dias);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Dias = reader.ReadInt();
        }
    }
}
