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
                return "Uma Lanterna de Luz";
            }
        }
        public override object Description
        {
            get
            {
                return @"New Haven foi criada pela forca de dois magos, para proteger o povo de forcas do mal.  <br>

Resultado da criacao da cidade, uma <basefont color=#00FF00>lanterna magica</basefont> foi criada para guardar toda a luz dos feiticeiros. <br>

Um <basefont color=#FF5A5A>feiticeiro</basefont> roubou a lanterna, e a usou para atacar New Haven, criando um vulcao no sul da ilha.<br><br>
<basefont color=#00FF00>Ao norte de onde estamos tem uma catacumba</basefont> la voce poderia encontrar o discipulo do mago que roubou a lanterna. ";
            }
        }
        public override object Refuse
        {
            get
            {
                return "Entendo. Retorne quando estiver pronto.";
            }
        }
        public override object Uncomplete
        {
            get
            {
                return "Encontrou algo na catacumba acima ?";
            }
        }

        public override object Complete
        {
            get
            {
                return @"O QUE ? Voce achou a lanterna ??? <br>Vou te contar um segredo em troca dela! <br><br>
<b><u>No buraco, na sala do pentagrama, tem um segredo atraz da teia de aranha</u></b>.
<br><br>Espere, esta lanterna eh apenas uma lanterna comum !!! Que pombas foi isso ? Quer me fazer perder tempo ! Tome sua laterna comum e a coloque onde achar conveniente. Tenha um bom dia.";
            }
        }

        public SapatoLindoQ()
            : base()
        {
            this.AddObjective(new ObtainObjective(typeof(LanternaMagica), "Lanterna Magica", 1));
            this.AddReward(new BaseReward(typeof(Gold), 800, "800 Moedas e 1 Segredo"));
        }

        public override void OnCompleted()
        {
            PointsSystem.Exp.AwardPoints(this.Owner, 300);
            this.Owner.AddToBackpack(new LanternaMagica());
            this.Owner.PlaySound(this.CompleteSound);
            this.Owner.SendMessage(78, "O segredo parece estar no buraco na sala do pentagrama, perto da teia de aranha...");
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
            this.Say("Ei voce, venha aqui um segundo, vou lhe contar uma historia.");  // Know yourself, and you will become a true warrior.
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
