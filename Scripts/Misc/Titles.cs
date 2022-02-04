using System;
using System.Text;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;
using Server.Accounting;

namespace Server.Misc
{
    public class Titles
    {
        public const int MinFame = 0;
        public const int MaxFame = 32000;

        public static void AwardFame(Mobile m, int offset, bool message)
        {
            if (offset > 0)
            {
                if (m.Fame >= MaxFame)
                    return;

                offset -= m.Fame / 100;

                if (offset < 0)
                    offset = 0;
            }
            else if (offset < 0)
            {
                if (m.Fame <= MinFame)
                    return;

                offset -= m.Fame / 100;

                if (offset > 0)
                    offset = 0;
            }

            if ((m.Fame + offset) > MaxFame)
                offset = MaxFame - m.Fame;
            else if ((m.Fame + offset) < MinFame)
                offset = MinFame - m.Fame;

            m.Fame += offset;

            if (message)
            {
                if (offset > 40)
                    m.SendMessage("Voce ganhou muita fama"); // You have gained a lot of fame.
                else if (offset > 20)
                    m.SendMessage("Voce ganhou um bom bocado de fama"); // You have gained a good amount of fame.
                else if (offset > 10)
                    m.SendMessage("Voce ganhou alguma fama"); // You have gained some fame.
                else if (offset > 0)
                    m.SendMessage("Voce ganhou um pouco de fama"); // You have gained a little fame.
                else if (offset < -40)
                    m.SendMessage("Voce perdeu muita fama"); // You have lost a lot of fame.
                else if (offset < -20)
                    m.SendMessage("Voce perdeu um bocado de fama");  // You have lost a good amount of fame.
                else if (offset < -10)
                    m.SendMessage("Voce perdeu alguma fama"); // You have lost some fame.
                else if (offset < 0)
                    m.SendMessage("Voce perdeu um pouco de fama");  // You have lost a little fame.
            }
        }

        public const int MinKarma = -32000;
        public const int MaxKarma = 32000;

        public static void AwardKarma(Mobile m, int offset, bool message)
        {
            #region Mondain's Legacy
            if (m.Talisman is BaseTalisman)
            {
                BaseTalisman talisman = (BaseTalisman)m.Talisman;

                if (talisman.KarmaLoss > 0)
                    offset *= (1 + (int)(((double)talisman.KarmaLoss) / 100));
                else if (talisman.KarmaLoss < 0)
                    offset *= (1 - (int)(((double)-talisman.KarmaLoss) / 100));
            }
            #endregion

            #region Heritage Items
            int karmaLoss = AosAttributes.GetValue(m, AosAttribute.IncreasedKarmaLoss);

            if (karmaLoss != 0 && offset < 0)
            {
                offset -= (int)(offset * (karmaLoss / 100.0));
            }
            #endregion

            if (offset > 0)
            {
                if (m is PlayerMobile && ((PlayerMobile)m).KarmaLocked)
                    return;

                if (m.Karma >= MaxKarma)
                    return;

                offset -= m.Karma / 100;

                if (offset < 0)
                    offset = 0;
            }
            else if (offset < 0)
            {
                if (m.Karma <= MinKarma)
                    return;

                offset -= m.Karma / 100;

                if (offset > 0)
                    offset = 0;
            }

            if ((m.Karma + offset) > MaxKarma)
                offset = MaxKarma - m.Karma;
            else if ((m.Karma + offset) < MinKarma)
                offset = MinKarma - m.Karma;

            bool wasPositiveKarma = (m.Karma >= 0);

            m.Karma += offset;

            if (message)
            {
                if (offset > 40)
                    m.SendMessage("Voce ganhou muito karma"); // You have gained a lot of karma.
                else if (offset > 20)
                    m.SendMessage("Voce ganhou um bocado de karma"); // You have gained a good amount of karma.
                else if (offset > 10)
                    m.SendMessage("Voce ganhou karma"); // You have gained some karma.
                else if (offset > 0)
                    m.SendMessage("Voce ganhou um pouquinho de karma"); // You have gained a little karma.
                else if (offset < -40)
                    m.SendMessage("Voce perdeu muito karma"); // You have lost a lot of karma.
                else if (offset < -20)
                    m.SendMessage("Voce perdeu um bocado de karma"); // You have lost a good amount of karma.
                else if (offset < -10)
                    m.SendMessage("Voce perdeu karma"); // You have lost some karma.
                else if (offset < 0)
                    m.SendMessage("Voce perdeu um pouquinho de karma"); // You have lost a little karma.
            }

            if (!Core.AOS && wasPositiveKarma && m.Karma < 0 && m is PlayerMobile && !((PlayerMobile)m).KarmaLocked)
            {
                ((PlayerMobile)m).KarmaLocked = true;
                m.SendMessage("Karma trancado - destranque em uma ankh"); // Karma is locked.  A mantra spoken at a shrine will unlock it again.
            }
        }

        public static List<string> GetFameKarmaEntries(Mobile m)
        {
            List<string> list = new List<string>();
            int fame = m.Fame;
            int karma = m.Karma;

            for (int i = 0; i < m_FameEntries.Length; ++i)
            {
                FameEntry fe = m_FameEntries[i];

                if (fame >= fe.m_Fame)
                {
                    KarmaEntry[] karmaEntries = fe.m_Karma;

                    for (int j = 0; j < karmaEntries.Length; ++j)
                    {
                        KarmaEntry ke = karmaEntries[j];
                        StringBuilder title = new StringBuilder();

                        if ((karma >= 0 && ke.m_Karma >= 0 && karma >= ke.m_Karma) || (karma < 0 && ke.m_Karma < 0 && karma < ke.m_Karma))
                        {
                            list.Add(title.AppendFormat(m.Female ? Feminino(ke.m_Title) : ke.m_Title, m.Name, m.Female ? "Lady" : "Lord").ToString());
                        }
                    }
                }
            }

            return list;
        }

        public static string[] HarrowerTitles = new string[] { "Spite", "Opponent", "Hunter", "Venom", "Executioner", "Annihilator", "Champion", "Assailant", "Purifier", "Nullifier" };

        public static string ComputeFameTitle(Mobile beheld)
        {
            int fame = beheld.Fame;
            int karma = beheld.Karma;

            for (int i = 0; i < m_FameEntries.Length; ++i)
            {
                FameEntry fe = m_FameEntries[i];

                if (fame <= fe.m_Fame || i == (m_FameEntries.Length - 1))
                {
                    KarmaEntry[] karmaEntries = fe.m_Karma;

                    for (int j = 0; j < karmaEntries.Length; ++j)
                    {
                        KarmaEntry ke = karmaEntries[j];

                        if (karma <= ke.m_Karma || j == (karmaEntries.Length - 1))
                        {
                            return String.Format(beheld.Female ? Feminino(ke.m_Title) : ke.m_Title, beheld.Name, beheld.Female ? "Lady" : "Lord");
                        }
                    }

                    return String.Empty;
                }
            }
            return String.Empty;
        }

        private static Dictionary<string, string> Cache = new Dictionary<string, string>();

        public static string Feminino(string title)
        {
            string cached = null;
            Cache.TryGetValue(title, out cached);
            if (cached != null)
            {
                return cached;
            }

            if (title != null)
            {
                var remake = "";
                var split = title.Split(' ');
                foreach (var parte in split)
                {
                    if (parte.Contains("{"))
                        remake += parte + " ";
                    else
                    {
                        if (parte == "o")
                            remake += "a ";
                        else
                        {
                            if (parte.Substring(parte.Length - 1) == "o")
                            {
                                remake += parte.Substring(0, parte.Length - 1) + "a ";
                            }
                        }
                    }
                }
                var txt = remake.Substring(0, remake.Length - 1);
                Cache[title] = txt;
                return txt;
            }
            return title;
        }

        public static string ComputeTitle(Mobile beholder, Mobile beheld)
        {
            StringBuilder title = new StringBuilder();

            bool showSkillTitle = beheld.ShowFameTitle && ((beholder == beheld) || (beheld.Fame >= 5000));

            if (beheld.ShowFameTitle && beheld is PlayerMobile && ((PlayerMobile)beheld).FameKarmaTitle != null)
            {
                title.AppendFormat(((PlayerMobile)beheld).FameKarmaTitle, beheld.Name, beheld.Female ? "Lady" : "Lord");
            }
            else if (beheld.ShowFameTitle || (beholder == beheld))
            {
                title.Append(ComputeFameTitle(beheld));
            }
            else
            {
                title.Append(beheld.Name);
            }

            if (beheld is PlayerMobile && ((PlayerMobile)beheld).DisplayChampionTitle)
            {
                PlayerMobile.ChampionTitleInfo info = ((PlayerMobile)beheld).ChampionTitles;

                if (Core.SA)
                {
                    if (((PlayerMobile)beheld).CurrentChampTitle != null)
                        title.AppendFormat(((PlayerMobile)beheld).CurrentChampTitle);
                }
                else if (info.Harrower > 0)
                    title.AppendFormat(": {0} of Evil", HarrowerTitles[Math.Min(HarrowerTitles.Length, info.Harrower) - 1]);
                else
                {
                    int highestValue = 0, highestType = 0;
                    for (int i = 0; i < ChampionSpawnInfo.Table.Length; i++)
                    {
                        int v = info.GetValue(i);

                        if (v > highestValue)
                        {
                            highestValue = v;
                            highestType = i;
                        }
                    }

                    int offset = 0;
                    if (highestValue > 800)
                        offset = 3;
                    else if (highestValue > 300)
                        offset = (int)(highestValue / 300);

                    if (offset > 0)
                    {
                        ChampionSpawnInfo champInfo = ChampionSpawnInfo.GetInfo((ChampionSpawnType)highestType);
                        title.AppendFormat(": {0} of the {1}", champInfo.LevelNames[Math.Min(offset, champInfo.LevelNames.Length) - 1], champInfo.Name);
                    }
                }
            }

            string customTitle = beheld.Title;

            if (true)
            {
                if (beheld is PlayerMobile && ((PlayerMobile)beheld).PaperdollSkillTitle != null)
                    title.Append(", ").Append(((PlayerMobile)beheld).PaperdollSkillTitle);
                else if (beheld is BaseVendor)
                    title.AppendFormat(" {0}", customTitle);
            }
            else if (customTitle != null && (customTitle = customTitle.Trim()).Length > 0)
            {
                title.AppendFormat(" {0}", customTitle);
            }
            else if (showSkillTitle && beheld.Player)
            {
                string skillTitle = GetSkillTitle(beheld);

                if (skillTitle != null)
                {
                    title.Append(", ").Append(skillTitle);
                }
            }

            return title.ToString();
        }

        public static string GetSkillTitle(Mobile mob)
        {
            Skill highest = GetHighestSkill(mob);// beheld.Skills.Highest;

            if (highest != null && highest.BaseFixedPoint >= 300)
            {
                string skillLevel = GetSkillLevel(highest);
                string skillTitle = highest.Info.Title;

                if (mob.Female && skillTitle.EndsWith("man"))
                    skillTitle = skillTitle.Substring(0, skillTitle.Length - 3) + "woman";

                return String.Concat(skillLevel, " ", skillTitle);
            }

            return null;
        }

        public static string GetSkillTitle(Mobile mob, Skill skill)
        {
            if (skill != null && skill.BaseFixedPoint >= 300)
            {
                string skillLevel = GetSkillLevel(skill);
                string skillTitle = skill.Info.Title;

                if (mob.Female && skillTitle.EndsWith("man"))
                    skillTitle = skillTitle.Substring(0, skillTitle.Length - 3) + "woman";

                return String.Concat(skillLevel, " ", skillTitle);
            }

            return null;
        }

        private static Skill GetHighestSkill(Mobile m)
        {
            Skills skills = m.Skills;

            if (!Core.AOS)
                return skills.Highest;

            Skill highest = null;

            for (int i = 0; i < m.Skills.Length; ++i)
            {
                Skill check = m.Skills[i];

                if (highest == null || check.BaseFixedPoint > highest.BaseFixedPoint)
                    highest = check;
                else if (highest != null && highest.Lock != SkillLock.Up && check.Lock == SkillLock.Up && check.BaseFixedPoint == highest.BaseFixedPoint)
                    highest = check;
            }

            return highest;
        }

        private static readonly string[,] m_Levels = new string[,]
        {
            { "Neofito", "Neofito", "Neofito" },
            { "Iniciante", "Iniciante", "Iniciante" },
            { "Estudante", "Estudante", "Estudante" },
            { "Aprendiz", "Aprendiz", "Aprendiz" },
            { "Experiente", "Experiente", "Experiente" },
            { "Adepto", "Adepto", "Adepto" },
            { "Mestre", "Mestre", "Mestre" },
            { "Grao Mestre", "Grao Mestre", "Grao Mestre" },
            { "Epico", "Epico", "Epico" },
            { "Lendario", "Lendario", "Lendario" }
        };

        private static string GetSkillLevel(Skill skill)
        {
            return m_Levels[GetTableIndex(skill), GetTableType(skill)];
        }

        private static int GetTableType(Skill skill)
        {
            switch (skill.SkillName)
            {
                default:
                    return 0;
                case SkillName.Bushido:
                    return 1;
                case SkillName.Ninjitsu:
                    return 2;
            }
        }

        private static int GetTableIndex(Skill skill)
        {
            int fp = skill == null ? 300 : skill.BaseFixedPoint;

            fp = Math.Min(fp, 1200);

            return (fp - 300) / 100;
        }

        private static readonly FameEntry[] m_FameEntries = new FameEntry[]
        {
            new FameEntry(1249, new KarmaEntry[]
            {
                new KarmaEntry(-10000, "O Funesto {0}"),
                new KarmaEntry(-5000, "O Desprezivel {0}"),
                new KarmaEntry(-2500, "O Nocivo {0}"),
                new KarmaEntry(-1250, "O Soturno {0}"),
                new KarmaEntry(-625, "O Rude {0}"),
                new KarmaEntry(624, "{0}"),
                new KarmaEntry(1249, "O Correto {0}"),
                new KarmaEntry(2499, "O Gentil {0}"),
                new KarmaEntry(4999, "O Bom {0}"),
                new KarmaEntry(9999, "O Digno {0}"),
                new KarmaEntry(10000, "O Confiavel {0}")
            }),
            new FameEntry(2499, new KarmaEntry[]
            {
                new KarmaEntry(-10000, "O Fascinora {0}"),
                new KarmaEntry(-5000, "O Covarde {0}"),
                new KarmaEntry(-2500, "O Perverso {0}"),
                new KarmaEntry(-1250, "O Malicioso {0}"),
                new KarmaEntry(-625, "O Desonesto {0}"),
                new KarmaEntry(624, "O Notavel {0}"),
                new KarmaEntry(1249, "O Honesto {0}"),
                new KarmaEntry(2499, "O Respeitavel {0}"),
                new KarmaEntry(4999, "O Qualificado {0}"),
                new KarmaEntry(9999, "O Estimado {0}"),
                new KarmaEntry(10000, "O Honrado {0}")
            }),
            new FameEntry(4999, new KarmaEntry[]
            {
                new KarmaEntry(-10000, "O Abominável {0}"),
                new KarmaEntry(-5000, "O Depravado {0}"),
                new KarmaEntry(-2500, "O Vil {0}"),
                new KarmaEntry(-1250, "O Ignobil {0}"),
                new KarmaEntry(-625, "O Estupido {0}"),
                new KarmaEntry(624, "O Renomado {0}"),
                new KarmaEntry(1249, "O Nobre {0}"),
                new KarmaEntry(2499, "O Veneravel {0}"),
                new KarmaEntry(4999, "O Extraordinario {0}"),
                new KarmaEntry(9999, "O Admiravel {0}"),
                new KarmaEntry(10000, "O Grandioso {0}")
            }),
            new FameEntry(9999, new KarmaEntry[]
            {
                new KarmaEntry(-10000, "O Terrivel {0}"),
                new KarmaEntry(-5000, "O Cruel {0}"),
                new KarmaEntry(-2500, "O Maligno {0}"),
                new KarmaEntry(-1250, "O Sinistro {0}"),
                new KarmaEntry(-625, "O Infame {0}"),
                new KarmaEntry(624, "O Renomado {0}"),
                new KarmaEntry(1249, "O Probo {0}"),
                new KarmaEntry(2499, "O Eminente {0}"),
                new KarmaEntry(4999, "O Ilustre {0}"),
                new KarmaEntry(9999, "O Grandioso {0}"),
                new KarmaEntry(10000, "O Glorioso {0}")
            }),
            new FameEntry(10000, new KarmaEntry[]
            {
                new KarmaEntry(-10000, "O Temido {1} {0}"),
                new KarmaEntry(-5000, "O Cruel {1} {0}"),
                new KarmaEntry(-2500, "O Maligno {1} {0}"),
                new KarmaEntry(-1250, "O Sinistro {1} {0}"),
                new KarmaEntry(-625, "O Infame {1} {0}"),
                new KarmaEntry(624, "{1} {0}"),
                new KarmaEntry(1249, "O Renomado {1} {0}"),
                new KarmaEntry(2499, "O Eminente {1} {0}"),
                new KarmaEntry(4999, "O Ilustre {1} {0}"),
                new KarmaEntry(9999, "O Grandioso {1} {0}"),
                new KarmaEntry(10000, "O Glorioso {1} {0}")
            })
        };

        public static VeteranTitle[] VeteranTitles { get; set; }

        public static void Initialize()
        {
            VeteranTitles = new VeteranTitle[9];

            for (int i = 0; i < 9; i++)
            {
                VeteranTitles[i] = new VeteranTitle(1154341 + i, 2 * (i + 1));
            }
        }

        public static List<VeteranTitle> GetVeteranTitles(Mobile m)
        {
            Account a = m.Account as Account;

            if (a == null)
                return null;

            int years = (int)(DateTime.UtcNow - a.Created).TotalDays;
            years /= 365;

            if (years < 2)
                return null;

            List<VeteranTitle> titles = new List<VeteranTitle>();

            foreach (VeteranTitle title in VeteranTitles)
            {
                if (years >= title.Years)
                    titles.Add(title);
            }

            return titles;
        }
    }

    public class FameEntry
    {
        public int m_Fame;
        public KarmaEntry[] m_Karma;

        public FameEntry(int fame, KarmaEntry[] karma)
        {
            this.m_Fame = fame;
            this.m_Karma = karma;
        }
    }

    public class KarmaEntry
    {
        public int m_Karma;
        public string m_Title;

        public KarmaEntry(int karma, string title)
        {
            this.m_Karma = karma;
            this.m_Title = title;
        }
    }

    public class VeteranTitle
    {
        public int Title { get; set; }
        public int Years { get; set; }

        public VeteranTitle(int title, int years)
        {
            Title = title;
            Years = years;
        }
    }
}
