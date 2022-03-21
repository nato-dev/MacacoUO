using Server;
using Server.Items;
using System;

public class AntiParaPotion : BasePotion
{
    public override double Delay
    {
        get
        {
            return 10.0;
        }
    }

    [Constructable]
    public AntiParaPotion(): base(3836, PotionEffect.AntiParalize)
    {
        Name = "Pocao Anti Paralizia";
        Hue = 2550;
        Stackable = true;
    }

    public AntiParaPotion(Serial serial)
        : base(serial)
    {
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0);
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }

    public override void Drink(Mobile m)
    {
        m.OverheadMessage("* bebendo algo *");
        Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
        {
            if(m.Alive && m.Paralyzed)
            {
                m.SendMessage("Voce terminou de tomar uma pocao para paralizia");
                m.Paralyzed = false;
                m.Stam = 1;
                m.Damage(m.Hits / 10);
                Consume();
                m.FixedEffect(0x375A, 10, 15);
                m.PlaySound(0x1E7);
            }
        });
    }
}
