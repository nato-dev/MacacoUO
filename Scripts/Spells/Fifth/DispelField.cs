using System;
using System.Collections.Generic;
using Server.Items;
using Server.Misc;
using Server.Targeting;

namespace Server.Spells.Fifth
{
    public class Field
    {
        public List<Item> items = new List<Item>();

        public void Add(Item item)
        {
            this.items.Add(item);
            DispelFieldSpell.fields[item.Serial] = this;
            Shard.Debug("Added field item num " + DispelFieldSpell.fields.Count);
        }

        /*
        public void Register()
        {
            Shard.Debug("Registering field items: " + items.Count);
            foreach (var item in items)
            {
                DispelFieldSpell.fields[item.Serial] = this;
            }
            Shard.Debug("REGISTERED FIELDS SIZE: " + DispelFieldSpell.fields.Count);
        }
        */
    }

    public class DispelFieldSpell : MagerySpell
    {
        public static Dictionary<Serial, Field> fields = new Dictionary<Serial, Field>();

        private static readonly SpellInfo m_Info = new SpellInfo(
            "Dispel Field", "An Grav",
            206,
            9002,
            Reagent.BlackPearl,
            Reagent.SpidersSilk,
            Reagent.SulfurousAsh,
            Reagent.Garlic);
        public DispelFieldSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override SpellCircle Circle
        {
            get
            {
                return SpellCircle.Fifth;
            }
        }
        public override void OnCast()
        {
            this.Caster.Target = new InternalTarget(this);
        }

        public bool Validate(Item item, bool msg = true)
        {
            Type t = item.GetType();
            if (!this.Caster.CanSee(item))
            {
                if (msg)
                    this.Caster.SendLocalizedMessage(500237); // Target can not be seen.
                return false;
            }
            else if (!t.IsDefined(typeof(DispellableFieldAttribute), false))
            {
                if (msg)
                    this.Caster.SendLocalizedMessage(1005049); // That cannot be dispelled.
                return false;
            }
            else if (item is Moongate && !((Moongate)item).Dispellable)
            {
                if (msg)
                    this.Caster.SendLocalizedMessage(1005047); // That magic is too chaotic
                return false;
            }
            return true;
        }

        public void Target(Item item)
        {
            Type t = item.GetType();

            if (Validate(item) && this.CheckSequence())
            {
                SpellHelper.Turn(this.Caster, item);

                if (fields.ContainsKey(item.Serial))
                {
                    var field = fields[item.Serial];
                    Shard.Debug("Found registered field with " + field.items.Count + " items");
                    foreach (var i in field.items)
                    {
                        if (i == null || i.Deleted || i.Map == Map.Internal)
                            continue;

                        Effects.SendLocationParticles(EffectItem.Create(i.Location, i.Map, EffectItem.DefaultDuration), 0x376A, 9, 20, 5042);
                        Effects.PlaySound(i.GetWorldLocation(), i.Map, 0x201);
                        i.Delete();
                    }
                }
                else
                {
                    Shard.Debug("FIELD NAO REGISTRADO RAPAIZ");
                    Effects.SendLocationParticles(EffectItem.Create(item.Location, item.Map, EffectItem.DefaultDuration), 0x376A, 9, 20, 5042);
                    Effects.PlaySound(item.GetWorldLocation(), item.Map, 0x201);
                    item.Delete();
                }
            }

        }

        public class InternalTarget : Target
        {
            private readonly DispelFieldSpell m_Owner;
            public InternalTarget(DispelFieldSpell owner)
                : base(Core.ML ? 10 : 12, true, TargetFlags.None)
            {
                this.m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Item)
                {
                    this.m_Owner.Target((Item)o);
                    this.m_Owner.FinishSequence();

                }
                else if (o is IPoint3D)
                {
                    var position = new Point3D((IPoint3D)o);
                    var items = from.Map.GetItemsInRange(position, 2);
                    var valids = new List<Item>();

                    foreach (var i in items)
                    {
                        if (m_Owner.Validate(i, false))
                        {
                            valids.Add(i);
                        }
                    }

                    items.Free();
                    if (valids.Count > 0)
                    {
                        var ms = 0;
                        var jaFoi = new List<Field>();
                        foreach (var i in valids)
                        {
                            if (fields.ContainsKey(i.Serial))
                            {
                                var field = fields[i.Serial];
                                if (!jaFoi.Contains(field))
                                {
                                    jaFoi.Add(field);
                                    this.m_Owner.Target(i);
                                    ms += 100;
                                }
                            }
                        }
                        this.m_Owner.FinishSequence();
                    }
                    else
                    {
                        this.m_Owner.Caster.SendMessage("Nenhum campo magico encontrado");
                    }

                }
                else
                {
                    this.m_Owner.Caster.SendMessage("Voce nao pode esconjurar isto"); // That cannot be dispelled.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                this.m_Owner.FinishSequence();
            }
        }
    }
}
