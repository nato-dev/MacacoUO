using Server.Items;
using Server.Regions;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Ziden.Items
{
    public class ChaveDg : Item
    {
        public static string GetDungeonName(string r)
        {
            for (var ri = 0; ri < _regionMap.GetLength(0); ri++)
            {
                if (r == _regionMap[ri, 0])
                    return _regionMap[ri, 1];
            }
            return "Dungeon Desconhecida...";
        }

        public static string[,] _regionMap = new string[,]
        {
            { "Wrong", "Prisao"},
            { "Shame", "Caverna Elemental"},
            { "Exodus", "Laboratorio Goblin"},
            { "Orc Cave", "Caverna dos Orcs" },
            { "Khalmer", "Masmorra de Arenito" },
            { "Pulma", "Templo da Medusa" },
            { "Destard", "Caverna dos Dragoes" }
        };

        [CommandProperty(AccessLevel.GameMaster)]
        public int Nivel { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Region { get; set; }

        public static void CriaChave(Container c)
        {
            return;

            var region = c.GetRegion();
            Shard.Debug("Criando chave dg " + region.Name);
            if (region != null && region is DungeonRegion)
            {
                List<BaseTreasureChestMod> lista = null;
                if (BaseTreasureChestMod.ByRegion.TryGetValue(region, out lista))
                {
                    if (lista.Count > 0)
                    {
                        var random = lista[Utility.Random(lista.Count)];
                        if (region.Name != null)
                        {
                            var level = random.GetLevel();
                            var chave = new ChaveDg(level);
                            chave.Region = region.Name;
                            c.AddItem(chave);
                            c.PublicOverheadMessage(Network.MessageType.Regular, 0, true, "!");

                        }

                    }
                }
            }
        }

        public ChaveDg(Serial s) : base(s) { }

        [Constructable]
        public ChaveDg(int nivel) : base(0x100F)
        {

            this.Nivel = nivel;
            switch (nivel)
            {
                case 0:
                    Name = "Chave Muito Velha";
                    Hue = Utility.RandomMetalHue();
                    break;
                case 1:
                    Name = "Chave Antiga";
                    Hue = Utility.RandomMetalHue();
                    break;
                case 2:
                    Name = "Chave Estranha";
                    Hue = Utility.RandomBirdHue();
                    break;
                case 3:
                    Name = "Chave Ornamentada";
                    Hue = Utility.RandomBirdHue();
                    break;
                case 4:
                    Name = "Chave Magica";
                    Hue = Loot.RandomRareDye();
                    break;
            }
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            list.Add("Chave Nivel " + (Nivel + 1));
            list.Add("Permite Abrir Um De Dungeons");
            list.Add("Dungeon: " + GetDungeonName(Region));
            base.AddNameProperties(list);
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Selecione onde deseja usar isto");
            from.Target = new Targ(this);
        }

        private class Targ : Target
        {
            private ChaveDg chave;

            public Targ(ChaveDg chave) : base(10, false, TargetFlags.None)
            {
                this.chave = chave;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (!from.InRegion(chave.Region))
                {
                    from.SendMessage("Essa chave nao funciona nesta dungeon...");
                    return;
                }

                var chest = targeted as BaseTreasureChestMod;
                if (chest != null)
                {
                    if (this.chave.Nivel >= chest.GetLevel())
                    {
                        from.PlaySound(0x4A);
                        this.chave.Consume();
                        from.SendMessage("Voce usou a chave para destrancar o bau");
                        chest.PublicOverheadMessage(Network.MessageType.Regular, 0, true, "* destrancado *");
                        chest.LockPick(from);
                        chest.Locked = false;
                    }
                    else
                    {
                        from.SendMessage("Esta chave nao pode abrir este tipo de bau.");
                    }
                }
                else
                {
                    from.SendMessage("Parece que esta chave nao serve para isto");
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            writer.Write((int)0);
            writer.Write(Nivel);
            writer.Write(Region);
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            var v = reader.ReadInt();
            this.Nivel = reader.ReadInt();
            this.Region = reader.ReadString();
            base.Deserialize(reader);
        }
    }
}
