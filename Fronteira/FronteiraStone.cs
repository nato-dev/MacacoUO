using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Fronteira
{
    public class FronteiraStone : Item
    {
        [Constructable]
        public FronteiraStone(): base(4484)
        {
            Name = "Fronteira Stone";
            Visible = false;
        }

        public FronteiraStone(Serial s) : base(s) { }

        /*
        public string Alchemy { get; set; }
        public string Anatomy { get; set; }
        public string AnimalLore { get; set; }
        public string ItemID { get; set; }
        public string ArmsLore { get; set; }
        public string Parry { get; set; }
        public string Begging { get; set; }
        public string Blacksmith { get; set; }
        public string Fletching { get; set; }
        public string Peacemaking { get; set; }
        public string Camping { get; set; }
        public string Carpentry { get; set; }
        public string Cartography { get; set; }
        public string Cooking { get; set; }
        public string DetectHidden { get; set; }
        public string Discordance { get; set; }
        public string EvalInt { get; set; }
        public string Healing { get; set; }
        public string Fishing { get; set; }
        public string Forensics { get; set; }
        public string Herding { get; set; }
        public string Hiding { get; set; }
        public string Provocation { get; set; }
        public string Inscribe { get; set; }
        public string Lockpicking { get; set; }
        public string Magery { get; set; }
        public string MagicResist { get; set; }
        public string Tactics { get; set; }
        public string Snooping { get; set; }
        public string Musicianship = 29,
        public string Poisoning = 30,
        public string Archery = 31,
        public string SpiritSpeak = 32,
        public string Stealing = 33,
        public string Tailoring = 34,
        public string AnimalTaming = 35,
        public string Jewelcrafting = 36,
        public string Tinkering = 37,
        public string Tracking = 38,
        public string Veterinary = 39,
        public string Swords = 40,
        public string Macing = 41,
        public string Fencing = 42,
        public string Wrestling = 43,
        public string Lumberjacking = 44,
        public string Mining = 45,
        public string Meditation = 46,
        public string Stealth = 47,
        public string RemoveTrap = 48,
        public string Necromancy = 49,
        public string Focus = 50,
        public string Chivalry = 51,
        public string Bushido = 52,
        public string Ninjitsu = 53,
        public string Spellweaving = 54,
        public string Mysticism = 55,
        public string Imbuing = 56,
        public string Throwing = 57
        */



        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }

    }
}
