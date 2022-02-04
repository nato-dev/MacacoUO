using System;

namespace Server.Factions
{
    public class CouncilOfMages : Faction
    {
        private static Faction m_Instance;
        public CouncilOfMages()
        {
            m_Instance = this;

            this.Definition =
                new FactionDefinition(
                    1,
                    1325, // blue
                    1310, // bluish white
                    1325, // join stone : blue
                    1325, // broadcast : blue
                    0x77, 0x3EB1, // war horse
                    "Concelho dos Magos", "Concelho", "CdM",
                    new TextDefinition(1011535, "Concelho dos Magos"),
                    new TextDefinition(1060770, "Concelho dos Magos"),
                    new TextDefinition(1011422, "<center>Concelho dos Magos</center>"),
                    new TextDefinition(1011449, "O conselho dos Magos tem suas raízes na cidade de Moonglow, onde" +
                                               "eles se reuniram uma vez. Eles começaram como um pequeno movimento, dedicado a" +
                                               "convocando o Estrangeiro, que antes salvou as terras uma vez. A" +
                                               "série de guerras e assassinatos e julgamentos ilegítimos por aqueles que são leais a" +
                                               "Lord British fez com que o grupo assumisse a bandeira da guerra."),
                    new TextDefinition(1011455, "Esta cidade eh controlada pelo Concelho dos Magos."),
                    new TextDefinition(1042253, "Sigilo corrompido pelo Concelho dos Magos"),
                    new TextDefinition(1041044, "Alistamento Concelho dos Magos"),
                    new TextDefinition(1041382, "Pedra do Concelho dos Magos"),
                    new TextDefinition(1011464, ": Concelho dos Magos"),
                    new TextDefinition(1005187, "Members of the Council of Mages will now be ignored."),
                    new TextDefinition(1005188, "Members of the Council of Mages will now be warned to leave."),
                    new TextDefinition(1005189, "Members of the Council of Mages will now be beaten with a stick."),
                    Settings.NewCoMLocation ?
                    // New CoM Location
                    new StrongholdDefinition(
                        new Rectangle2D[]
                        {
                            new Rectangle2D( 4463, 1488, 16, 31 ),
                            new Rectangle2D( 4445, 1519, 47, 55 )
                        },
                        new Point3D(4469, 1486, 0),
                        new Point3D(4457, 1544, 0),
                        new Point3D[]
                        {
                            new Point3D( 4464, 1534, 21 ),
                            new Point3D( 4464, 1536, 21 ),
                            new Point3D( 4466, 1534, 21 ),
                            new Point3D( 4466, 1536, 21 ),
                            new Point3D( 4468, 1534, 21 ),
                            new Point3D( 4468, 1536, 21 ),
                            new Point3D( 4470, 1534, 21 ),
                            new Point3D( 4470, 1536, 21 )
                        },
                        new Point3D(4458, 1544, 0))
                    : // Old CoM Location
                    new StrongholdDefinition(
                        new Rectangle2D[]
                        {
                            new Rectangle2D(3756, 2232, 4, 23),
                            new Rectangle2D(3760, 2227, 60, 28),
                            new Rectangle2D(3782, 2219, 18, 8),
                            new Rectangle2D(3778, 2255, 35, 17)
                        },
                        new Point3D(3750, 2241, 20),
                        new Point3D(3795, 2259, 20),
                        new Point3D[]
                        {
                            new Point3D(3793, 2255, 20),
                            new Point3D(3793, 2252, 20),
                            new Point3D(3793, 2249, 20),
                            new Point3D(3793, 2246, 20),
                            new Point3D(3797, 2255, 20),
                            new Point3D(3797, 2252, 20),
                            new Point3D(3797, 2249, 20),
                            new Point3D(3797, 2246, 20)
                        },
                        new Point3D(3796, 2259, 20)),
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
                        new GuardDefinition(typeof(FactionSorceress),	0x0E89, 7000, 3000, 10, new TextDefinition(1011507, "SORCERESS"), new TextDefinition(1011501, "Hire Sorceress")),
                        new GuardDefinition(typeof(FactionWizard), 0x13F8, 8000, 4000, 10, new TextDefinition(1011508, "ELDER WIZARD"),	new TextDefinition(1011502, "Hire Elder Wizard")),
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
