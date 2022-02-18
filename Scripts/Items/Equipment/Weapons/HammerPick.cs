using System;
using Server.Engines.Craft;
using Server.Engines.Harvest;

namespace Server.Items
{
    [Alterable(typeof(DefBlacksmithy), typeof(DiscMace))]
    [FlipableAttribute(0x143D, 0x143C)]
    public class HammerPick : BaseBashing, IHarvestTool, IUsesRemaining
    {
        [Constructable]
        public HammerPick()
            : base(0x143D)
        {
            this.Weight = 9.0;
            this.Layer = Layer.OneHanded;
            Name = "Marreta";
            this.UsesRemaining = 100;
            this.ShowUsesRemaining = true;
        }

        public HammerPick(Serial serial)
            : base(serial)
        {
        }

        public override WeaponAbility PrimaryAbility
        {
            get
            {
                return WeaponAbility.ArmorIgnore;
            }
        }
        public override WeaponAbility SecondaryAbility
        {
            get
            {
                return WeaponAbility.MortalStrike;
            }
        }
        public override int AosStrengthReq
        {
            get
            {
                return 45;
            }
        }
        public override int AosMinDamage
        {
            get
            {
                return 13;
            }
        }
        public override int AosMaxDamage
        {
            get
            {
                return 17;
            }
        }
        public override int AosSpeed
        {
            get
            {
                return 28;
            }
        }
        public override float MlSpeed
        {
            get
            {
                return 3.25f;
            }
        }
        public override int OldStrengthReq
        {
            get
            {
                return 35;
            }
        }
        public override int OldMinDamage
        {
            get
            {
                return 6;
            }
        }
        public override int OldMaxDamage
        {
            get
            {
                return 33;
            }
        }
        public override int OldSpeed
        {
            get
            {
                return 30;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 31;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 70;
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (HarvestSystem == null || Deleted)
                return;

            Point3D loc = GetWorldLocation();

            if (!from.InLOS(loc) || !from.InRange(loc, 2))
            {
                from.LocalOverheadMessage(Server.Network.MessageType.Regular, 0x3E9, 1019045); // I can't reach that
                return;
            }
            else if (!IsAccessibleTo(from))
            {
                PublicOverheadMessage(Server.Network.MessageType.Regular, 0x3E9, 1061637); // You are not allowed to access 
                return;
            }

            if (!(HarvestSystem is Mining))
                from.SendLocalizedMessage(1010018); // What do you want to use this item on?

            if (from.FindItemOnLayer(Layer.OneHanded) == this ||
                   from.FindItemOnLayer(Layer.TwoHanded) == this)
            {
                HarvestSystem.BeginHarvesting(from, this);
            }
            else
            {
                if (this.RootParent != from)
                {
                    from.SendMessage("Voce precisa estar com isto na mochila");
                    return;
                }
                var tool = from.FindItemOnLayer(Layer.OneHanded);
                if (tool == null) tool = from.FindItemOnLayer(Layer.TwoHanded);
                if (tool != null)
                {
                    if (from.HasAction(HarvestSystem.GetLock(from, tool, null, null)))
                    {
                        from.SendMessage("Voce ja esta fazendo algo");
                        return;
                    }
                }
                from.ClearHands();
                from.EquipItem(this);
                HarvestSystem.BeginHarvesting(from, this);
            }
        }

        public HarvestSystem HarvestSystem => Mining.System;

        public bool AutoHarvest => true;

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
