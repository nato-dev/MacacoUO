using System;
using Server.Engines.Points;
using Server.Items;
using Server.Mobiles;
using Server.Ziden.Dungeons.Goblins.Quest;
using Server.Ziden.Items;

namespace Server.Engines.Quests
{
    public class MatarDeceit : BaseQuest
    {
     
        public override bool DoneOnce { get { return true; } }

        public override object Title
        {
            get
            {
                return "Oblitere Mortos Vivos";
            }
        }
        public override object Description
        {
            get
            {
                return "Mortos vivos estao por toda parte por essas terras. Por favor viajante, oblitere os mortos vivos !";
            }
        }
        public override object Refuse
        {
            get
            {
                return "Ah uma pena";
            }
        }
        public override object Uncomplete
        {
            get
            {
                return "Vejo que retornou, mas ainda acho que tem mais monstros a serem mortos !";
            }
        }
        public override object Complete
        {
            get
            {
                return "Ah muito obrigado aventureiro, agora poderei sossegar";
            }
        }

        public MatarDeceit()
            : base()
        {
            this.AddObjective(new SlayObjective(typeof(Ghoul), "Zumbi Assombrado", 20));
            this.AddObjective(new SlayObjective(typeof(Shade), "Sombra", 20));
            this.AddObjective(new SlayObjective(typeof(Skeleton), "Esqueleto", 50));

            this.AddReward(new BaseReward(typeof(Amber), 3, "3 Ambares"));
            this.AddReward(new BaseReward(typeof(CaixaDeGold), 1, "1 Caixa"));
            this.AddReward(new BaseReward(typeof(CombatSkillBook), 2, "2 Livros de Combate"));
            this.AddReward(new BaseReward(typeof(LivroAntigo), 1, "1 Livro Antigo"));
        }

        public override void OnCompleted()
        {
            PointsSystem.Exp.AwardPoints(this.Owner, 300);
            this.Owner.PlaySound(this.CompleteSound);
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
 
    public class QuesterDeceit : MondainQuester
    {
        public override Type[] Quests
        {
            get
            {
                return new Type[] {
                    typeof(MatarDeceit)
        };
            }
        }


        [Constructable]
        public QuesterDeceit()
            : base("Helton", "")
        {
            this.SetSkill(SkillName.Anatomy, 120.0, 120.0);
            this.SetSkill(SkillName.Parry, 120.0, 120.0);
            this.SetSkill(SkillName.Healing, 120.0, 120.0);
            this.SetSkill(SkillName.Tactics, 120.0, 120.0);
            this.SetSkill(SkillName.Swords, 120.0, 120.0);
            this.SetSkill(SkillName.Focus, 120.0, 120.0);
        }

        public QuesterDeceit(Serial serial)
            : base(serial)
        {
        }

        public override void Advertise()
        {
            this.Say("Por favor, me ajudem !");  // Know yourself, and you will become a true warrior.
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
            switch (Utility.Random(3))
            {
                case 0:
                    SetWearable(new FancyShirt(GetRandomHue()));
                    break;
                case 1:
                    SetWearable(new Doublet(GetRandomHue()));
                    break;
                case 2:
                    SetWearable(new Shirt(GetRandomHue()));
                    break;
            }

            SetWearable(new Shoes(GetShoeHue()));
            int hairHue = GetHairHue();

            Utility.AssignRandomHair(this, hairHue);
            Utility.AssignRandomFacialHair(this, hairHue);

            if (Body == 0x191)
            {
                FacialHairItemID = 0;
            }

            if (Body == 0x191)
            {
                switch (Utility.Random(6))
                {
                    case 0:
                        SetWearable(new ShortPants(GetRandomHue()));
                        break;
                    case 1:
                    case 2:
                        SetWearable(new Kilt(GetRandomHue()));
                        break;
                    case 3:
                    case 4:
                    case 5:
                        SetWearable(new Skirt(GetRandomHue()));
                        break;
                }
            }
            else
            {
                switch (Utility.Random(2))
                {
                    case 0:
                        SetWearable(new LongPants(GetRandomHue()));
                        break;
                    case 1:
                        SetWearable(new ShortPants(GetRandomHue()));
                        break;
                }
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
