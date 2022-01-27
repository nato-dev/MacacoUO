#region References
using System;
using Server.Fronteira.Talentos;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
#endregion

namespace Server.SkillHandlers
{
    public class Begging
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.Begging].Callback = OnUse;
        }

        public static TimeSpan OnUse(Mobile m)
        {
            m.RevealingAction();

            m.SendLocalizedMessage("A quem deseja pedir?"); // To whom do you wish to grovel?

            Timer.DelayCall(() => m.Target = new InternalTarget());

            return TimeSpan.FromSeconds(5.0);
        }

        private class InternalTarget : Target
        {
            private bool m_SetSkillTime = true;

            public InternalTarget()
                : base(12, false, TargetFlags.None)
            { }

            protected override void OnTargetFinish(Mobile from)
            {
              
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();

                string number = null;

                if (targeted is Mobile)
                {
                    Mobile targ = (Mobile)targeted;

                    if (targ.Player) // We can't beg from players
                    {
                        number = "Voce pode perguntar a esta pessoa..."; // Perhaps just asking would work better.
                    }
                    else if (!targ.Body.IsHuman) // Make sure the NPC is human
                    {
                        number = "Isto nao vai te dar dinheiro..."; // There is little chance of getting money from that!
                    }
                    else if(from.Mounted)
                    {
                        number = "Voce rico assim com montaria vem me pedir dinheiro ? Ah por favor...";
                    }
                    else if (!from.InRange(targ, 2))
                    {
                        if (!targ.Female)
                        {
                            number = "Voce esta muito longe"; // You are too far away to beg from him.
                        }
                        else
                        {
                            number = "Voce esta muito longe"; // You are too far away to beg from her.
                        }
                    }
                    else
                    {
                        // Face eachother
                        from.Direction = from.GetDirectionTo(targ);
                        targ.Direction = targ.GetDirectionTo(from);

                        from.Animate(32, 5, 1, true, false, 0); // Bow

                        new InternalTimer(from, targ).Start();

                        m_SetSkillTime = false;
                    }
                }
                else // Not a Mobile
                {
                    number = "Isto nao vai te dar esmola..."; // There is little chance of getting money from that!
                }

                if (number != null)
                {
                    from.SendLocalizedMessage(number);
                }
            }

            private class InternalTimer : Timer
            {
                private readonly PlayerMobile m_From;
                private readonly Mobile m_Target;

                public InternalTimer(Mobile from, Mobile target)
                    : base(TimeSpan.FromSeconds(2.0))
                {
                    m_From = from as PlayerMobile;
                    m_Target = target;
                    Priority = TimerPriority.TwoFiftyMS;
                }

                protected override void OnTick()
                {
                    Container theirPack = m_Target.Backpack;

                    double badKarmaChance = 0.35 - ((double)m_From.Karma / 8570);

                    if (theirPack == null && m_Target.Race != Race.Elf)
                    {
                        m_From.SendLocalizedMessage("Isto nao quis te dar dinheiro"); // They seem unwilling to give you any money.
                    }
                    else if (m_From.Karma < 0 && badKarmaChance > Utility.RandomDouble())
                    {
                        m_Target.PublicOverheadMessage(MessageType.Regular, m_Target.SpeechHue, true, "Voce nao me parece alguem de confianca, hoje nao.");
                        // Thou dost not look trustworthy... no gold for thee today!
                    }
                    else if (m_Target.IsCooldown("beg"+m_From.Name))
                    {
                        m_From.CheckTargetSkillMinMax(SkillName.Begging, m_Target, 0.0, 100.0);
                        m_Target.SayTo(m_From, "Ja doei dinheiro...");
                        var delay = TimeSpan.FromSeconds(10);
                        m_From.NextSkillTime = Core.TickCount + (int)delay.TotalMilliseconds / 2;
                    }
                    else if (m_From.CheckTargetSkillMinMax(SkillName.Begging, m_Target, 0.0, 100.0) || m_From.Talentos.Tem(Talento.Comandante))
                    {
                        m_Target.SetCooldown("beg" + m_From.Name, TimeSpan.FromHours(2));
                        if (m_Target is BaseHire)
                        {
                            if(m_From.Talentos.Tem(Talento.Comandante) || m_From.Skills[SkillName.Forensics].Value >= 100 && m_From.Skills[SkillName.Begging].Value >= 80)
                            {
                                var hired = (BaseHire)m_Target;
                                if (hired.Controlled || hired.IsHired)
                                {
                                    if (m_From.CheckTargetSkillMinMax(SkillName.Begging, m_Target, 80, 120.0))
                                    {
                                        hired.Say("Vou deixar meu mestre para lhe ajudar...");
                                    }
                                    else
                                    {
                                        hired.Say("Nao vou deixar meu mestre...");
                                        m_From.SendMessage("Voce falhou em persuadir o alvo");
                                        return;
                                    }
                                }
                                else
                                {
                                    hired.Say("Irei lhe ajudar, serei seu capanga !");
                                }
                                hired.Beg = true;
                                hired.AddHire(m_From);
                                return;
                            }
                            m_From.SendMessage(78, "Talvez com maestria na skill Forensics e Begging voce possa convencer essa pessoa a trabalhar pra voce.");
                        }

                        Item item = null;

                        if (m_From.Skills[SkillName.Begging].Value > 60)
                        {
                            if (m_Target is Mage || m_Target is Alchemist)
                            {
                                if (Utility.RandomDouble() < 0.25)
                                {
                                    item = new BagOfReagents(15);
                                }
                            }
                        } 
                        if (item == null)
                        {
                            var gold = Utility.Random(15) + 6;
                            item = new Gold(gold);
                        }
                        m_From.AddToBackpack(item);
                        m_Target.Say("Aqui, tome isto... espero que lhe ajude");
                        var delay = TimeSpan.FromSeconds(10);
                        m_From.NextSkillTime = Core.TickCount + (int)delay.TotalMilliseconds / 2;
                    }
                    else
                    {
                        var delay = TimeSpan.FromSeconds(10);
                        m_From.NextSkillTime = Core.TickCount + (int)delay.TotalMilliseconds / 2;
                        m_From.SendMessage("Voce nao conseguiu convence-lo a te dar nada.");
                    }
                }
            }
        }
    }
}
