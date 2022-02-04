using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Items
{
    public class KeypunchReader : OrnateWoodenChest
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public Item Link { get; set; }

        [Constructable]
        public KeypunchReader() 
            : base()
        {
            this.Name = "Leitor de Cartoes";
            this.Weight = 0.0;
            this.Hue = 2500;
            this.Movable = false;       
        }

        public KeypunchReader(Serial serial) : base(serial)
        {
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (!base.OnDragDrop(from, dropped))
                return false;

            if (this.TotalItems >= 50)
            {
                CheckItems(from);
            }

            if (dropped is PunchCard)
            {
             
                Cartao(from);
            }

            return true;
        }

        public void Cartao(Mobile from)
        {
            from.OverheadMessage("* inseriu o cartao *");
            if(Link == null)
            {
                from.SendMessage("Parece que nada aconteceu... bug ?");
                return;
            }
            else if(Link.Visible)
            {
                return;
            }
            this.PublicOverheadMessage(Network.MessageType.Regular, 0, true, "* click *");
            this.Items.ToList().ForEach(i => {
                if(i is PunchCard || i is ExoticToolkit)
                    i.Movable = false;
            });
            if (!Link.Visible)
            {
                Link.Visible = true;
                Effects.SendBoltEffect(EffectMobile.Create(Link.Location, Link.Map, EffectMobile.DefaultDuration), true, 0, false);
                Timer.DelayCall(TimeSpan.FromSeconds(60), () => { Link.Visible = false; });
                Link.PublicOverheadMessage(Network.MessageType.Regular, 0, true, "* zap *");
            }
        }

        public override bool OnDragDropInto(Mobile from, Item item, Point3D p)
        {
            if (!base.OnDragDropInto(from, item, p))
                return false;

            if (this.TotalItems >= 50)
            {
                CheckItems(from);
            }

            if (item is PunchCard)
            {
               
                Cartao(from);
            }

            return true;
        }

        public void CheckItems(Mobile m)
        {
            List<Item> items = this.Items;

            var punch = items.Where(x => x is PunchCard);
            var kit = items.Where(x => x is ExoticToolkit);

            if (punch.Count() >= 50 && kit.Count() >= 1)
            {
                punch.ToList().ForEach(f => f.Delete());

                this.DropItem(new NexusAddonDeed());
                m.SendMessage("A maquina cospe uma escritura"); // As you feed the punch card into the machine it turns on! 
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
            writer.Write(Link);

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            if(version == 1)
            {
                Link = reader.ReadItem();
            }
        }
    }
}
