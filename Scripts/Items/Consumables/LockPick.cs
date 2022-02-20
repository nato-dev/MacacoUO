using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Regions;
using Server.Targeting;

namespace Server.Items
{
    public interface ILockpickable : IPoint2D
    {
        int LockLevel { get; set; }
        bool Locked { get; set; }
        Mobile Picker { get; set; }
        int MaxLockLevel { get; set; }
        int RequiredSkill { get; set; }
        void LockPick(Mobile from);
    }

    [FlipableAttribute(0x14fc, 0x14fb)]
    public class Lockpick : Item
    {
        public virtual bool IsSkeletonKey { get { return false; } }
        public virtual int SkillBonus { get { return 0; } }

        [Constructable]
        public Lockpick()
            : this(1)
        {
        }

        [Constructable]
        public Lockpick(int amount)
            : base(0x14FC)
        {
            Stackable = true;
            Amount = amount;
        }

        public Lockpick(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0 && Weight == 0.1)
                Weight = -1;
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendLocalizedMessage(502068); // What do you want to pick?
            from.Target = new InternalTarget(this);
        }

        public virtual void OnUse()
        {
        }

        protected virtual void BeginLockpick(Mobile from, ILockpickable item)
        {
            if (item.Locked)
            {
                from.PlaySound(0x241);

                if(from.Hidden)
                {
                    foreach (var m in from.FindMobilesInRange(from.Map, 20))
                    {
                        var bc = m as BaseCreature;
                        if (bc == null)
                            continue;

                        if(m.Skills.DetectHidden.Value > 50 || bc.IsSmart)
                        {
                            if(m.Map.LineOfSight(m, from))
                            {
                                from.RevealingAction();
                                from.SendMessage("Voce foi descoberto");
                                m.Combatant = from;
                                m.OverheadMessage("!");
                                return;
                            }
                        }
                    }
                }
               

                Timer.DelayCall(TimeSpan.FromMilliseconds(200.0), EndLockpick, new object[] { item, from });
            }
            else
            {
                // The door is not locked
                from.SendLocalizedMessage(502069); // This does not appear to be locked
            }
        }

        protected virtual void BrokeLockPickTest(Mobile from)
        {
            // When failed, a 25% chance to break the lockpick
            if (!IsSkeletonKey && Utility.Random(4) == 0)
            {
                // You broke the lockpick.
                from.SendMessage("Voce quebrou o lockpick");

                from.PlaySound(0x3A4);
                Consume();
            }
        }

        protected virtual void EndLockpick(object state)
        {
            object[] objs = (object[])state;
            ILockpickable lockpickable = objs[0] as ILockpickable;
            Mobile from = objs[1] as Mobile;

            Item item = (Item)lockpickable;

            if (!from.InRange(item.GetWorldLocation(), 1))
                return;

            if ((!(lockpickable is TreasureLevel3) && lockpickable.LockLevel == 0) || lockpickable.LockLevel == -255)
            {
                // LockLevel of 0 means that the door can't be picklocked
                // LockLevel of -255 means it's magic locked
                from.SendMessage("Estra tranca... parece ter algo diferente nela..."); // This lock cannot be picked by normal means
                return;
            }

            if (from.Skills[SkillName.Lockpicking].Value < lockpickable.RequiredSkill - SkillBonus)
            {
                /*
                // Do some training to gain skills
                from.CheckSkill( SkillName.Lockpicking, 0, lockpickable.LockLevel );*/
                // The LockLevel is higher thant the LockPicking of the player
                from.SendMessage("Voce precisa de pelo menos "+ lockpickable.RequiredSkill+" lockpicking para conseguir abrir isto"); // You don't see how that lock can be manipulated.
                return;
            }

            int maxlevel = lockpickable.MaxLockLevel;
            int minLevel = lockpickable.LockLevel;

            if (lockpickable is Skeletonkey)
            {
                minLevel -= SkillBonus;
                maxlevel -= SkillBonus; //regulars subtract the bonus from the max level
            }

            if (this is MasterSkeletonKey || from.CheckTargetSkillMinMax(SkillName.Lockpicking, lockpickable, minLevel, maxlevel))
            {
                // Success! Pick the lock!
                OnUse();

                from.SendMessage("Voce conseguiu destrancar o item"); // The lock quickly yields to your skill.
                from.PlaySound(0x4A);
                lockpickable.LockPick(from);

                if(lockpickable is BaseTreasureChestMod && from.Region is GuardedRegion)
                {
                    var deletes = new List<Item>();
                    foreach(var i in ((BaseTreasureChestMod)lockpickable).Items)
                    {
                        if (Utility.RandomBool() || Utility.RandomBool())
                            deletes.Add(i);
                        else if (i is Gold)
                        {
                            i.Amount /= 4;
                        }
                    }
                    foreach (var i in deletes)
                        i.Delete();
                }
            }
            else
            {
                // The player failed to pick the lock
                BrokeLockPickTest(from);
                from.SendMessage("Voce nao conseguiu destrancar o item");

                if (item is TreasureMapChest && ((Container)item).Items.Count > 0 && 0.25 > Utility.RandomDouble())
                {
                    Container cont = (Container)item;

                    Item toBreak = cont.Items[Utility.Random(cont.Items.Count)];

                    if (!(toBreak is Container))
                    {
                        toBreak.Delete();
                        Effects.PlaySound(item.Location, item.Map, 0x1DE);
                        from.SendMessage(0x20, "Voce escuta algo quebrando dentro do bau.");
                    }
                }
            }
        }

        private class InternalTarget : Target
        {
            private Lockpick m_Item;

            public InternalTarget(Lockpick item)
                : base(1, false, TargetFlags.None)
            {
                m_Item = item;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Item.Deleted)
                    return;

                if (targeted is ILockpickable)
                {
                    m_Item.BeginLockpick(from, (ILockpickable)targeted);
                }
                else
                {
                    from.SendLocalizedMessage("Voce nao pode destrancar isto"); // You can't unlock that!
                }
            }
        }
    }
}
