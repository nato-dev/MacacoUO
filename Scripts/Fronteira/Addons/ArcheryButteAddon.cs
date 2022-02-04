using System;
using System.Collections;
using Server.Network;
using Server.Gumps;

namespace Server.Items
{
    [FlipableAttribute(0x100A/*East*/, 0x100B/*South*/)]
    public class ArcheryButte : AddonComponent
    {
        private static readonly TimeSpan UseDelay = TimeSpan.FromSeconds(2.0);

        private double m_MinSkill;
        private double m_MaxSkill;
        private int m_Arrows, m_Bolts;
        private DateTime m_LastUse;
        private Hashtable m_Entries;

        [Constructable]
        public ArcheryButte()
            : this(0x100A)
        {
            Name = "Tiro ao Alvo";
        }

        public ArcheryButte(int itemID)
            : base(itemID)
        {
            m_MinSkill = -25.0;
            m_MaxSkill = +90;
        }

        public ArcheryButte(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public double MinSkill
        {
            get
            {
                return m_MinSkill;
            }
            set
            {
                m_MinSkill = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public double MaxSkill
        {
            get
            {
                return m_MaxSkill;
            }
            set
            {
                m_MaxSkill = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime LastUse
        {
            get
            {
                return m_LastUse;
            }
            set
            {
                m_LastUse = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool FacingEast
        {
            get
            {
                return (ItemID == 0x100A);
            }
            set
            {
                ItemID = value ? 0x100A : 0x100B;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Arrows
        {
            get
            {
                return m_Arrows;
            }
            set
            {
                m_Arrows = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Bolts
        {
            get
            {
                return m_Bolts;
            }
            set
            {
                m_Bolts = value;
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if ((from.Weapon is Boomerang || from.Weapon is Cyclone || from.Weapon is BaseThrown) && from.InRange(GetWorldLocation(), 1))
                Fire(from);
            if ((m_Arrows > 0 || m_Bolts > 0) && from.InRange(GetWorldLocation(), 1))
                Gather(from);
            else
                Fire(from);
        }

        public void Gather(Mobile from)
        {
            from.LocalOverheadMessage(MessageType.Regular, 0x7B2, true, "* pegou flechas *"); // You gather the arrows and bolts.

            if (m_Arrows > 0)
                from.AddToBackpack(new Arrow(m_Arrows));

            if (m_Bolts > 0)
                from.AddToBackpack(new Bolt(m_Bolts));

            m_Arrows = 0;
            m_Bolts = 0;

            m_Entries = null;
        }

        public void Fire(Mobile from)
        {
            BaseRanged ranged = from.Weapon as BaseRanged;

            if (ranged == null)
            {
                from.SendMessage("Voce precisa praticar com arcos ou bestas nisso"); // You must practice with ranged weapons on 
                return;
            }

            if (DateTime.UtcNow < (m_LastUse + UseDelay))
                return;

            Point3D worldLoc = GetWorldLocation();

            if (FacingEast ? from.X <= worldLoc.X : from.Y <= worldLoc.Y)
            {
                from.SendMessage("Seria melhor se voce ficasse de frente para o alvo"); // You would do better to stand in front of the archery butte.
                return;
            }

            if (FacingEast ? from.Y != worldLoc.Y : from.X != worldLoc.X)
            {
                from.SendMessage("Voce nao esta alinhado com o alvo"); // You aren't properly lined up with the archery butte to get an accurate shot.
                return;
            }

            if (!from.InRange(worldLoc, 6))
            {
                from.SendMessage("Voce esta muito longe"); // You are too far away from the archery butte to get an accurate shot.
                return;
            }
            
            if (from.InRange(worldLoc, 4))
            {
                from.SendMessage("Voce esta muito proximo"); // You are too close to the target.
                return;
            }

            Type ammoType = ranged.AmmoType;

            bool isArrow = (ammoType == typeof(Arrow));
            bool isBolt = (ammoType == typeof(Bolt));

            BaseThrown thrown = ranged as BaseThrown;

            if (ammoType == null && thrown == null)
            {
                isArrow = ranged.Animation == WeaponAnimation.ShootBow;
                isBolt = ranged.Animation == WeaponAnimation.ShootXBow;
            }

            bool isKnown = (isArrow || isBolt);

            if (thrown == null)
            {
                Container pack = from.Backpack;

                if (pack == null || ammoType == null || !pack.ConsumeTotal(ammoType, 1))
                {
                    if (isArrow)
                        from.SendMessage("Voce nao tem flechas"); // You do not have any arrows with which to practice.
                    else if (isBolt)
                        from.SendMessage("Voce nao tem dardos"); // You do not have any crossbow bolts with which to practice.
                    else
                        from.SendMessage("Voce precisa ter uma arma de ataque a distancia para praticar"); // You must practice with ranged weapons on 

                    return;
                }
            }

            m_LastUse = DateTime.UtcNow;
            
            from.MovingEffect(this, ranged.EffectID, 18, 1, false, false);
            from.Direction = from.GetDirectionTo(GetWorldLocation());
            ranged.PlaySwingAnimation(from);

            ScoreEntry se = GetEntryFor(from);

            if (!from.CheckSkillMult(ranged.Skill, m_MinSkill, m_MaxSkill))
            {
                from.PlaySound(ranged.MissSound);

                from.SendMessage("Voce errou o alvo"); // You miss the target altogether.

                se.Record(0);

                PublicOverheadMessage(MessageType.Regular, 0x7B2, false, String.Format("Pontos: {0} Tiros:\t{1}", se.Total, se.Count));

                return;
            }

            Effects.PlaySound(Location, Map, 0x2B1);

            double rand = Utility.RandomDouble();

            int area, score, splitScore;

            string msg = null;
            if (0.10 > rand)
            {
                msg = "bullseye";
                area = 0; // bullseye
                score = 50;
                splitScore = 100;
            }
            else if (0.25 > rand)
            {
                msg = "arco interno";
                area = 1; // inner ring
                score = 10;
                splitScore = 20;
            }
            else if (0.50 > rand)
            {
                msg = "arco do meio";
                area = 2; // middle ring
                score = 5;
                splitScore = 15;
            }
            else
            {
                msg = "arco de fora";
                area = 3; // outer ring
                score = 2;
                splitScore = 5;
            }

            bool split = (isKnown && ((m_Arrows + m_Bolts) * 0.02) > Utility.RandomDouble());

            if (split)
            {
                PublicOverheadMessage(MessageType.Regular, 0x7B2, true, "* "+msg+" *");
                PublicOverheadMessage(MessageType.Regular, 0x7B2, true, "* partiu uma flecha no meio *");
            }
            else
            {
                PublicOverheadMessage(MessageType.Regular, 0x7B2, false, "* " + msg + " *");

                if (ammoType != null)
                {
                    if (isArrow)
                        ++m_Arrows;
                    else if (isBolt)
                        ++m_Bolts;
                }
            }

            se.Record(split ? splitScore : score);
            PublicOverheadMessage(MessageType.Regular, 0x7B2, false, String.Format("Pontos: {0} Tiros: \t{1}", se.Total, se.Count));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);

            writer.Write(m_MinSkill);
            writer.Write(m_MaxSkill);
            writer.Write(m_Arrows);
            writer.Write(m_Bolts);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_MinSkill = reader.ReadDouble();
                        m_MaxSkill = reader.ReadDouble();
                        m_Arrows = reader.ReadInt();
                        m_Bolts = reader.ReadInt();

                        if (m_MinSkill == 0.0 && m_MaxSkill == 30.0)
                        {
                            m_MinSkill = -25.0;
                            m_MaxSkill = +25.0;
                        }

                        break;
                    }
            }
        }

        private ScoreEntry GetEntryFor(Mobile from)
        {
            if (m_Entries == null)
                m_Entries = new Hashtable();

            ScoreEntry e = (ScoreEntry)m_Entries[from];

            if (e == null)
                m_Entries[from] = e = new ScoreEntry();

            return e;
        }

        private class ScoreEntry
        {
            private int m_Total;
            private int m_Count;

            public int Total
            {
                get
                {
                    return m_Total;
                }
                set
                {
                    m_Total = value;
                }
            }

            public int Count
            {
                get
                {
                    return m_Count;
                }
                set
                {
                    m_Count = value;
                }
            }

            public void Record(int score)
            {
                m_Total += score;
                m_Count += 1;
            }
        }
    }

    public class ArcheryButteAddon : BaseAddon
    {
        [Constructable]
        public ArcheryButteAddon(AddonFacing facing)
        {
            switch (facing)
            {
                case AddonFacing.East:
                    AddComponent(new ArcheryButte(0x100A), 0, 0, 0);
                    break;
                case AddonFacing.South:
                    AddComponent(new ArcheryButte(0x100B), 0, 0, 0);
                    break;
            }
        }

        public ArcheryButteAddon(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddonDeed Deed
        {
            get
            {
                return new ArcheryButteDeed();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt();
        }
    }

    public class ArcheryButteDeed : BaseAddonDeed, IRewardOption
    {
        private AddonFacing Facing { get; set; }

        [Constructable]
        public ArcheryButteDeed()
        {
            Name = "Escritura para tiro ao alvo";
        }

        public ArcheryButteDeed(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddon Addon
        {
            get
            {
                return new ArcheryButteAddon(Facing);
            }
        }

        public override int LabelNumber
        {
            get
            {
                return 1080205;
            }
        }// archery butte

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.CloseGump(typeof(RewardOptionGump));
                from.SendGump(new RewardOptionGump(this));
            }
            else
                from.SendLocalizedMessage(1062334); // This item must be in your backpack to be used.       	
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt();
        }

        public void GetOptions(RewardOptionList list)
        {
            list.Add((int)AddonFacing.South, 1080204);
            list.Add((int)AddonFacing.East, 1080203);
        }

        public void OnOptionSelected(Mobile from, int choice)
        {
            Facing = (AddonFacing)choice;

            if (!Deleted)
                base.OnDoubleClick(from);
        }
    }
}
