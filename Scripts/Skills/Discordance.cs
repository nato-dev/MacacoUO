#region References
using System;
using System.Collections;

using Server.Engines.XmlSpawner2;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Engines.Quests;
#endregion

namespace Server.SkillHandlers
{
    public class Discordance
    {
        private static readonly Hashtable m_Table = new Hashtable();

        public static bool UnderEffects(Mobile m)
        {
            return m != null && m_Table.Contains(m);
        }

        public static int CUSTO_STAMINA = 5;

        public static bool RemoveEffect(Mobile m)
        {
            if (m_Table.Contains(m))
            {
                var info = m_Table[m] as DiscordanceInfo;
                if (info == null)
                {
                    return false;
                }
                End(info);
                return true;
            }
            return false;
        }

        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.Discordance].Callback = OnUse;
        }

        public static TimeSpan OnUse(Mobile m)
        {
            if (m.Stam < CUSTO_STAMINA)
            {
                m.SendMessage("Voce esta muito cansado para tocar");
                return TimeSpan.FromSeconds(2.0);
            }

            m.RevealingAction();

            BaseInstrument.PickInstrument(m, OnPickedInstrument);

            return TimeSpan.FromSeconds(1.0); // Cannot use another skill for 1 second
        }

        public static void OnPickedInstrument(Mobile from, BaseInstrument instrument)
        {
            from.RevealingAction();
            from.SendLocalizedMessage("Selecione o alvo da musica enfraquecedora"); // Choose the target for your song of discordance.
            from.Target = new DiscordanceTarget(from, instrument);
            from.NextSkillTime = Core.TickCount + 6000;
        }

        public static bool GetEffect(Mobile targ, ref int effect)
        {
            DiscordanceInfo info = m_Table[targ] as DiscordanceInfo;

            if (info == null)
            {
                return false;
            }

            effect = info.m_Effect;
            return true;
        }

        public static void End(DiscordanceInfo info)
        {
            if (info.m_Timer != null)
            {
                info.m_Timer.Stop();
            }
            var targ = info.m_Creature;
            info.Clear();
            m_Table.Remove(targ);
        }

        private static void ProcessDiscordance(DiscordanceInfo info)
        {
            Mobile from = info.m_From;
            Mobile targ = info.m_Creature;
            bool ends = false;

            // According to uoherald bard must remain alive, visible, and 
            // within range of the target or the effect ends in 15 seconds.
            if (!targ.Alive || targ.Deleted || !from.Alive || from.Hidden)
            {
                ends = true;
            }
            else
            {
                int range = (int)targ.GetDistanceToSqrt(from);
                int maxRange = BaseInstrument.GetBardRange(from, SkillName.Discordance);
                Map targetMap = targ.Map;

                if (targ is BaseMount && ((BaseMount)targ).Rider != null)
                {
                    Mobile rider = ((BaseMount)targ).Rider;

                    range = (int)rider.GetDistanceToSqrt(from);
                    targetMap = rider.Map;
                }

                if (from.Map != targetMap || range > maxRange)
                {
                    ends = true;
                }
            }

            if (ends && info.m_Ending && info.m_EndTime < DateTime.UtcNow)
            {
                End(info);
            }
            else
            {
                if (ends && !info.m_Ending)
                {
                    info.m_Ending = true;
                    info.m_EndTime = DateTime.UtcNow + TimeSpan.FromSeconds(15);
                }
                else if (!ends)
                {
                    info.m_Ending = false;
                    info.m_EndTime = DateTime.UtcNow;
                }

                targ.FixedEffect(0x376A, 1, 32, 38, 0);
            }
        }

        public class DiscordanceTarget : Target
        {
            private readonly BaseInstrument m_Instrument;

            public DiscordanceTarget(Mobile from, BaseInstrument inst)
                : base(BaseInstrument.GetBardRange(from, SkillName.Discordance), false, TargetFlags.None)
            {
                m_Instrument = inst;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                from.RevealingAction();
                from.NextSkillTime = Core.TickCount + 1000;

                if (!m_Instrument.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(1062488); // The instrument you are trying to play is no longer in your backpack!
                }
                else if (target is Mobile)
                {
                    Mobile targ = (Mobile)target;

                    if (
                        (targ is BaseCreature && (((BaseCreature)targ).BardImmune || !from.CanBeHarmful(targ, false)) &&
                         ((BaseCreature)targ).ControlMaster != from))
                    {
                        from.SendLocalizedMessage("Voce nao pode enfraquecer isto"); // A song of discord would have no effect on that.
                    }
                    else if (m_Table.Contains(targ)) //Already discorded
                    {
                        from.SendLocalizedMessage("Seu alvo ja esta fraco"); // Your target is already in discord.
                    }
                    else if (from.Player || (from is BaseCreature && ((BaseCreature)from).CanDiscord))
                    {
                        Shard.Debug("Disc");

                        from.Stam -= CUSTO_STAMINA;

                        double diff = m_Instrument.GetDifficultyFor(targ) - 10.0;
                        double music = from.Skills[SkillName.Musicianship].Value;

                        if (from is BaseCreature)
                            music = 120.0;

                        int masteryBonus = 0;

                        if (music > 100.0)
                        {
                            diff -= (music - 100.0) * 0.5;
                        }

                        if (from is PlayerMobile)
                        {
                            masteryBonus = Spells.SkillMasteries.BardSpell.GetMasteryBonus((PlayerMobile)from, SkillName.Discordance);
                        }

                        if (masteryBonus > 0)
                        {
                            diff -= (diff * ((double)masteryBonus / 100));
                        }

                        if (!BaseInstrument.CheckMusicianship(from))
                        {
                            from.SendLocalizedMessage("Voce tocou muito mal"); // You play poorly, and there is no effect.
                            m_Instrument.PlayInstrumentBadly(from);
                            m_Instrument.ConsumeUse(from);
                        }
                        else if (from.CheckTargetSkillMinMax(SkillName.Discordance, target, diff - 25.0, diff + 25.0))
                        {
                            from.OverheadMessage("* tocando *");
                            m_Instrument.PlayInstrumentWell(from);
                            m_Instrument.ConsumeUse(from);

                            from.NextSkillTime = Core.TickCount + (8000 - ((masteryBonus / 5) * 1000));
                            from.NextActionTime = Core.TickCount + 3000;

                            Timer.DelayCall(TimeSpan.FromSeconds(4 - from.Dex / 50), () =>
                            {
                                if (from == null || !from.Alive || from.Map == Map.Internal || !targ.Alive || targ.Map == Map.Internal)
                                    return;

                                int range = BaseInstrument.GetBardRange(from, SkillName.Discordance);
                                if (!from.InRange(targ.Location, range))
                                {
                                    from.SendMessage("Voce esta muito longe do alvo");
                                    return;
                                }

                                if (!from.InLOS(targ))
                                {
                                    from.SendMessage("A musica precisa estar direcionada ao alvo diretamente");
                                    return;
                                }

                                from.SendLocalizedMessage("Voce tocou bem, reduzindo as forcas do alvo"); // You play the song surpressing your targets strength
                             
                                ArrayList mods = new ArrayList();
                                int effect;
                                double scalar;

                                var mod = -5.0;
                                if (targ is PlayerMobile)
                                    mod = -10;
                                effect = (int)(from.Skills[SkillName.Discordance].Value / mod);
                                scalar = effect * 0.01;

                                mods.Add(new StatMod(StatType.Str, "DiscordanceStr", (int)(targ.RawStr * scalar), TimeSpan.Zero));
                                mods.Add(new StatMod(StatType.Int, "DiscordanceInt", (int)(targ.RawInt * scalar), TimeSpan.Zero));
                                mods.Add(new StatMod(StatType.Dex, "DiscordanceDex", (int)(targ.RawDex * scalar), TimeSpan.Zero));

                                from.MovingParticles(targ, m_Instrument.ItemID, 10, 0, false, false, 38, 9502, 0x374A, 0x204, 1, 1);
                                //Effects.SendMovingParticles(from, new Entity(Serial.Zero, new Point3D(from.X, from.Y, from.Z + 15), from.Map), m_Instrument.ItemID, 7, 0, false, true, 38, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                                //Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X, from.Y, from.Z + 15), from.Map), targ, m_Instrument.ItemID, 7, 0, false, true, 38, 0, 9502, 1, 0, (EffectLayer)255, 0x100);

                                if (!(targ is PlayerMobile))
                                {
                                    for (int i = 0; i < targ.Skills.Length; ++i)
                                    {
                                        if (targ.Skills[i].Value > 0)
                                        {
                                            mods.Add(new DefaultSkillMod((SkillName)i, true, Math.Max(100, targ.Skills[i].Value) * scalar));
                                        }
                                    }
                                }

                                DiscordanceInfo info = new DiscordanceInfo(from, targ, Math.Abs(effect), mods);
                                info.m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(2), ProcessDiscordance, info);

                                #region Bard Mastery Quest
                                if (from is PlayerMobile)
                                {
                                    BaseQuest quest = QuestHelper.GetQuest((PlayerMobile)from, typeof(WieldingTheSonicBladeQuest));

                                    if (quest != null)
                                    {
                                        foreach (BaseObjective objective in quest.Objectives)
                                            objective.Update(targ);
                                    }
                                }
                                #endregion

                                m_Table[targ] = info;

                                from.NextSkillTime = Core.TickCount + (8000 - ((masteryBonus / 5) * 1000));
                            });


                        }
                        else
                        {
                            if (from is BaseCreature && PetTrainingHelper.Enabled)
                                from.CheckSkillMult(SkillName.Discordance, 0, from.Skills[SkillName.Discordance].Cap);

                            from.SendLocalizedMessage("Voce falhou em reduzir as forcas do alvo"); // You fail to disrupt your target
                            m_Instrument.PlayInstrumentBadly(from);
                            m_Instrument.ConsumeUse(from);

                            from.NextSkillTime = Core.TickCount + 5000;
                        }
                    }
                    else
                    {
                        m_Instrument.PlayInstrumentBadly(from);
                    }
                }
                else
                {
                    from.SendLocalizedMessage("Uma musica nao vai fazer efeito nisto"); // A song of discord would have no effect on that.
                }
            }
        }

        public class DiscordanceInfo
        {
            public readonly Mobile m_From;
            public readonly Mobile m_Creature;
            public readonly int m_Effect;
            public readonly ArrayList m_Mods;
            public DateTime m_EndTime;
            public bool m_Ending;
            public Timer m_Timer;

            public DiscordanceInfo(Mobile from, Mobile creature, int effect, ArrayList mods)
            {
                m_From = from;
                m_Creature = creature;
                m_EndTime = DateTime.UtcNow;
                m_Ending = false;
                m_Effect = effect;
                m_Mods = mods;

                Apply();
            }

            public void Apply()
            {
                for (int i = 0; i < m_Mods.Count; ++i)
                {
                    object mod = m_Mods[i];

                    if (mod is ResistanceMod)
                    {
                        m_Creature.AddResistanceMod((ResistanceMod)mod);
                    }
                    else if (mod is StatMod)
                    {
                        m_Creature.AddStatMod((StatMod)mod);
                    }
                    else if (mod is SkillMod)
                    {
                        m_Creature.AddSkillMod((SkillMod)mod);
                    }
                }
            }

            public void Clear()
            {
                for (int i = 0; i < m_Mods.Count; ++i)
                {
                    object mod = m_Mods[i];

                    if (mod is ResistanceMod)
                    {
                        m_Creature.RemoveResistanceMod((ResistanceMod)mod);
                    }
                    else if (mod is StatMod)
                    {
                        m_Creature.RemoveStatMod(((StatMod)mod).Name);
                    }
                    else if (mod is SkillMod)
                    {
                        m_Creature.RemoveSkillMod((SkillMod)mod);
                    }
                }
            }
        }
    }
}
