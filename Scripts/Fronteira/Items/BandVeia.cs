using Server.Misc.Custom;
using Server.Targeting;

namespace Server.Ziden
{
    public class PergaminhoRunebook : Item
    {

        [Constructable]
        public PergaminhoRunebook() : base(0x1F35)
        {
            this.Name = "Pergaminho de Cargas de Runebook";
            this.Stackable = true;
        }

        public PergaminhoRunebook(Serial s) : base(s) { }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Use em um runebook para carrega-lo");
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Arraste isto para um runebook");
        }
    }
}
