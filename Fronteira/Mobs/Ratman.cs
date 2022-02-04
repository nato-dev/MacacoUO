using System;
using System.Collections.Generic;
using Server.Items;
using Server.Misc;
using Shrink.ShrinkSystem;

namespace Server.Mobiles
{
    [CorpseName("a ratman's corpse")]
    public class BaseRatman : BaseCreature
    {
        [Constructable]
        public BaseRatman(AIType type, FightMode mode, int n1, int n2, double n3, double n4)
            : base(type, mode, n1, n2, n3, n4)
        {

        }

        public BaseRatman(Serial serial)
            : base(serial)
        {
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            Orc.TentaAtacarMaster(this, from);
            base.OnDamage(amount, from, willKill);
        }

        public override void OnStartCombat(Mobile m)
        {
            if (this.Map == Map.Internal)
                return;

            if (m == null)
                return;

            if (this.Map == null)
                return;

            var eable = this.Map.GetMobilesInRange(this.Location, 25);
            try
            {
                var msg = false;
                this.PlayAngerSound();

                if (!IsCooldown("ajuda"))
                {
                    foreach (var mob in eable)
                    {
                        if (mob == this || mob == null)
                            continue;

                        if (mob is BaseRatman && mob.Combatant != m)
                        {
                            msg = true;
                            mob.PublicOverheadMessage(Network.MessageType.Emote, 0, false, "* ! *");
                            ((BaseRatman)mob).SetCooldown("ajuda", TimeSpan.FromSeconds(30));
                            mob.Combatant = m;
                        }
                    }
                    SetCooldown("ajuda", TimeSpan.FromSeconds(30));
                    if (msg)
                        this.OverheadMessage("* Aponta para " + m.Name + " *");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro Ratman");
            } finally
            {
                eable.Free();
            }
        }

        public override InhumanSpeech SpeechType
        {
            get
            {
                return InhumanSpeech.Ratman;
            }
        }
        public override bool CanRummageCorpses
        {
            get
            {
                return true;
            }
        }
        public override int Hides
        {
            get
            {
                return 8;
            }
        }
        public override HideType HideType
        {
            get
            {
                return HideType.Spined;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Meager);
            AddPackedLoot(LootPack.MeagerProvisions, typeof(Bag));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [CorpseName("a ratman's corpse")]
    public class Ratman : BaseRatman
    {
        [Constructable]
        public Ratman()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = NameList.RandomName("ratman") + " o ratoso";
            this.Body = 42;
            this.BaseSoundID = 437;

            this.SetStr(96, 120);
            this.SetDex(81, 100);
            this.SetInt(36, 60);

            this.SetHits(58, 72);

            this.SetDamage(4, 10);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 25, 30);
            this.SetResistance(ResistanceType.Fire, 10, 20);
            this.SetResistance(ResistanceType.Cold, 10, 20);
            this.SetResistance(ResistanceType.Poison, 10, 20);
            this.SetResistance(ResistanceType.Energy, 10, 20);

            this.SetSkill(SkillName.MagicResist, 35.1, 60.0);
            this.SetSkill(SkillName.Tactics, 50.1, 75.0);
            this.SetSkill(SkillName.Wrestling, 50.1, 75.0);

            this.Fame = 1500;
            this.Karma = -1500;

            this.VirtualArmor = 28;

            if(Utility.RandomDouble() > 0.1)
            {
                var leash = new PetLeash();
                leash.ShrinkCharges = 1;
                AddItem(leash);
            }
        }

        public Ratman(Serial serial)
            : base(serial)
        {
        }

        public override InhumanSpeech SpeechType
        {
            get
            {
                return InhumanSpeech.Ratman;
            }
        }
        public override bool CanRummageCorpses
        {
            get
            {
                return true;
            }
        }
        public override int Hides
        {
            get
            {
                return 8;
            }
        }
        public override HideType HideType
        {
            get
            {
                return HideType.Spined;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Meager);
            AddPackedLoot(LootPack.MeagerProvisions, typeof(Bag));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
