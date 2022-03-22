using Server.Engines.VvV;
using Server.Fronteira.Guildas;
using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.ViceVsVirtue.Items
{
    public class QuadroRankingVvV : Item
    {

        [Constructable]
        public QuadroRankingVvV() : this(1)
        {
        }

        [Constructable]
        public QuadroRankingVvV(int amount) : base(16395)
        {
            Name = "Quadro Guerra Infinita";
            Stackable = true;
            Amount = amount;

            Hue = TintaPreta.COR;
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendGump(new GuildLeaderboardGump(from as PlayerMobile));

            var data = ViceVsVirtueSystem.Instance?.Battle?.CooldownEnds;
            var agora = DateTime.UtcNow;
            var falta = (data - agora);

            from.SendMessage($"Faltam {(int)Math.Floor(falta.Value.TotalHours)} horas para o inicio da guerra infinita");
        }

        public QuadroRankingVvV(Serial serial) : base(serial)
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
    }

    public class QuadroGuerras : Item
    {

        [Constructable]
        public QuadroGuerras() : this(1)
        {
        }

        [Constructable]
        public QuadroGuerras(int amount) : base(16395)
        {
            Name = "Quadro Guerras";
            Stackable = true;
            Amount = amount;

            Hue = TintaPreta.COR;
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendGump(new GuerrasAtivas(from as PlayerMobile));
        }

        public QuadroGuerras(Serial serial) : base(serial)
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
    }
}
