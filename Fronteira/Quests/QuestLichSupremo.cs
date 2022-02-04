using System;
using Server.Engines.Points;
using Server.Items;
using Server.Mobiles;
using Server.Ziden;

namespace Server.Engines.Quests
{
    public class MatarLichzaum : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        //public override QuestChain ChainID => QuestChain.Tutorial;

        //public override Type NextQuest =>  typeof(CloseEnoughQuest)

        public override object Title
        {
            get
            {
                return "O Necromante";
            }
        }
        public override object Description
        {
            get
            {
                return @"Ola viajante. Existe uma lenda, que todos mortos vivos destas terras sao criados por uma entidade sombria - o Lich Rei. Dizem que este vive em uma caverna pelo mundo... mas nunca o encontrei.
Se voce o encontrar, conseguiria mata-lo e trazer sua cabeca para mim ? Gostaria muito de estuda-lo. Provavelmente este ser nao eh facilmente encnotrado - voce tera de matar muitos mortos vivos em algum local especifico para chamar sua atencao.
Se voce conseguir, quem sabe<b> voce nao consegue um livro de necromancia </b> por la ?";
            }
        }
        public override object Refuse
        {
            get
            {
                return "Eh eu tambem teria medo do Lich Rei...";
            }
        }
        public override object Uncomplete
        {
            get
            {
                return "Cade minha cabeca do Lich Rei ?";
            }
        }
        public override object Complete
        {
            get
            {
                return "Ah muito obrigado aventureiro, irei estuda-la. Aqui, sua recompensa.";
            }
        }

        public MatarLichzaum()
            : base()
        {
            this.AddObjective(new SlayObjective(typeof(AncientLichRenowned), "Lich Rei", 1));
            this.AddReward(new BaseReward(typeof(Gold), 1000, "1000 Moedas de Ouro"));
            this.AddReward(new BaseReward(typeof(LichKiller), 1, "Titulo de Matador de Liches"));
            this.AddReward(new BaseReward(typeof(BagOfNecromancerReagents), 25, "Sacola de Reags Necro"));
        }

        public override void OnCompleted()
        {
            this.Owner.PlaySound(this.CompleteSound);
            PointsSystem.Exp.AwardPoints(this.Owner, 1000);
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

    public class EstudanteLichRei : MondainQuester
    {
        public override Type[] Quests
        {
            get
            {
                return new Type[] {
                    typeof(MatarLichzaum)
        };
            }
        }


        [Constructable]
        public EstudanteLichRei()
            : base("Wololo", "O Mago")
        {
            this.SetSkill(SkillName.Anatomy, 120.0, 120.0);
            this.SetSkill(SkillName.Parry, 120.0, 120.0);
            this.SetSkill(SkillName.Healing, 120.0, 120.0);
            this.SetSkill(SkillName.Tactics, 120.0, 120.0);
            this.SetSkill(SkillName.Swords, 120.0, 120.0);
            this.SetSkill(SkillName.Focus, 120.0, 120.0);
        }

        public EstudanteLichRei(Serial serial)
            : base(serial)
        {
        }

        public override void Advertise()
        {
            this.Say("* estudando *");  // Know yourself, and you will become a true warrior.
        }

        public override void InitBody()
        {
            this.Female = false;
            this.CantWalk = true;
            this.Race = Race.Human;

            base.InitBody();
        }

        public override void InitOutfit()
        {
            SetWearable(new Robe(Utility.RandomBlueHue()));
            SetWearable(new WizardsHat(Utility.RandomBlueHue()));
            SetWearable(new Shoes(Utility.RandomBlueHue()));
            SetWearable(new Spellbook());
            int hairHue = GetHairHue();

            Utility.AssignRandomHair(this, hairHue);
            Utility.AssignRandomFacialHair(this, hairHue);

            if (Body == 0x191)
            {
                FacialHairItemID = 0;
            }

            if (!Siege.SiegeShard)
                PackGold(100, 200);
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
