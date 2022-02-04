using System;
using Server.Items;

namespace Server.Mobiles
{
    public class ThiefGuildmaster : BaseGuildmaster
    {
        [Constructable]
        public ThiefGuildmaster()
            : base("o mestre da guilda dos ladinos")
        {
            this.SetSkill(SkillName.DetectHidden, 75.0, 98.0);
            this.SetSkill(SkillName.Hiding, 65.0, 88.0);
            this.SetSkill(SkillName.Lockpicking, 85.0, 100.0);
            this.SetSkill(SkillName.Snooping, 90.0, 100.0);
            this.SetSkill(SkillName.Poisoning, 60.0, 83.0);
            this.SetSkill(SkillName.Stealing, 90.0, 100.0);
            this.SetSkill(SkillName.Fencing, 75.0, 98.0);
            this.SetSkill(SkillName.Stealth, 85.0, 100.0);
            this.SetSkill(SkillName.RemoveTrap, 85.0, 100.0);
        }

        public ThiefGuildmaster(Serial serial)
            : base(serial)
        {
        }

        public override NpcGuild NpcGuild
        {
            get
            {
                return NpcGuild.ThievesGuild;
            }
        }
        public override TimeSpan JoinAge
        {
            get
            {
                return Siege.SiegeShard ? TimeSpan.FromDays(0.0) : TimeSpan.FromDays(14.0);
            }
        }
        public override TimeSpan JoinGameAge
        {
            get
            {
                return Siege.SiegeShard ? TimeSpan.FromDays(0.0) : TimeSpan.FromDays(5.0);
            }
        }
        public override void InitOutfit()
        {
            base.InitOutfit();

            if (Utility.RandomBool())
                this.AddItem(new Server.Items.Kryss());
            else
                this.AddItem(new Server.Items.Dagger());
        }

        public override bool CheckCustomReqs(PlayerMobile pm)
        {
            if (pm.Young && !Siege.SiegeShard)
            {
                this.SayTo(pm, "Voce nao pode se tornar membro se ainda for jovem"); // You cannot be a member of the Thieves' Guild while you are Young.
                return false;
            }
            else if (pm.Kills > 0)
            {
                this.SayTo(pm, "Somos uma guilda de ladinos nao de assassinos"); // This guild is for cunning thieves, not oafish cutthroats.
                return false;
            }
            else if (pm.Skills[SkillName.Stealing].Base < 60.0 && !Siege.SiegeShard)
            {
                this.SayTo(pm, "Voce nao me parece um ladino bom o suficiente, treine suas skills de furto."); // You must be at least a journeyman pickpocket to join this elite organization.
                return false;
            }

            return true;
        }

        public override void SayWelcomeTo(Mobile m)
        {
            this.SayTo(m, "Bem vindo a guilda dos ladinos"); // Welcome to the guild! Stay to the shadows, friend.
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from.InRange(this.Location, 2))
                return true;

            return base.HandlesOnSpeech(from);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!e.Handled && from is PlayerMobile && from.InRange(this.Location, 2) && (e.HasKeyword(0x1F) || e.Speech.Contains("disfarce"))) // *disguise*
            {
                PlayerMobile pm = (PlayerMobile)from;

                if (pm.NpcGuild == NpcGuild.ThievesGuild)
                    this.SayTo(from, "700 ouros por favor"); // That particular item costs 700 gold pieces.
                else
                    this.SayTo(from, "Nao sei do que voce esta falando."); // I don't know what you're talking about.

                e.Handled = true;
            }

            base.OnSpeech(e);
        }

        public override bool OnGoldGiven(Mobile from, Gold dropped)
        {
            if (from is PlayerMobile && dropped.Amount == 700)
            {
                PlayerMobile pm = (PlayerMobile)from;

                if (pm.NpcGuild == NpcGuild.ThievesGuild)
                {
                    from.AddToBackpack(new DisguiseKit());

                    dropped.Delete();
                    return true;
                }
            }

            return base.OnGoldGiven(from, dropped);
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
