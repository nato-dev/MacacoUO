#region References
using System;
using Server.Engines.XmlSpawner2;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Engines.Quests;
#endregion

namespace Server.SkillHandlers
{
    public class Peacemaking
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.Peacemaking].Callback = OnUse;
        }

        public static int CUSTO_STAMINA = 5;

        public static TimeSpan OnUse(Mobile m)
        {
            m.RevealingAction();

            if (m.Stam < CUSTO_STAMINA)
            {
                m.SendMessage("Voce esta muito cansado para tocar");
                return TimeSpan.FromSeconds(2.0);
            }

            BaseInstrument.PickInstrument(m, OnPickedInstrument);

            return TimeSpan.FromSeconds(5.0); // Cannot use another skill for 1 second
        }

        public static void OnPickedInstrument(Mobile from, BaseInstrument instrument)
        {
            from.RevealingAction();
            from.SendLocalizedMessage("Quem voce deseja acalmar ?"); // Whom do you wish to calm?
            from.Target = new InternalTarget(from, instrument);
            from.NextSkillTime = Core.TickCount + 21600000;
        }

        public static bool UnderEffects(Mobile m)
        {
            return m is BaseCreature && ((BaseCreature)m).BardPacified;
        }

        public class InternalTarget : Target
        {
            private readonly BaseInstrument m_Instrument;
            private bool m_SetSkillTime = true;

            public InternalTarget(Mobile from, BaseInstrument instrument)
                : base(BaseInstrument.GetBardRange(from, SkillName.Peacemaking), false, TargetFlags.None)
            {
                m_Instrument = instrument;
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (m_SetSkillTime)
                {
                    from.NextSkillTime = Core.TickCount;
                }
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();

                if (!(targeted is Mobile))
                {
                    Shard.Debug("N eh mobile oxe");
                    from.SendLocalizedMessage(1049528); // You cannot calm that!
                }
                else if (!m_Instrument.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(1062488); // The instrument you are trying to play is no longer in your backpack!
                }
                else
                {

                    from.Stam -= CUSTO_STAMINA;

                    m_SetSkillTime = false;

                    int masteryBonus = 0;

                    if (from is PlayerMobile)
                        masteryBonus = Spells.SkillMasteries.BardSpell.GetMasteryBonus((PlayerMobile)from, SkillName.Peacemaking);

                    if (targeted == from)
                    {
                        // Standard mode : reset combatants for everyone in the area
                        if (from.Player && !BaseInstrument.CheckMusicianship(from))
                        {
                            from.SendLocalizedMessage(500612); // You play poorly, and there is no effect.
                            m_Instrument.PlayInstrumentBadly(from);
                            m_Instrument.ConsumeUse(from);

                            from.NextSkillTime = Core.TickCount + (10000 - ((masteryBonus / 5) * 1000));
                        }
                        else if (!from.CheckSkillMult(SkillName.Peacemaking, 0.0, 120.0))
                        {
                            from.SendLocalizedMessage(500613); // You attempt to calm everyone, but fail.
                            m_Instrument.PlayInstrumentBadly(from);
                            m_Instrument.ConsumeUse(from);

                            from.NextSkillTime = Core.TickCount + (10000 - ((masteryBonus / 5) * 1000));
                        }
                        else
                        {
                            from.NextSkillTime = Core.TickCount + 10000;
                            from.NextActionTime = Core.TickCount + 3000;
                            m_Instrument.PlayInstrumentWell(from);
                            m_Instrument.ConsumeUse(from);
                            from.OverheadMessage("* tocando *");

                            Timer.DelayCall(TimeSpan.FromSeconds(3), () =>
                            {
                                if (from == null || !from.Alive || from.Map == Map.Internal)
                                    return;

                                int range = BaseInstrument.GetBardRange(from, SkillName.Peacemaking);
   
                                Map map = from.Map;

                                if (map != null)
                                {
                                    bool calmed = false;
                                    IPooledEnumerable eable = from.GetMobilesInRange(range);

                                    foreach (Mobile m in eable)
                                    {
                                        if (!(m is BaseCreature))
                                        {
                                            continue;
                                        }
                                        var bc = (BaseCreature)m;

                                        if (bc.Tribe == TribeType.MortoVivo)
                                        {
                                            Shard.Debug("Slayer: " + m_Instrument.Slayer);
                                            if (m_Instrument.Slayer != SlayerName.Undeads && m_Instrument.Slayer2 != SlayerName.Undeads)
                                            {
                                                bc.Say("* ... *");
                                                continue;
                                            }
                                        }

                                        if (bc.Uncalmable ||
                                            (bc.AreaPeaceImmune) || m == from || !from.CanBeHarmful(m, false))
                                        {
                                            continue;
                                        }

                                        calmed = true;

                                        m.SendLocalizedMessage(500616); // You hear lovely music, and forget to continue battling!
                                        m.Combatant = null;
                                        m.Warmode = false;

                                        if (bc.BardPacified)
                                        {
                                            bc.Pacify(from, DateTime.UtcNow + TimeSpan.FromSeconds(1.0));
                                        }
                                    }
                                    eable.Free();

                                    if (!calmed)
                                    {
                                        from.SendLocalizedMessage("Voce nao conseguiu acalmar a criatura"); // You play hypnotic music, but there is nothing in range for you to calm.
                                    }
                                    else
                                    {
                                        from.SendLocalizedMessage("Voce toca uma musica hipnotica parando a batalha"); // You play your hypnotic music, stopping the battle.
                                    }
                                }
                            });
                        }
                    }
                    else
                    {
                        from.OverheadMessage("* tocando *");

                        // Target mode : pacify a single target for a longer duration
                        Mobile targ = (Mobile)targeted;

                        if (!from.CanBeHarmful(targ, false))
                        {
                            from.SendLocalizedMessage(1049528);
                            m_SetSkillTime = true;
                        }
                        else if (targ is BaseCreature && ((BaseCreature)targ).Uncalmable)
                        {
                            from.SendLocalizedMessage("Voce nao tem chance de acalmar isto"); // You have no chance of calming that creature.
                            m_SetSkillTime = true;
                        }
                        else if (targ is BaseCreature && ((BaseCreature)targ).BardPacified)
                        {
                            var dif = (((BaseCreature)targ).BardEndTime - DateTime.Now).TotalSeconds;
                            from.SendLocalizedMessage("A criatura ja esta calma por mais "+((int)dif)+" segundos"); // That creature is already being calmed.
                            m_SetSkillTime = true;
                        }
                        else if (from.Player && !BaseInstrument.CheckMusicianship(from))
                        {
                            from.SendLocalizedMessage("Voce tocou muito mal..."); // You play poorly, and there is no effect.
                            from.NextSkillTime = Core.TickCount + 5000;
                            m_Instrument.PlayInstrumentBadly(from);
                            m_Instrument.ConsumeUse(from);
                        }
                        else
                        {
                            double diff = m_Instrument.GetDifficultyFor(targ) - 10.0;
                            double music = from.Skills[SkillName.Musicianship].Value;

                            if (music > 100.0)
                            {
                                diff -= (music - 100.0) * 0.5;
                            }

                            if (masteryBonus > 0)
                                diff -= (diff * ((double)masteryBonus / 100));

                            if (!from.CheckTargetSkillMinMax(SkillName.Peacemaking, targ, diff - 25.0, diff + 25.0))
                            {
                                from.SendLocalizedMessage("Voce falhou em acalmar a criatura"); // You attempt to calm your target, but fail.
                                m_Instrument.PlayInstrumentBadly(from);
                                m_Instrument.ConsumeUse(from);

                                from.NextSkillTime = Core.TickCount + (10000 - ((masteryBonus / 5) * 1000));
                            }
                            else
                            {
                                m_Instrument.PlayInstrumentWell(from);
                                m_Instrument.ConsumeUse(from);
                                from.NextSkillTime = Core.TickCount + 10000;
                                from.NextActionTime = Core.TickCount + 3000;

                                Timer.DelayCall(TimeSpan.FromSeconds(4.2 - from.Dex / 50), () =>
                                {
                                    if (from == null || !from.Alive || from.Map == Map.Internal || !targ.Alive || targ.Map == Map.Internal)
                                        return;

                                    int range = BaseInstrument.GetBardRange(from, SkillName.Peacemaking);
                                    if(!from.InRange(targ.Location, range))
                                    {
                                        from.SendMessage("Voce esta muito longe do alvo");
                                        return;
                                    }

                                    if(!from.InLOS(targ))
                                    {
                                        from.SendMessage("A musica precisa estar direcionada ao alvo diretamente");
                                        return;
                                    }

                                    from.NextSkillTime = Core.TickCount + (5000 - ((masteryBonus / 5) * 1000));

                                    if (targ is BaseCreature)
                                    {
                                        BaseCreature bc = (BaseCreature)targ;

                                        if (bc.Tribe == TribeType.MortoVivo)
                                        {
                                            if (m_Instrument.Slayer != SlayerName.Undeads && m_Instrument.Slayer2 != SlayerName.Undeads)
                                            {
                                                bc.Say("* ... *");
                                                from.SendMessage("A criatura morto viva ignora a musica. Talvez com um instrumento especial...");
                                                return;
                                            }
                                        }

                                        //Effects.SendMovingParticles(targ, new Entity(Serial.Zero, new Point3D(targ.X, targ.Y, targ.Z + 15), from.Map), m_Instrument.ItemID, 10, 10, false, false, 1154, 0, 9502, 1, 0, EffectLayer.Head, 1);
                                        from.MovingParticles(targ, m_Instrument.ItemID, 7, 0, false, false, 1152, 9502, 0x374A, 0x204, 1, 1);
                                        from.SendLocalizedMessage("Voce toca uma musica hipnotica acalmando a criatura"); // You play hypnotic music, calming your target.
                                        targ.FixedEffect(0x376A, 1, 32, 1154, 0);
                                        targ.Combatant = null;
                                        targ.Warmode = false;

                                        double seconds = 100 - (diff / 1.5);

                                        if (seconds > 120)
                                        {
                                            seconds = 120;
                                        }
                                        else if (seconds < 10)
                                        {
                                            seconds = 10;
                                        }

                                        bc.Pacify(from, DateTime.UtcNow + TimeSpan.FromSeconds(seconds));

                                        #region Bard Mastery Quest
                                        if (from is PlayerMobile)
                                        {
                                            BaseQuest quest = QuestHelper.GetQuest((PlayerMobile)from, typeof(TheBeaconOfHarmonyQuest));

                                            if (quest != null)
                                            {
                                                foreach (BaseObjective objective in quest.Objectives)
                                                    objective.Update(bc);
                                            }
                                        }
                                        #endregion
                                    }
                                    else // target is player
                                    {

                                        if (!from.CanBeHarmful(targ))
                                            return;

                                        from.DoHarmful(targ);

                                        if (Utility.Random(3) == 1)
                                        {
                                            targ.SendMessage("Voce resistiu ao efeito da musica");
                                            from.SendMessage("O alvo resistiu a musica");
                                            targ.PlaySound(0x1E6);
                                            targ.FixedEffect(0x42CF, 10, 5);
                                            return;
                                        }

                                        from.SendLocalizedMessage("Voce acalmcou o alvo"); // You play hypnotic music, calming your target.
                                                                                           //from.MovingParticles(targ, m_Instrument.ItemID, 15, 0, false, false, 0, 9502, 0x374A, 0x204, 1, 1);
                                        var danoBase = 1;
                                        var rng = 0;
                                        switch (m_Instrument.Resource)
                                        {
                                            case CraftResource.Carmesim:
                                            case CraftResource.Gelo:
                                                danoBase = 2;
                                                rng = 2;
                                                break;
                                            case CraftResource.Eucalipto:
                                            case CraftResource.Mogno:
                                                danoBase = 2;
                                                rng = 1;
                                                break;
                                            case CraftResource.Pinho:
                                            case CraftResource.Carvalho:
                                                danoBase = 2;
                                                rng = 1;
                                                break;
                                        }
                                        if (m_Instrument.Quality != ItemQuality.Exceptional)
                                        {
                                            if (rng > 0)
                                                rng -= 1;
                                        } 

                                        var ratio = (from.Skills.Peacemaking.Value + from.Skills.Musicianship.Value) / 200;
                                        var par = Utility.Random(danoBase, rng) * ratio;
                                        //Effects.SendMovingParticles(from, new Entity(Serial.Zero, new Point3D(from.X, from.Y, from.Z + 15), from.Map), m_Instrument.ItemID, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                                        from.MovingParticles(targ, m_Instrument.ItemID, 10, 0, false, false, 1152, 9502, 0x374A, 0x204, 1, 1);
                                        targ.SendLocalizedMessage("Voce escuta uma musica calmante e esquece de lutar"); // You hear lovely music, and forget to continue battling!
                                        targ.Combatant = null;
                                        targ.Warmode = false;
                                        targ.Paralyze(TimeSpan.FromSeconds(par));
                                        targ.OverheadMessage("* acalmado *");
                                        from.NextSkillTime = Core.TickCount + 10000;
                                    }
                                });  
                            }
                        }
                    }
                }
            }
        }
    }
}
