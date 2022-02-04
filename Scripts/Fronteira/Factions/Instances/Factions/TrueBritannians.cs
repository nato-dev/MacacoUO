using System;

namespace Server.Factions
{
    public class TrueBritannians : Faction
    {
        private static Faction m_Instance;
        public TrueBritannians()
        {
            m_Instance = this;

            this.Definition =
                new FactionDefinition(
                    2,
                    1254, // dark purple
                    2125, // gold
                    2214, // join stone : gold
                    2125, // broadcast : gold
                    0x76, 0x3EB2, // war horse
                    "Britoes Reais", "Britoes", "BR",
                    new TextDefinition(1011536, "Bretoes Reais"),
                    new TextDefinition(1060771, "Bretoes Reais"),
                    new TextDefinition(1011423, "<center>Bretoes Reais</center>"),
                    new TextDefinition(1011450, "Os Bretoes Reais são leais ao trono do Lorde Britânico. Eles se recusam" +
                                               "entregar suas terras natais ao vil Minax e os Senhoreses das Sombras" +
                                               ". Além disso, o Conselho de Magos ameaça a" +
                                               "existência de seu governante, e como tal eles se armaram, e" +
                                               "preparam-se para a guerra com todos."),
                    new TextDefinition(1011454, "Esta cidade eh controlada pelos Britoes Reais."),
                    new TextDefinition(1042254, "Este sigilo esta corrompido pelos Britoes Reais"),
                    new TextDefinition(1041045, "Alistamento Britoes Reais"),
                    new TextDefinition(1041383, "Pedra dos Britoes Reais"),
                    new TextDefinition(1011465, ": Britoes Reais"),
                    new TextDefinition(1005181, "Followers of Lord British will now be ignored."),
                    new TextDefinition(1005182, "Followers of Lord British will now be warned of their impending doom."),
                    new TextDefinition(1005183, "Followers of Lord British will now be attacked on sight."),
                    new StrongholdDefinition(
                        new Rectangle2D[]
                        {
                            new Rectangle2D(1292, 1556, 25, 25),
                            new Rectangle2D(1292, 1676, 120, 25),
                            new Rectangle2D(1388, 1556, 25, 25),
                            new Rectangle2D(1317, 1563, 71, 18),
                            new Rectangle2D(1300, 1581, 105, 95),
                            new Rectangle2D(1405, 1612, 12, 21),
                            new Rectangle2D(1405, 1633, 11, 5)
                        },
                        new Point3D(1419, 1622, 20),
                        new Point3D(1330, 1621, 50),
                        new Point3D[]
                        {
                            new Point3D(1328, 1627, 50),
                            new Point3D(1328, 1621, 50),
                            new Point3D(1334, 1627, 50),
                            new Point3D(1334, 1621, 50),
                            new Point3D(1340, 1627, 50),
                            new Point3D(1340, 1621, 50),
                            new Point3D(1345, 1621, 50),
                            new Point3D(1345, 1627, 50)
                        },
                        new Point3D(1331, 1621, 50)),
                    new RankDefinition[]
                    {
                        new RankDefinition(10, 991, 8, new TextDefinition(1060784, "Marechal")),
                        new RankDefinition(9, 950, 7, new TextDefinition(1060783, "General")),
                        new RankDefinition(8, 900, 6, new TextDefinition(1060782, "Major")),
                        new RankDefinition(7, 800, 6, new TextDefinition(1060782, "Capitao")),
                        new RankDefinition(6, 700, 5, new TextDefinition(1060781, "Coronel")),
                        new RankDefinition(5, 600, 5, new TextDefinition(1060781, "Tenente")),
                        new RankDefinition(4, 500, 5, new TextDefinition(1060781, "Sargento")),
                        new RankDefinition(3, 400, 4, new TextDefinition(1060780, "Cabo")),
                        new RankDefinition(2, 200, 4, new TextDefinition(1060780, "Soldado")),
                        new RankDefinition(1, 0, 4, new TextDefinition(1060780, "Recruta"))
                    },
                    new GuardDefinition[]
                    {
                        new GuardDefinition(typeof(FactionHenchman), 0x1403, 5000, 1000, 10, new TextDefinition(1011526, "HENCHMAN"), new TextDefinition(1011510, "Hire Henchman")),
                        new GuardDefinition(typeof(FactionMercenary),	0x0F62, 6000, 2000, 10, new TextDefinition(1011527, "MERCENARY"), new TextDefinition(1011511, "Hire Mercenary")),
                        new GuardDefinition(typeof(FactionKnight), 0x0F4D, 7000, 3000, 10, new TextDefinition(1011528, "KNIGHT"), new TextDefinition(1011497, "Hire Knight")),
                        new GuardDefinition(typeof(FactionPaladin), 0x143F, 8000, 4000, 10, new TextDefinition(1011529, "PALADIN"), new TextDefinition(1011498, "Hire Paladin")),
                    });
        }

        public static Faction Instance
        {
            get
            {
                return m_Instance;
            }
        }
    }
}
