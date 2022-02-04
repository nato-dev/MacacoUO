using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.Engines.XmlSpawner2
{
    public class XmlCast : XmlAttachment
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public string SpellName { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Chance { get; set; }
         
        public XmlCast(ASerial serial) : base(serial)
        {
        }

        [Attachable]
        public XmlCast(string spell, int chance)
        {
            SpellName = spell;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
            writer.Write(SpellName);
            writer.Write(Chance);
        }


        public Spell GetSpell(Mobile caster)
        {
            try
            {
                return SpellRegistry.NewSpell(SpellName, caster, null);
            } catch(Exception e)
            {
                return null;
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            SpellName = reader.ReadString();
            Chance = reader.ReadInt();
        }
        
    }
}
