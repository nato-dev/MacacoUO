using System;
using Server.Engines.Points;
using Server.Items;
using Server.Mobiles;
using Server.Ziden.Dungeons.Esgoto;

namespace Server.Engines.Quests
{
    public class SapatoLindoQ : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title
        {
            get
            {
                return "Meus Sapatinhos Lindos";
            }
        }
        public override object Description
        {
            get
            {
                return "Ah modeuzo mosso eu dixi cai meu sapato lindju no rio ! Acho qui foi pru bueru! <br> Ondi fica o bueru ce mi pregunta ? <b>Logo no buraco a nortali</b>, ! Si mi devorve os sapatos eu ti do um trem baum e ti conto um segredo !";
            }
        }
        public override object Refuse
        {
            get
            {
                return "Oxi mosso";
            }
        }
        public override object Uncomplete
        {
            get
            {
                return "Uai quede meu sapato ?!";
            }
        }

        public override object Complete
        {
            get
            {
                return @"Ah bom de mais da conta ! Ceh muito bao ! <br> Vou te contar um segredo em troca ! <br><br>
<b><u>No esgoto, na sala do pentagrama, tem um segredo atraz da teia de aranha</u></b>.
<br><br>Ja contei uqui sabia, ce eh baum dimais da conta brigado pelo meu sapato.";
            }
        }

        public SapatoLindoQ()
            : base()
        {
            //this.AddObjective(new ObtainObjective(typeof(LuckyDagger), "lucky dagger", 1));
            this.AddObjective(new ObtainObjective(typeof(SapatoLindo), "Sapato do Zeh", 1));
            this.AddReward(new BaseReward(typeof(Gold), 800, "800 Moedas e 1 Segredo"));
        }

        public override void OnCompleted()
        {
            PointsSystem.Exp.AwardPoints(this.Owner, 300);
            this.Owner.PlaySound(this.CompleteSound);
            this.Owner.SendMessage(78, "O segredo parece estar no esgoto na sala do pentagrama, perto da teia de aranha...");
            if (this.Owner.Wisp != null)
            {
                this.Owner.Wisp.EntregaSapato();
            }
        }

        public override void OnAccept()
        {
            base.OnAccept();
            if(this.Owner.Wisp != null)
            {
                this.Owner.Wisp.QuestNoob();
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
   
    public class ZehRoela : MondainQuester
    {
        public override Type[] Quests
        {
            get
            {
                return new Type[] {
                    typeof(SapatoLindoQ),
                    //typeof(SapatoLindoQ2)
                };
            }
        }


        [Constructable]
        public ZehRoela()
            : base("Zeh Roela", "")
        {
            this.SetSkill(SkillName.Anatomy, 120.0, 120.0);
            this.SetSkill(SkillName.Parry, 120.0, 120.0);
            this.SetSkill(SkillName.Healing, 120.0, 120.0);
            this.SetSkill(SkillName.Tactics, 120.0, 120.0);
            this.SetSkill(SkillName.Swords, 120.0, 120.0);
            this.SetSkill(SkillName.Focus, 120.0, 120.0);
        }

        public ZehRoela(Serial serial)
            : base(serial)
        {
        }

        public override void Advertise()
        {
            this.Say("Ah mo deuso, dichi meu sapato cai no rio ! Aaahhhhh");  // Know yourself, and you will become a true warrior.
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
