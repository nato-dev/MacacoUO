using System;
using Server.Targeting;
using Server.Multis;
using Server.Mobiles;
using Server.Engines.PartySystem;
using System.Linq;
using Server.Engines.Exodus;

namespace Server.Items
{
    public class ExodusSummoningAlter : BaseDecayingItem
    {
        public override int LabelNumber { get { return 1153502; } } // exodus summoning altar

        [Constructable]
        public ExodusSummoningAlter() : base(0x14F0)
        {
            Name = "Pergaminho Estranho";
            this.PartyLoot = true;
            this.LootType = LootType.Regular;
            this.Weight = 1;
        }

        public override int Lifespan { get { return 604800; } }
        public override bool UseSeconds { get { return false; } }        

        public ExodusSummoningAlter(Serial serial)
            : base(serial)
        {
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
        
        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1054107); // This item must be in your backpack.
            }
            else if (Party.Get(from) == null)
            {
                from.SendLocalizedMessage(1153596); // You must join a party with the players you wish to perform the ritual with. 
            }
            else
            {
                from.SendLocalizedMessage(1153675); // The Summoning Altar must be built upon a shrine, within Trammel or Felucca it matters not...                
                from.Target = new SummoningTarget(from, this);
            }                
        }

        public class SummoningTarget : Target
        {            
            private Mobile m_Mobile;
            private Item m_Deed;

            public SummoningTarget(Mobile from, Item deed) : base(2, true, TargetFlags.None)
            {
                m_Mobile = from;
                m_Deed = deed;
            }

            public static bool IsValidTile(int itemID)
            {
                return (itemID >= 0x149F && itemID <= 0x14D6); 
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                Point3D loc = Point3D.Zero;
                if (targeted is StaticTarget || targeted is Item)
                {
                    bool valid = false;
                    if(targeted is StaticTarget)
                    {
                        StaticTarget targ = (StaticTarget)targeted;
                        valid = IsValidTile(targ.ItemID);
                        loc = targ.Location;
                    } else
                    {
                        var targ = (Item)targeted;
                        valid = IsValidTile(targ.ItemID);
                        loc = targ.Location;
                    }
                    

                    if (valid)
                    {
                        bool alter = from.Map.GetItemsInRange(loc, 5).Where(x => x is ExodusTomeAltar).Any();

                        if (alter)
                        {
                            from.SendLocalizedMessage(1153590); // An altar has already been built here. 
                        }
                        else if (ExodusTomeAltar.Altar == null) // && VerLorRegController.Active && VerLorRegController.Mobile != null && CheckExodus())
                        {
                            Point3D p = Point3D.Zero;
                            if (from.Region != null)
                                Shard.Debug(from.Region.Name);
                            if (from.Region != null && from.Region.Name == "Exodo" || from.Region.IsPartOf("Exodo"))
                            {
                                p = new Point3D(5266, 1177, 12);
                            }

                            if (p != Point3D.Zero)
                            {
                                ExodusTomeAltar altar = new ExodusTomeAltar(from);
                                altar.MoveToWorld(p, from.Map);
                                altar.Owner = from;
                                m_Deed.Delete();
                            }
                        }
                        else
                        {
                            from.SendMessage("Ja existe um altar construido"); // The master of this realm has already been summoned and is engaged in combat.  Your opportunity will come after he has squashed the current batch of intruders!
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(500269); // You cannot build that there.
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500269); // You cannot build that there.
                }
            }
        }

        public static bool CheckExodus() // Before ritual check
        {
            return ClockworkExodus.Instances.FirstOrDefault(m => m.Region.IsPartOf("Ver Lor Reg") && ((m.Hits >= m.HitsMax * 0.60 && m.MinHits >= m.HitsMax * 0.60) || (m.Hits >= m.HitsMax * 0.75))) != null;
        }
    }
}
