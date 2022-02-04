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
    public class Provocation
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.Provocation].Callback = OnUse;
        }

        public static int CUSTO_STAMINA = 5;

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
            from.SendLocalizedMessage(501587); // Whom do you wish to incite?
            from.Target = new InternalFirstTarget(from, instrument);
        }

        public class InternalFirstTarget : Target
        {
            private readonly BaseInstrument m_Instrument;

            public InternalFirstTarget(Mobile from, BaseInstrument instrument)
                : base(BaseInstrument.GetBardRange(from, SkillName.Provocation), false, TargetFlags.None)
            {
                m_Instrument = instrument;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();

                if (targeted is BaseCreature && from.CanBeHarmful((Mobile)targeted, true))
                {
                    BaseCreature creature = (BaseCreature)targeted;

                    if (!m_Instrument.IsChildOf(from.Backpack))
                    {
                        from.SendLocalizedMessage(1062488); // The instrument you are trying to play is no longer in your backpack!
                    }
                    else if (from is PlayerMobile && creature.Controlled)
                    {
                        from.SendLocalizedMessage(501590); // They are too loyal to their master to be provoked.
                    }
                    else if (creature.IsParagon && BaseInstrument.GetBaseDifficulty(creature) >= 160.0)
                    {
                        from.SendLocalizedMessage(1049446); // You have no chance of provoking those creatures.
                    }
                    else
                    {
                        from.Stam -= CUSTO_STAMINA;

                        from.RevealingAction();
                        //m_Instrument.PlayInstrumentWell(from);
                        from.SendLocalizedMessage("Quem deseja atacar com a criatura?");
                        // You play your music and your target becomes angered.  Whom do you wish them to attack?
                        from.Target = new InternalSecondTarget(from, m_Instrument, creature);
                    }
                }
                else if(targeted is PlayerMobile)
                {
                    from.Stam -= CUSTO_STAMINA;

             
                    if(!from.CheckSkillMult(SkillName.Provocation, 0, 110, 1))
                    {
                        m_Instrument.PlayInstrumentBadly(from);
                        return;
                    }

                    from.OverheadMessage("* tocando *");

                    from.NextSkillTime = Core.TickCount + 5000;
                    from.NextActionTime = Core.TickCount + 3000;

                    Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
                    {
                        var alvo = targeted as PlayerMobile;
                        if (alvo != null && alvo != from && alvo.Alive && from.Alive)
                        {
                            if (!from.CanBeHarmful(alvo))
                                return;

                            int range = BaseInstrument.GetBardRange(from, SkillName.Peacemaking);
                            if (!from.InRange(alvo.Location, range))
                            {
                                from.SendMessage("Voce esta muito longe do alvo");
                                return;
                            }

                            if (!from.InLOS(alvo))
                            {
                                from.SendMessage("A musica precisa estar direcionada ao alvo diretamente");
                                return;
                            }

                            from.DoHarmful(alvo);

                            from.SendMessage("Voce toca uma musica provocadora contra o alvo, causando dano por deixar o alvo stressado");
                            var danoBase = 2;
                            var rng = 2;
                            switch (m_Instrument.Resource)
                            {
                                case CraftResource.Carmesim:
                                case CraftResource.Gelo:
                                    danoBase = 8;
                                    rng = 8;
                                    break;
                                case CraftResource.Eucalipto:
                                case CraftResource.Mogno:
                                    danoBase = 6;
                                    rng = 6;
                                    break;
                                case CraftResource.Pinho:
                                case CraftResource.Carvalho:
                                    danoBase = 4;
                                    rng = 4;
                                    break;
                            }
                            if (m_Instrument.Quality == ItemQuality.Exceptional)
                            {
                                danoBase += 2;
                                rng += 2;
                            }


                            var ratio = (from.Skills.Provocation.Value + from.Skills.Musicianship.Value) / 200;
                            var par = Utility.Random(danoBase, rng) * ratio;

                            m_Instrument.PlayInstrumentWell(from);
                            AOS.Damage(alvo, (int)par , DamageType.Spell);
                            from.MovingParticles(alvo, m_Instrument.ItemID, 7, 0, false, false, 38, 9502, 0x374A, 0x204, 1, 1);
                            from.NextSkillTime = Core.TickCount + 10000;
                            m_Instrument.ConsumeUse(from);
                            return;
                        }
                        from.SendLocalizedMessage(501589); // You can't incite that!
                    });

                    
                }
            }
        }

        public class InternalSecondTarget : Target
        {
            private readonly BaseCreature m_Creature;
            private readonly BaseInstrument m_Instrument;

            public InternalSecondTarget(Mobile from, BaseInstrument instrument, BaseCreature creature)
                : base(BaseInstrument.GetBardRange(from, SkillName.Provocation), false, TargetFlags.None)
            {
                m_Instrument = instrument;
                m_Creature = creature;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();

                if (targeted is PlayerMobile)
                {
                  
                    if (!from.CanBeHarmful(m_Creature, true))
                    {
                        from.SendMessage("Voce nao pode fazer mau a esta criatura");
                        return;
                    }

                    if (!from.CheckTargetSkillMinMax(SkillName.Provocation, (Mobile)targeted, 0, 120))
                    {
                        from.NextSkillTime = Core.TickCount + 5000;
                        from.SendLocalizedMessage(501599); // Your music fails to incite enough anger.
                        m_Instrument.PlayInstrumentBadly(from);
                        m_Instrument.ConsumeUse(from);
                    }
                    else
                    {
                        from.OverheadMessage("* chamando atencao *");
                        m_Creature.OverheadMessage("* ! *");
                        from.SendLocalizedMessage("Voce inicia uma briga"); // Your music succeeds, as you start a fight.
                        m_Instrument.PlayInstrumentWell(from);
                        m_Instrument.ConsumeUse(from);

                        from.MovingParticles(m_Creature, m_Instrument.ItemID, 7, 0, false, false, 38, 9502, 0x374A, 0x204, 1, 1);

                        m_Creature.Provoke(from, (Mobile)targeted, true);
                    }
                }

                if (targeted is BaseCreature || (from is BaseCreature && ((BaseCreature)from).CanProvoke))
                {
                    var creature = targeted as BaseCreature;
                    Mobile target = targeted as Mobile;

                    if (!m_Instrument.IsChildOf(from.Backpack))
                    {
                        from.SendLocalizedMessage(1062488); // The instrument you are trying to play is no longer in your backpack!
                    }
                    else if (m_Creature.Unprovokable)
                    {
                        from.SendLocalizedMessage(1049446); // You have no chance of provoking those creatures.
                    }
                    else if (creature != null && creature.Unprovokable)
                    {
                        from.SendLocalizedMessage(1049446); // You have no chance of provoking those creatures.
                    }
                    else if (m_Creature.Map != target.Map ||
                             !m_Creature.InRange(target, BaseInstrument.GetBardRange(from, SkillName.Provocation)))
                    {
                        from.SendLocalizedMessage(1049450);
                        // The creatures you are trying to provoke are too far away from each other for your music to have an effect.
                    }
                    else if (m_Creature != target)
                    {
                        from.NextSkillTime = Core.TickCount + 10000;

                        double diff = ((m_Instrument.GetDifficultyFor(m_Creature) + m_Instrument.GetDifficultyFor(target)) * 0.5) - 5.0;
                        double music = from.Skills[SkillName.Musicianship].Value;
                        int masteryBonus = 0;

                        if (from is PlayerMobile)
                            masteryBonus = Spells.SkillMasteries.BardSpell.GetMasteryBonus((PlayerMobile)from, SkillName.Provocation);

                        if (masteryBonus > 0)
                            diff -= (diff * ((double)masteryBonus / 100));

                        if (music > 100.0)
                        {
                            diff -= (music - 100.0) * 0.5;
                        }

                        if (from.CanBeHarmful(m_Creature, true) && from.CanBeHarmful(target, true))
                        {

                            if (from.Player && !BaseInstrument.CheckMusicianship(from))
                            {
                                from.NextSkillTime = Core.TickCount + (10000 - ((masteryBonus / 5) * 1000));
                                from.SendLocalizedMessage(500612); // You play poorly, and there is no effect.
                                m_Instrument.PlayInstrumentBadly(from);
                                m_Instrument.ConsumeUse(from);
                            }
                            else
                            {
                                //from.DoHarmful( m_Creature );
                                //from.DoHarmful( creature );
                                if (!from.CheckTargetSkillMinMax(SkillName.Provocation, target, diff - 25.0, diff + 25.0))
                                {
                                    from.NextSkillTime = Core.TickCount + (10000 - ((masteryBonus / 5) * 1000));
                                    from.SendLocalizedMessage(501599); // Your music fails to incite enough anger.
                                    m_Instrument.PlayInstrumentBadly(from);
                                    m_Instrument.ConsumeUse(from);
                                }
                                else
                                {
                                    from.OverheadMessage("* tocando *");

                                    Timer.DelayCall(TimeSpan.FromSeconds(4 - from.Dex / 50), () =>
                                    {
                                        if (from == null || from.Deleted || target.Deleted || !from.Alive || !target.Alive || target.Map == Map.Internal)
                                            return;

                                        int range = BaseInstrument.GetBardRange(from, SkillName.Provocation);
                                        if (!from.InRange(target.Location, range))
                                        {
                                            from.SendMessage("Voce esta muito longe do alvo");
                                            return;
                                        }

                                        if (!from.InLOS(target))
                                        {
                                            from.SendMessage("A musica precisa estar direcionada ao alvo diretamente");
                                            return;
                                        }

                                        from.SendLocalizedMessage(501602); // Your music succeeds, as you start a fight.
                                        m_Instrument.PlayInstrumentWell(from);
                                        m_Instrument.ConsumeUse(from);
                                        from.MovingParticles(m_Creature, m_Instrument.ItemID, 7, 0, false, false, 38, 9502, 0x374A, 0x204, 1, 1);
                                        m_Creature.Provoke(from, target, true);
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(501593); // You can't tell someone to attack themselves!
                    }
                }
                else
                {
                    from.SendLocalizedMessage(501589); // You can't incite that!
                }
            }

        }
    }
}
