using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Fronteira.Pvm;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;

public class GateDG : Item
{
    [Constructable]
    public GateDG()
        : base(0xF6C)
    {
        Movable = false;
        Stackable = false;
    }

    public GateDG(Serial serial)
        : base(serial)
    {
    }

    public static void Completa(PlayerMobile m, CustomDungeons nome)
    {
        m.SendMessage(78, "Voce completou a dungeon " + nome);
        m.SendMessage("Novas dungeons o guardam...");
        m.DungeonsCompletas.Add(nome);
        var from = m;
        Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0, 0, 0, 0, 0, 5060, 0);
        Effects.PlaySound(from.Location, from.Map, 0x243);

        Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 6, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
        Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 4, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
        Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 6, from.Y - 4, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);

        Effects.SendTargetParticles(from, 0x375A, 35, 90, 0x00, 0x00, 9502, (EffectLayer)255, 0x100);

    }

    public override bool OnMoveOver(Mobile m)
    {
        if (!this.Visible)
            return true;

        if (m is PlayerMobile)
        {

            var p = m as PlayerMobile;

            m.SendGump(new DungeonSelect(GetDungeons(p), m));
        }
        return true;
    }

    public List<DungeonOption> Dgs = new List<DungeonOption>(new DungeonOption[] {
        new DungeonOption() {Map = Map.Felucca, Location = new Point3D(5187, 638, 0), Name=CustomDungeons.FARAOH },
        new DungeonOption() {Map = Map.Felucca, Location = new Point3D(5456, 1862, 0), Name=CustomDungeons.PIRATAS },
    });

    public List<DungeonOption> GetDungeons(PlayerMobile player)
    {
        List<DungeonOption> r = new List<DungeonOption>();
        for (var x = 0; x < Dgs.Count; x++)
        {
            if (x == 0 || player.DungeonsCompletas.Contains(Dgs[x - 1].Name))
                r.Add(Dgs[x]);
        }
        return r;
    }

    public override bool OnMoveOff(Mobile m)
    {
        if (m.HasGump(typeof(DungeonSelect)))
            m.CloseGump(typeof(DungeonSelect));
        return base.OnMoveOff(m);
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
}
