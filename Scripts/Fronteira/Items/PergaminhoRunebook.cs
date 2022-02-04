using Server.Misc.Custom;
using Server.Targeting;

namespace Server.Ziden
{
    public class BandVeia : Item
    {

        [Constructable]
        public BandVeia() : base(0x0E20)
        {
            this.Name = "Bandagem Usada";
            this.Stackable = true;
        }

        public BandVeia(Serial s) : base(s) { }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Talvez algumas pessoas gostariam de lavar isto e re-usar, mas isso nao seria nada higienico.");
        }
    }
}
