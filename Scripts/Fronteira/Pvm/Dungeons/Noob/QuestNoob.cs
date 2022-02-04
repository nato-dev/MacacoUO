using System;
using Server.Items;
using Server.Mobiles;
using Server.Ziden.Items;
using Shrink.ShrinkSystem;

namespace Server.Engines.Quests
{
    public class QuestNoob : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        //public override QuestChain ChainID => QuestChain.Tutorial;

        //public override Type NextQuest =>  typeof(CloseEnoughQuest)

        public override object Title
        {
            get
            {
                return "Essa Fada";
            }
        }
        public override object Description
        {
            get
            {
                return @"Ah obrigada por me retornar meu velho cajado. Veja, este mago com uma <b>fada ma</b> roubaram ele.
<br>Aonde esta esta fada agora ? Siga este mapa magico que voce encontrara a caverna em que ela se esconde !";
            }
        }
        public override object Refuse
        {
            get
            {
                return "Ate a proxima";
            }
        }
        public override object Uncomplete
        {
            get
            {
                return @"Use o mapa magico que lhe dei para achar a caverna que a fada esta ! Procure pelos brazoes para encontrar a entrada !";
            }
        }
        public override object Complete
        {
            get
            {
                return "Voce pos fim aquela fada maligna ? Muito obrigada... Aqui esta sua recompensa.";
            }
        }

        public QuestNoob()
            : base()
        {
            this.AddObjective(new SlayObjective(typeof(FadaMa), "Fada Ma", 1));
            this.AddReward(new BaseReward(typeof(Gold), 1000, "1000 Moedas de Ouro"));
            this.AddReward(new BaseReward(typeof(LivroAntigo), 1, "1 Livro Antigo"));
            this.AddReward(new BaseReward(typeof(CombatSkillBook), 1, "1 Livro de Combate"));
        }

        public override void OnAccept()
        {
            base.OnAccept();
            this.Owner.SendMessage("Voce recebeu um mapa magico");
            this.Owner.Backpack.DropItem(new MapaMagico());
            if(this.Owner.Wisp != null)
            {
                this.Owner.Wisp.FalaJill();
            }
        }

        public override void OnCompleted()
        {
            Points.PointsSystem.Exp.AwardPoints(this.Owner, 600);
            this.Owner.PlaySound(this.CompleteSound);
            if (this.Owner.Wisp != null)
            {
                this.Owner.Wisp.Fala("Que tal ir explorar o portal azul perto do centro da cidade agora ?");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class VelhoNoob : MondainQuester
    {
        public override Type[] Quests
        {
            get
            {
                return new Type[] {
                    typeof(QuestNoob)
        };
            }
        }

        [Constructable]
        public VelhoNoob()
            : base("Jill", "a velha")
        {
            this.SetSkill(SkillName.Anatomy, 120.0, 120.0);
            this.SetSkill(SkillName.Parry, 120.0, 120.0);
            this.SetSkill(SkillName.Healing, 120.0, 120.0);
            this.SetSkill(SkillName.Tactics, 120.0, 120.0);
            this.SetSkill(SkillName.Swords, 120.0, 120.0);
            this.SetSkill(SkillName.Focus, 120.0, 120.0);
        }

        public VelhoNoob(Serial serial)
            : base(serial)
        {
        }

        public override void Advertise()
        {
            this.Say("Voce, aventureiro, quer dinheiro e conhecimento ?");  // Know yourself, and you will become a true warrior.
        }

        public override void InitBody()
        {
            this.Female = true;
            this.CantWalk = true;
            this.Race = Race.Human;
            this.HairItemID = 0x203D;
            this.HairHue = 0;
            base.InitBody();
        }

        public override void InitOutfit()
        {
            this.AddItem(new Backpack());
            this.AddItem(new Robe(788));
            this.AddItem(new ThighBoots());

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
