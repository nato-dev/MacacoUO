using System;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Regions;

namespace Server.Gumps
{
    public class DeadGump : Gump
    {
        public DeadGump()
            : base(0, 0)
        {
            this.Closable = false;
            this.Disposable = false;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(141, 110, 378, 280, 9200);
            this.AddHtml(264, 123, 248, 20, @"Você Morreu", (bool)false, (bool)false);
            this.AddHtml(264, 154, 249, 60, @"Seu corpo ficará caído no local em que você morreu até que você retorne a ele.", (bool)false, (bool)false);
            this.AddButton(159, 219, 9721, 9721, (int)Buttons.Teleportar, GumpButtonType.Reply, 0);
            if (!Shard.WARSHARD)
                this.AddHtml(196, 223, 248, 25, @"Teleportar a um curandeiro", (bool)true, (bool)false);
            else
                this.AddHtml(196, 223, 248, 25, @"Teleportar para o Hall", (bool)true, (bool)false);
            this.AddHtml(195, 296, 248, 25, @"Continuar como alma", (bool)true, (bool)false);
            this.AddButton(159, 292, 9721, 9721, (int)Buttons.Continuar, GumpButtonType.Reply, 0);
            if (Shard.WARSHARD)
                this.AddHtml(197, 246, 309, 35, @"Voce será ressuscitado e enviado ao Hall", (bool)false, (bool)false);
            else
                this.AddHtml(197, 246, 309, 35, @"Voce será ressuscitado pelo curandeiro mais proximo", (bool)false, (bool)false);
            this.AddHtml(196, 321, 307, 59, @"Continue vagando como alma até encontrar um curandeiro ou alguém que lhe retorne à vida", (bool)false, (bool)false);
            this.AddItem(187, 153, 3808);
            this.AddItem(163, 133, 3799);
            this.AddItem(148, 162, 3897);
            this.AddItem(195, 166, 3794);
            this.AddItem(193, 173, 587);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            base.OnResponse(sender, info);

            var m = (PlayerMobile)sender.Mobile;

            switch (info.ButtonID)
            {
                case (int)Buttons.Continuar:
                    m.SendMessage("Voce continuará como uma alma penada");
                    return;

                case (int)Buttons.Teleportar:

                    if (Shard.WARSHARD)
                    {
                        var tempo = TimeSpan.FromSeconds(8);
                        m.SendMessage(0x00FE, "Você será enviado para o Hall dentro de alguns segundos.");
                        m.Freeze(tempo);
                        Timer.DelayCall(tempo, t => Revive(t), m);
                        return;
                    }

                    BaseHealer curandeiro = null;
                    var loc = Point3D.Zero;
                    double maxD = 500;
                    foreach (var h in BaseHealer.healers)
                    {
                        if (h.Map != m.Map)
                            continue;

                        var d = h.Location.GetDistance(m.Location);
                        if (d < maxD)
                        {
                            maxD = d;
                            curandeiro = h;
                            loc = curandeiro.Location;
                        }
                    }

                    if (curandeiro == null)
                    {
                        Shard.Debug("Checking last teleport");
                        if (m.LastDungeonEntrance != Point3D.Zero && m.Region != null && (m.Region is DungeonRegion || m.Region is DungeonGuardedRegion))
                        {
                            Shard.Debug("Achei last teleport"); 
                            loc = m.LastDungeonEntrance;
                        }
                    }

                    if (loc.X != 0 && loc.Y != 0)
                    {
                        Effects.SendLocationEffect(m.Location, m.Map, 0x3728, 10, 10);
                        BaseCreature.TeleportPets(m, loc, m.Map);

                        m.MoveToWorld(loc, m.Map);

                        Effects.SendLocationEffect(m.Location, m.Map, 0x3728, 10, 10);
                        m.SendGump(new ResurrectGump(m, ResurrectMessage.Healer));

                        return;
                    } else
                    {
                        m.SendMessage("Nao foi encontrado um local para levar sua alma...");
                    }
                    
                    break;
            }
        }

        public static void Revive(Mobile m)
        {
            var hall = CharacterCreation.WSHALL;
            BaseCreature.TeleportPets(m, hall, Map.Malas);
            m.PlaySound(0x214);
            m.FixedEffect(0x376A, 10, 16);
            m.Resurrect();
            m.MoveToWorld(hall, Map.Malas);
            m.Frozen = false;
            m.SendMessage(0x00FE, "Você ganhou uma nova chance e retornou ao Hall.");
        }

        public enum Buttons
        {
            Continuar,
            Teleportar,
        }

    }
}
