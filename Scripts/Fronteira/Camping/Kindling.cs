using System;
using System.Collections;
using Server.Network;
using Server.Regions;

namespace Server.Items
{
    public class Kindling : Item
    {
        [Constructable]
        public Kindling()
            : this(1)
        {
        }

        [Constructable]
        public Kindling(int amount)
            : base(0xDE1)
        {
            Stackable = true;
            Weight = 1.0;
            Amount = amount;
        }

        public Kindling(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
                Weight = 1.0;
        }

        public void Acende()
        {

        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.IsCooldown("fogueira"))
                return;
            from.SetCooldown("fogueira", TimeSpan.FromSeconds(5));

            Shard.Debug("Fogueira", from);
            if (!VerifyMove(from))
                return;

            if (!from.InRange(GetWorldLocation(), 2))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                return;
            }

            if(this.Parent != null)
            {
                from.SendMessage("Voce precisa colocar a fogueira no chao");
                return;
            }

            bool f = false;
            var items = this.Map.GetItemsInRange(this.Location);
            foreach(var item in items)
            {
                if(item is Campfire)
                {
                    item.Consume();
                    f = true;
                }
            }
            items.Free();

            Point3D fireLocation = GetFireLocation(from);

            if (fireLocation == Point3D.Zero)
            {
                from.SendMessage("Nao a local para uma fogueira aqui"); // There is not a spot nearby to place your campfire.
            }
            else if (!from.CheckSkillMult(SkillName.Camping, 0.0, 100.0))
            {
                from.SendMessage("Voce nao conseguir ascender a fogueira"); // You fail to ignite the campfire.
            }
            else
            {
                Consume(1);

                //if (!Deleted && Parent == null)
                //    from.PlaceInBackpack(this);
                fireLocation.Z = fireLocation.Z + 1;
                var campfire = new Campfire();
                campfire.nomeDeQUemAscendeu = from.Name;
                campfire.MoveToWorld(fireLocation, from.Map);

                if (from.Skills[SkillName.Camping].Value >= 80)
                {
                    campfire.regenera = true;
                    from.SendMessage("Voce ascende a fogueira com perfeicao. Esta fogueira regenerara HP e Stamina de jogadores proximos."); 
                }
            }
        }

        private Point3D GetFireLocation(Mobile from)
        {
            //if (from.Region.IsPartOf<DungeonRegion>())
            //    return Point3D.Zero;

            if (Parent == null)
                return Location;
            else
                return Point3D.Zero;
        }

        private void AddOffsetLocation(Mobile from, int offsetX, int offsetY, ArrayList list)
        {
            Map map = from.Map;

            int x = from.X + offsetX;
            int y = from.Y + offsetY;

            Point3D loc = new Point3D(x, y, from.Z);

            if (map.CanFit(loc, 1) && from.InLOS(loc))
            {
                list.Add(loc);
            }
            else
            {
                loc = new Point3D(x, y, map.GetAverageZ(x, y));

                if (map.CanFit(loc, 1) && from.InLOS(loc))
                    list.Add(loc);
            }
        }
    }
}
