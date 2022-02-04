using Server.Misc.Custom;
using Server.Targeting;

namespace Server.Ziden
{
    public class PermissaoBarco : Item
    {

        [Constructable]
        public PermissaoBarco() : base(0x1F35)
        {
            this.Name = "Permissao para construir barcos";
            this.Stackable = true;
        }

        public PermissaoBarco(Serial s) : base(s) { }

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
