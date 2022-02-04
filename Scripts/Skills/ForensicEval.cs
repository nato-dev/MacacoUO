using System;
using System.Linq;
using System.Text;
using Server.Engines.Harvest;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System.Collections.Generic;
using Server.Ziden.Traducao;
using Server.Gumps;
using Server.Fronteira.Recursos;
using Server.Network;

namespace Server.SkillHandlers
{
    public interface IForensicTarget
    {
        void OnForensicEval(Mobile m);
    }

    public class ForensicEvaluation
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.Forensics].Callback = new SkillUseCallback(OnUse);
        }


        public static bool TellHarvestInformation(Mobile from, HarvestSystem system, object target)
        {
            int tileID;
            Map map;
            Point3D loc;

            if (!system.GetHarvestDetails(from, null, target, out tileID, out map, out loc))
            {
                //from.SendMessage("Este local nao parece ter recursos");
                return false;
            }

            var definition = system.GetDefinition(tileID);

            if (definition == null)
            {
                Shard.Debug("Definition null");
                return false;
            }

            var bank = definition.GetBank(map, loc.X, loc.Y);
            bool available = (bank != null && bank.Current >= definition.ConsumedPerHarvest);

            if (bank == null)
            {
                //.SendMessage("Parece nao haver nenhum tipo de recurso ali");
                return false;
            }

            List<Type> harvestables = new List<Type>();

            if (bank.Vein.FallbackResource != null)
            {
                foreach (var r in bank.Vein.FallbackResource.Types)
                {
                    harvestables.Add(r);
                }
            }

            foreach (var r in bank.Vein.PrimaryResource.Types)
            {
                harvestables.Add(r);
            }

            var jaFoi = new HashSet<CraftResource>();

            var str = "Aqui voce poderia encontrar  ";
            foreach (var t in harvestables)
            {
                var cs = CraftResources.GetFromType(t);
                if (cs != CraftResource.None && !jaFoi.Contains(cs))
                {
                    str += cs.ToString() + "  ";
                    jaFoi.Add(cs);
                }
                else
                    str += Trads.GetNome(t) + "  ";
            }
            from.SendMessage(str);
            return true;
        }

        public static TimeSpan OnUse(Mobile m)
        {
            m.Target = new ForensicTarget();
            m.RevealingAction();
            if(m.QuestArrow != null)
            {
                m.QuestArrow.Stop();
                m.QuestArrow = null;
            }
            m.SendMessage("O que voce gostaria de investigar ? Selecione voce mesmo para tentar localizar recursos proximos"); // Show me the crime.

            return TimeSpan.FromSeconds(1.0);
        }

        public class RecursoGump : Gump
        {
            private readonly Mobile m_From;
            private readonly int m_Range;
            private readonly List<Recurso> m_List;
            public RecursoGump(Mobile from, List<Recurso> list)
                : base(20, 30)
            {
                this.m_From = from;
                this.m_List = list;



                this.AddPage(0);

                this.AddBackground(0, 0, 440, 155, 5054);

                this.AddBackground(10, 10, 420, 75, 2620);
                this.AddBackground(10, 85, 420, 45, 3000);

                if (Shard.DebugEnabled)
                    Shard.Debug("Vendo lista de recursos proximos " + list.Count);

                if (list.Count > 4)
                {
                    this.AddBackground(0, 155, 440, 155, 5054);

                    this.AddBackground(10, 165, 420, 75, 2620);
                    this.AddBackground(10, 240, 420, 45, 3000);

                    if (list.Count > 8)
                    {
                        this.AddBackground(0, 310, 440, 155, 5054);

                        this.AddBackground(10, 320, 420, 75, 2620);
                        this.AddBackground(10, 395, 420, 45, 3000);
                    }
                }

                for (int i = 0; i < list.Count && i < 12; ++i)
                {
                    Recurso m = list[i];

                    this.AddItem(20 + ((i % 4) * 100), 20 + ((i / 4) * 155), m.Folha != null ? 3671 : m.ItemID, CraftResources.GetHue(m.Resource));
                    this.AddButton(20 + ((i % 4) * 100), 130 + ((i / 4) * 155), 4005, 4007, i + 1, GumpButtonType.Reply, 0);

                    if (m.Name != null)
                        this.AddHtml(20 + ((i % 4) * 100), 90 + ((i / 4) * 155), 90, 40, m.Resource.ToString(), false, false);
                }
            }

            public override void OnResponse(NetState state, RelayInfo info)
            {
                int index = info.ButtonID - 1;

                if (index >= 0 && index < this.m_List.Count && index < 12)
                {
                    Recurso m = this.m_List[index];

                    this.m_From.OverheadMessage("* analisando *");

                    m_From.QuestArrow = new QuestArrow(m_From, m);
                    m_From.QuestArrow.Update();
                }
            }
        }

        public static void SpyClasseia(Mobile from)
        {
            if (from.QuestArrow != null)
            {
                from.QuestArrow.Stop();
                from.QuestArrow = null;
            }

            List<Recurso> recursos = new List<Recurso>();
            var sector = from.Map.GetSector(from);

            for (int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    var s = from.Map.InternalGetSector(sector.X + x, sector.Y + y);
                    if (Shard.DebugEnabled)
                        Shard.Debug("Vendo setor " + (s.X) + " " + (sector.Y + y));
                    recursos.AddRange(Recurso.RecursosNoSector(from.Map, s));
                }
            }
            from.SendGump(new GumpOpcoes("Procurando ?", (opt) =>
            {
                if (opt == 0)
                {
                    from.SendGump(new RecursoGump(from, recursos.Where(r => CraftResources.GetType(r.Resource) == CraftResourceType.Wood).ToList()));
                }
                else if (opt == 1)
                {
                    from.SendGump(new RecursoGump(from, recursos.Where(r => CraftResources.GetType(r.Resource) == CraftResourceType.Metal).ToList()));
                }
                else
                {
                    from.SendGump(new RecursoGump(from, recursos));
                }
            }, 0x14F5, 0, "Madeiras", "Minerios", "Tudo"));
        }

        public class ForensicTarget : Target
        {
            public ForensicTarget()
                : base(10, true, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object target)
            {
                double skill = from.Skills[SkillName.Forensics].Value;
                double minSkill = 30.0;

                from.PublicOverheadMessage(Network.MessageType.Emote, 0, false, "* Investigando *");

                if (target is Corpse)
                {
                    Shard.Debug("Corpse");
                    if (skill < minSkill)
                    {
                        from.SendMessage("Voce nao ve nada fora do comum"); //You notice nothing unusual.
                        return;
                    }

                    if (from.CheckTargetSkillMinMax(SkillName.Forensics, target, minSkill, 100))
                    {
                        Corpse c = (Corpse)target;

                        if (c.m_Forensicist != null)
                            from.SendLocalizedMessage(1042750, c.m_Forensicist); // The forensicist  ~1_NAME~ has already discovered that:
                        else
                            c.m_Forensicist = from.Name;

                        if (((Body)c.Amount).IsHuman)
                            from.SendMessage("Isto foi morto por " + (c.Killer == null ? "no one" : c.Killer.Name));//This person was killed by ~1_KILLER_NAME~

                        if (c.Looters.Count > 0)
                        {
                            StringBuilder sb = new StringBuilder();

                            for (int i = 0; i < c.Looters.Count; i++)
                            {
                                if (i > 0)
                                    sb.Append(", ");

                                sb.Append(((Mobile)c.Looters[i]).Name);
                            }

                            from.SendLocalizedMessage(1042752, sb.ToString());//This body has been distrubed by ~1_PLAYER_NAMES~
                        }
                        else
                        {
                            from.SendLocalizedMessage(501002);//The corpse has not be desecrated.
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(501001);//You cannot determain anything useful.
                    }
                }
                else if (target is Mobile)
                {
                    if(target==from)
                    {
                        if(!from.HasItem<Spyglass>())
                        {
                            from.SendMessage("Voce precisa de uma luneta para encontrar recursos pelos mapa. Inventores (Tinkers) podem produzir ou vende-las.");
                            return;
                        }
                        if(from.Skills.Forensics.Value < 80)
                        {
                            from.SendMessage("Voce precisa de pelo menos 80 forensics para isto");
                            return;
                        }
                        ForensicEvaluation.SpyClasseia(from);
                        return;
                    }
                    if (skill < 36.0)
                    {
                        from.SendLocalizedMessage(501003);//You notice nothing unusual.
                    }
                    else if (from.CheckTargetSkillMinMax(SkillName.Forensics, target, 36.0, 100.0))
                    {
                        if (target is PlayerMobile && ((PlayerMobile)target).NpcGuild == NpcGuild.ThievesGuild)
                        {
                            from.SendLocalizedMessage(501004);//That individual is a thief!
                        }
                        else
                        {
                            from.SendLocalizedMessage(501003);//You notice nothing unusual.
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(501001);//You cannot determain anything useful.
                    }
                }
                else if (target is ILockpickable)
                {
                    if (skill < 41.0)
                    {
                        from.SendLocalizedMessage("Nada estranho...."); //You notice nothing unusual.
                    }
                    else if (from.CheckTargetSkillMinMax(SkillName.Forensics, target, 41.0, 100.0))
                    {
                        ILockpickable p = (ILockpickable)target;
                        if (target is TrapableContainer)
                        {
                            var trap = (TrapableContainer)target;
                            from.SendMessage("Armadilha: "+ trap.TrapType.ToString());
                            from.SendMessage("Nivel Armadilha: " + trap.TrapLevel.ToString());
                            from.SendMessage("Poder Armadilha: " + trap.TrapPower.ToString());
                        }
                        if (p.Picker != null)
                        {
                            from.SendLocalizedMessage(1042749, p.Picker.Name);//This lock was opened by ~1_PICKER_NAME~
                        }
                        else
                        {
                            from.SendLocalizedMessage(501003);//You notice nothing unusual.
                            from.SendMessage("Skill lockpick: " + p.RequiredSkill);
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(501001);//You cannot determain anything useful.
                    }
                }
                else if (target is Item)
                {


                    Item item = (Item)target;

                    if (item is IForensicTarget)
                    {
                        ((IForensicTarget)item).OnForensicEval(from);
                    }
                    else if (skill < 41.0)
                    {
                        from.SendLocalizedMessage("Voce nao pode ver nada estranho");//You cannot determain anything useful.
                        return;
                    }

                    var honestySocket = item.GetSocket<HonestyItemSocket>();

                    if (honestySocket != null)
                    {
                        if (honestySocket.HonestyOwner == null)
                            Server.Services.Virtues.HonestyVirtue.AssignOwner(honestySocket);

                        if (from.CheckTargetSkillMinMax(SkillName.Forensics, target, 41.0, 100.0))
                        {
                            string region = honestySocket.HonestyRegion == null ? "an unknown place" : honestySocket.HonestyRegion;

                            if (from.Skills.Forensics.Value >= 61.0)
                            {
                                from.SendLocalizedMessage(1151521, String.Format("{0}\t{1}", honestySocket.HonestyOwner.Name, region)); // This item belongs to ~1_val~ who lives in ~2_val~.
                            }
                            else
                            {
                                from.SendLocalizedMessage(1151522, region); // You find seeds from a familiar plant stuck to the item which suggests that this item is from ~1_val~.
                            }
                        }
                    }
                }
                else // chao
                {
                    if (from.CheckTargetSkillMinMax(SkillName.Forensics, target, 0, 100))
                    {
                        if (!ForensicEvaluation.TellHarvestInformation(from, Lumberjacking.System, target))
                            if (!ForensicEvaluation.TellHarvestInformation(from, Mining.System, target))
                                from.SendMessage("Nao parece haver recursos neste local");
                    }
                }
            }
        }
    }
}
