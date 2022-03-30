using Server.Gumps;
using System;

namespace Server.Items
{

    public class RareCraftableItem : Item
    {
        public RareCraftableItem(Serial s) : base(s) { }
        public RareCraftableItem(int itemId) : base(itemId) { }

        public override bool Decays
        {
            get
            {
                return false;
            }
        }
        public override void OnDoubleClick(Mobile from)
        {
            Server.Commands.CommandSystem.Handle(from, ".rctest");
            from.SendMessage(78, "[DICA] Voce talvez precise de pozinho transformador para usar isto. Voce pode comprar isto em qualquer joalheiro.");
            base.OnDoubleClick(from);
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }

   
    ////////////////////////////////////////////////////////////
    // IPY Craftable rares and ingredients
    ////////////////////////////////////////////////////////////
    [FurnitureAttribute]
    public class DyeableCurtainSouth : Item, Server.Items.IDyable
    {
        [Constructable]
        public DyeableCurtainSouth() : base(0x160E) { Weight = 1.0; }
        public DyeableCurtainSouth(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "cortina"; } }

        public bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted)
                return false;
            Hue = sender.DyedHue;
            return true;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    [FurnitureAttribute]
    public class DyeableCurtainEast : Item, Server.Items.IDyable
    {
        [Constructable]
        public DyeableCurtainEast() : base(0x160D) { Weight = 1.0; }
        public DyeableCurtainEast(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "cortina"; } }

        public bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted)
                return false;
            Hue = sender.DyedHue;
            return true;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    public class RareFishnetSmall : RareCraftableItem
    {
        public override string DefaultName
        {
            get
            {
                return "rede de pesca (raro)";
            }
        }

        [Constructable]
        public RareFishnetSmall()
            : base(0x0DCB)
        {
            Weight = 1.0;
        }

        public RareFishnetSmall(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }

    public class RareFishnetLarge : RareCraftableItem
    {
        [Constructable]
        public RareFishnetLarge() : base(0x0DCA) { Weight = 1.0; }
        public RareFishnetLarge(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "rede de pesca (raro)"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    public class RareFlask : RareCraftableItem
    {
        [Constructable]
        public RareFlask() : base(0x182D) { Weight = 1.0; }
        public RareFlask(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "frasco (raro)"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }
    public class RareVial : RareCraftableItem
    {
        [Constructable]
        public RareVial() : base(0x21FE) { Weight = 1.0; }
        public RareVial(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "tubo"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    public class RareArrowBundle : RareCraftableItem
    {
        [Constructable]
        public RareArrowBundle() : base(0x0F40) { Weight = 1.0; }
        public RareArrowBundle(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "flechas"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    public class RareBoltBundle : RareCraftableItem
    {
        [Constructable]
        public RareBoltBundle() : base(0x1BFC) { Weight = 1.0; }
        public RareBoltBundle(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "dardos"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    public class RareGoldIngotBundle : RareCraftableItem
    {
        [Constructable]
        public RareGoldIngotBundle() : base(0x1BEA) { Weight = 10.0; }
        public RareGoldIngotBundle(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "lingotes de ouro"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    public class RareCopperIngotBundle : RareCraftableItem
    {
        [Constructable]
        public RareCopperIngotBundle() : base(0x1BE4) { Weight = 10.0; }
        public RareCopperIngotBundle(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "lingotes de cobre"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    public class RareBeeswax : RareCraftableItem
    {
        [Constructable]
        public RareBeeswax() : base(0x1426) { Weight = 1.0; }
        public RareBeeswax(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "cera de abelha"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    public class RareHorseDung : RareCraftableItem
    {
        [Constructable]
        public RareHorseDung() : base(0x0F3B) { Weight = 1.0; }
        public RareHorseDung(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "coco de cavalo"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    public class RareBook : RareCraftableItem
    {
        [Constructable]
        public RareBook() : base(0x0FF4) { Weight = 1.0; }
        public RareBook(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "livro"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    public class RareOpenBook : RareCraftableItem
    {
        [Constructable]
        public RareOpenBook() : base(0x0FBD) { Weight = 1.0; }
        public RareOpenBook(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "livro"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    public class RareLogPile : RareCraftableItem
    {
        [Constructable]
        public RareLogPile() : base(0x1BE1) { Weight = 25.0; }
        public RareLogPile(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "toras"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    public class RareFoldedSheets : RareCraftableItem
    {
        [Constructable]
        public RareFoldedSheets() : base(0x0A92) { Weight = 1.0; }
        public RareFoldedSheets(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "lencol"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    public class RareChains : RareCraftableItem
    {
        [Constructable]
        public RareChains() : base(0x1A07) { Weight = 10.0; }
        public RareChains(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "correntes"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    public class RareNecroScroll : RareCraftableItem
    {
        [Constructable]
        public RareNecroScroll() : base(0x2265) { Weight = 1; }
        public RareNecroScroll(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "palavras proibidas"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }


    public class RareBrokenChair : RareCraftableItem
    {
        [Constructable]
        public RareBrokenChair() : base(0x0C1C) { Weight = 10.0; }
        public RareBrokenChair(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "cadeira quebrada"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    public class RarePegboard : RareCraftableItem
    {
        [Constructable]
        public RarePegboard() : base(0x0c39) { Weight = 10.0; }
        public RarePegboard(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "tabua de encaixe"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    public class RareBerries : Item
    {
        [Constructable]
        public RareBerries() : base(0x0D1A) { Weight = 1.0; Hue = 0x22; }
        public RareBerries(Serial serial) : base(serial) { }

        public override string DefaultName { get { return "amoras raras"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); //version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    public class RareDartboard : RareCraftableItem
    {
        [Constructable]
        public RareDartboard() : base(0x1E2E) { Weight = 10.0; }
        public RareDartboard(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "dardos"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    public class ValeDecoracaoRara : RareCraftableItem
    {
        [Constructable]
        public ValeDecoracaoRara() : base(0x9F64) { Hue = 200; Weight = 0.1; Stackable = false; }
        public ValeDecoracaoRara(Serial serial) : base(serial) { }
        public string QuemDropou { get; set; }
        public override string DefaultName { get { return "caixa misteriosa rara"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(1); // version
            writer.Write(QuemDropou);
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendGump(new CaixaGump());
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            if (version >= 1)
                QuemDropou = reader.ReadString();
            Stackable = false;
            Hue = 200;
        }
    }

    public class ValeDecoracaoComum : RareCraftableItem
    {
        [Constructable]
        public ValeDecoracaoComum() : base(0x9F64) { Weight = 0.1; Stackable = false; }
        public ValeDecoracaoComum(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "caixa misteriosa"; } }
        public string QuemDropou { get; set; }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
            writer.Write(QuemDropou);
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendGump(new CaixaGumpComum());
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            Stackable = false;
            QuemDropou = reader.ReadString();
        }
    }

    public class RareSkeleton : RareCraftableItem
    {
        [Constructable]
        public RareSkeleton() : base(0x1D8F) { Weight = 10.0; }
        public RareSkeleton(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "esqueleto (raro)"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    public class RarePot : RareCraftableItem
    {
        [Constructable]
        public RarePot() : base(0x09E0) { Weight = 10.0; }
        public RarePot(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "pote"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }

    // vendor sold! Ingredient in the rares crafting (goldsink item)
    public class TransformationDust : RareCraftableItem
    {
        [Constructable]
        public TransformationDust() : base(0x5745) { Weight = 1.0; Stackable = true; }
        public TransformationDust(Serial serial) : base(serial) { }
        public override string DefaultName { get { return "pozinho transformador"; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            Server.Commands.CommandSystem.Handle(from, ".rctest");
            //base.OnDoubleClick(from);
        }
    }
}
