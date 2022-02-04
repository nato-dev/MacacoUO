
using System;
using Server;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
    public class ArmadilhaCamaleao : BaseTrap
    {
        [Constructable]
        public ArmadilhaCamaleao() : base(1)
        {
            switch (Utility.Random(24))
            {
                case 0: ItemID = 2475; Name = "Bau"; break;
                case 1: ItemID = 4650; Name = "sangue"; break;
                case 2: ItemID = 3787; Name = "ossos"; break;
                case 3: ItemID = 6004; Name = "pedra"; break;
                case 4: ItemID = 6924; Name = "ossos"; break;
                case 5: ItemID = 4655; Name = "sangue"; break;
                case 6: ItemID = 3350; Name = "cogumelo"; break;
                case 7: ItemID = 3651; Name = "bau"; break;
                case 8: ItemID = 4651; Name = "sangue"; break;
                case 9: ItemID = 2323; Name = "terra"; break;
                case 10: ItemID = 7068; Name = "galhos"; break;
                case 11: ItemID = 6884; Name = "caveira"; break;
                case 12: ItemID = 2475; Name = "bau"; break;
                case 13: ItemID = 4650; Name = "sangue"; break;
                case 14: ItemID = 3787; Name = "ossos"; break;
                case 15: ItemID = 6004; Name = "pedra"; break;
                case 16: ItemID = 6924; Name = "ossos"; break;
                case 17: ItemID = 4655; Name = "sangue"; break;
                case 18: ItemID = 3350; Name = "cogumelo"; break;
                case 19: ItemID = 3651; Name = "bau"; break;
                case 20: ItemID = 4651; Name = "sangue"; break;
                case 21: ItemID = 2323; Name = "terra"; break;
                case 22: ItemID = 7068; Name = "galhos"; break;
                case 23: ItemID = 6884; Name = "caveira"; break;
            }
        }


        public override bool PassivelyTriggered { get { return true; } }
        public override TimeSpan PassiveTriggerDelay { get { return TimeSpan.Zero; } }
        public override int PassiveTriggerRange { get { return 2; } }
        public override TimeSpan ResetDelay { get { return TimeSpan.FromSeconds(0.0); } }

        public override void OnTrigger(Mobile from)
        {

            if (from is BaseCreature)
                return;

            from.Hidden = false;

            Mobile spawn;
            spawn = new Chameleon();
            spawn.MoveToWorld(this.Location, this.Map);
            spawn.Combatant = from;


            spawn.Say("* Camaleao se revela *");
            this.Delete();

            //this.Delete;
            //Effects.SendLocationEffect( Location, Map, 0x1D99, 48, 2, GetEffectHue(), 0 );

            //if ( from.Alive && CheckRange( from.Location, 0 ) )
            //	Spells.SpellHelper.Damage( TimeSpan.FromTicks( 1 ), from, null, Utility.Dice( 10, 7, 0 ) );
        }

        public ArmadilhaCamaleao(Serial serial) : base(serial)
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
    }
}
