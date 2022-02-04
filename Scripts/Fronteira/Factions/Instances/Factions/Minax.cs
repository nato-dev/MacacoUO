using System;

namespace Server.Factions
{
    public class Minax : Faction
    {
        private static Faction m_Instance;
        public Minax()
        {
            m_Instance = this;

            this.Definition =
                new FactionDefinition(
                    0,
                    1645, // dark red
                    1109, // shadow
                    1645, // join stone : dark red
                    1645, // broadcast : dark red
                    0x78, 0x3EAF, // war horse
                    "Minax", "minax", "Min",
                    new TextDefinition(1011534, "Minax"),
                    new TextDefinition(1060769, "Minax"),
                    new TextDefinition(1011421, "<center>Minax</center>"),
                    new TextDefinition(1011448, "Os seguidores de Minax assumiram o controle nas terras antigas, " +
                                               "e pretendem mantê-lo pelo tempo que puderem. Aliando-se" +
                                               "com orcs e principalmente selvagens, eles procuram" +
                                               "vingança contra Lord British, por ofensas reais e imaginárias," +
                                               "embora alguns dos seguidores desejem apenas causar estragos no" +
                                               "população inocente."),
                    new TextDefinition(1011453, "This city is controlled by Minax."),
                    new TextDefinition(1042252, "Este sigilo foi corrompido por Minax"),
                    new TextDefinition(1041043, "Pedra de Se Unir a Faction Minax"),
                    new TextDefinition(1041381, "Pedra da Faction Minax"),
                    new TextDefinition(1011463, ": Minax"),
                    new TextDefinition(1005190, "Followers of Minax will now be ignored."),
                    new TextDefinition(1005191, "Followers of Minax will now be told to go away."),
                    new TextDefinition(1005192, "Followers of Minax will now be hanged by their toes."),
                    new StrongholdDefinition(
                        new Rectangle2D[]
                        {
                            new Rectangle2D(1127, 2946, 70, 50)
                        },
                        new Point3D(1154, 2959, 0),
                        new Point3D(1144, 2963, 0),
                        new Point3D[]
                        {
                            new Point3D(1142, 2972, 0),
                            new Point3D(1142+2, 2972, 0),
                            new Point3D(1142+4, 2972, 0),
                            new Point3D(1142+6, 2972, 0),
                            new Point3D(1142+8, 2972, 0),
                            new Point3D(1142+10, 2972, 0),
                            new Point3D(1142+12, 2972, 0),
                            new Point3D(1142+14, 2972, 0)
                        },
                        new Point3D(1131, 2957, 0)),
                    new RankDefinition[]
                    {
                        //Recruta,Soldado,Cabo,Sargento,Tenente,Coronel,Capitao,Major,General,Marechal
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
                        new GuardDefinition(typeof(FactionBerserker),	0x0F4B, 7000, 3000, 10, new TextDefinition(1011505, "BERSERKER"), new TextDefinition(1011499, "Hire Berserker")),
                        new GuardDefinition(typeof(FactionDragoon), 0x1439, 8000, 4000, 10, new TextDefinition(1011506, "DRAGOON"), new TextDefinition(1011500, "Hire Dragoon")),
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
