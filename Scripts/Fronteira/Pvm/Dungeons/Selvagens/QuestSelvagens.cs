using System;
using Server.Engines.Points;
using Server.Items;

namespace Server.Engines.Quests
{
    public class QuestSelvagens : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title
        {
            get
            {
                return "Os Selvagens Protetores";
            }
        }
        public override object Description
        {
            get
            {
                return @"Ei, voce, sabe aquele pequeno pantano a norte da cidade de Rhodes ?
 Nao, nao onde aqueles ratos sujos vivem, mais a norte seguindo a estrada, existe um pequeno
 pantano populado por selvagens. Malditos estes tem um lider, Barracoon, um shaman elementalista que
 converteu todas essas pessoas a virerem nas sombras. Esses selvagens construiram um templo no pantano.
 <br> Eu ia perto do pantano coletar favos de mel para fabricar botas elficas, mas os malditos roubaram minha foicinha.
 Sim eu sei que eu posso comprar outra, mas essa foi um presente, tinha meu nome nela.
 <br><br>
Sera que voce consegue recuperar minha foicinha ? <br>Voce vai precisar de <b>botas elficas</b> para andar no pantano. Talvez se voce conhecer um bom cozinheiro, podera fazer <b> tintas tribais </b> e se disfarcar para nao ser atacado.";
               
            }
        }
        public override object Refuse
        {
            get
            {
                return "Uma pena...queria muito minha foicinha";
            }
        }
        public override object Uncomplete
        {
            get
            {
                return "Achou minha foicinha ? Esta no pantano, norte dessa cidade seguindo a estrada.";
            }
        }

        public override object Complete
        {
            get
            {
                return @"MINHA FOICINHA !! Muito obrigado !!";
            }
        }

        public QuestSelvagens()
            : base()
        {
            this.AddObjective(new ObtainObjective(typeof(FoicinhaAssinada), "Foicinha Assinada", 1));
            this.AddReward(new BaseReward(typeof(Gold), 1000, "1000 Moedas"));
            this.AddReward(new BaseReward(typeof(TribalSpear), 1, "Lanca Tribal"));
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

    public class ExSelvagem : MondainQuester
    {
        public override Type[] Quests
        {
            get
            {
                return new Type[] {
                    typeof(QuestSelvagens),
                    //typeof(SapatoLindoQ2)
                };
            }
        }


        [Constructable]
        public ExSelvagem()
            : base("Jonathan o ex selvagem", "")
        {
            this.SetSkill(SkillName.Anatomy, 120.0, 120.0);
            this.SetSkill(SkillName.Parry, 120.0, 120.0);
            this.SetSkill(SkillName.Healing, 120.0, 120.0);
            this.SetSkill(SkillName.Tactics, 120.0, 120.0);
            this.SetSkill(SkillName.Swords, 120.0, 120.0);
            this.SetSkill(SkillName.Focus, 120.0, 120.0);
        }

        public ExSelvagem(Serial serial)
            : base(serial)
        {
        }

        public override void Advertise()
        {
            this.Say("Minha foicinha...malditos selvagens...");  // Know yourself, and you will become a true warrior.
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
