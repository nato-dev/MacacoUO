using System;
using Server;
using Server.Items;
using Server.Factions;

namespace Server.Mobiles
{
    [CorpseName("a druid's corpse")]
    public class BearDruid : BaseCreature
    {
        [Constructable]
        public BearDruid()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "druida vil";
            Body = 0x190;
            Hue = TintaPreta.COR;

            SetStr(100, 200);
            SetDex(110, 115);
            SetInt(1000, 1200);

            SetHits(300, 300);
            SetMana(800, 1000);

            SetDamage(3, 18);

            VirtualArmor = 0;

            SetSkill(SkillName.Tactics, 100.0, 100.0);

            SetSkill(SkillName.EvalInt, 100, 120);
            SetSkill(SkillName.Meditation, 100.0, 100.0);
            SetSkill(SkillName.Magery, 100, 120);

            SetSkill(SkillName.MagicResist, 120, 120);

            SetSkill(SkillName.Wrestling, 70, 70);

            Fame = 500;
            Karma = -500;

            Item Sandals = new Sandals();
            Sandals.Movable = false;
            Sandals.Hue = TintaPreta.COR;
            AddItem(Sandals);

            Item BodySash = new Surcoat();
            BodySash.Movable = false;
            BodySash.Hue = TintaPreta.COR;
            AddItem(BodySash);

            Item LeatherChest = new LeatherChest();
            LeatherChest.Movable = false;
            LeatherChest.Hue = TintaPreta.COR;
            AddItem(LeatherChest);

            Item LeatherArms = new LeatherArms();
            LeatherArms.Movable = false;
            LeatherArms.Hue = TintaPreta.COR;
            AddItem(LeatherArms);

            Item LeatherLegs = new LeatherLegs();
            LeatherLegs.Movable = false;
            LeatherLegs.Hue = TintaPreta.COR;
            AddItem(LeatherLegs);

            Item LeatherGloves = new LeatherGloves();
            LeatherGloves.Movable = false;
            LeatherGloves.Hue = TintaPreta.COR;
            AddItem(LeatherGloves);

            Item LeatherGorget = new LeatherGorget();
            LeatherGorget.Movable = false;
            AddItem(LeatherGorget);

            Item Hood = new BearMask();
            Hood.Movable = false;
            Hood.Hue = 0;
            AddItem(Hood);
        }


        public override void GenerateLoot()
        {

        }

        public override bool CanPeace { get { return false; } }
        public override bool CanRummageCorpses { get { return true; } }

        public override int Meat { get { return 1; } }

        public BearDruid(Serial serial)
            : base(serial)
        {
        }

        public override bool Murderer { get { return true; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override bool OnBeforeDeath()
        {
            BearformDruid rm = new BearformDruid();
            rm.Team = this.Team;
            rm.Combatant = this.Combatant;
            Effects.PlaySound(this, Map, 0x208);
            Effects.SendLocationEffect(Location, Map, 0x3709, 30, 10, 542, 0);
            rm.MoveToWorld(Location, Map);
            rm.OverheadMessage("* Transformou *");
            Delete();
            return false;
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
