using System;
using Server.Spells;

namespace Server.Items
{
    public class CureLevelInfo
    {
        private readonly Poison m_Poison;
        private readonly double m_Chance;
        public CureLevelInfo(Poison poison, double chance)
        {
 
            this.m_Poison = poison;
            this.m_Chance = chance;
        }

        public Poison Poison
        {
            get
            {
                return this.m_Poison;
            }
        }
        public double Chance
        {
            get
            {
                return this.m_Chance;
            }
        }
    }

    public abstract class BaseCurePotion : BasePotion
    {
        public BaseCurePotion(PotionEffect effect)
            : base(0xF07, effect)
        {
        }

        public BaseCurePotion(Serial serial)
            : base(serial)
        {
        }

        public abstract CureLevelInfo[] LevelInfo { get; }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public void DoCure(Mobile from)
        {
            bool cure = false;

            CureLevelInfo[] info = this.LevelInfo;

            if(info.Length > 0 && info[0].Poison == null)
            {
               
            }

            for (int i = 0; i < info.Length; ++i)
            {
                CureLevelInfo li = info[i];

                if(li.Poison == null)
                {
                    Shard.Debug("Lesser null ? "+(Poison.Lesser==null));
                    Shard.Debug("Li pison null chance " + li.Chance);
                } else
                {
                    Shard.Debug("Chance Cure: " + li.Chance + " Level " + li.Poison.RealLevel);
                }
             

              //if (li.Poison != null && from.Poison != null && li.Poison.RealLevel == from.Poison.RealLevel &&
                if (li.Poison != null && from.Poison != null && li.Poison.RealLevel == from.Poison.RealLevel &&
                    Scale(from, li.Chance) > Utility.RandomDouble())
                {
                    cure = true;
                    break;
                }
            }

            if (cure && from.CurePoison(from))
            {
                from.SendMessage("Voce foi curado do veneno"); // You feel cured of poison!

                from.FixedEffect(0x373A, 10, 15);
                from.PlaySound(0x1E0);
            }
            else if (!cure && from.Poisoned)
            {
                from.SendMessage("O veneno eh forte demais para este antidoto"); // That potion was not strong enough to cure your ailment!
            }
        }

        public override void Drink(Mobile from)
        {
            if (TransformationSpellHelper.UnderTransformation(from, typeof(Spells.Necromancy.VampiricEmbraceSpell)))
            {
                from.SendLocalizedMessage(1061652); // The garlic in the potion would surely kill you.
            }
            DoCure(from);
            PlayDrinkEffect(from);
            from.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
            from.PlaySound(0x1E0);
            Consume();

        }
    }
}
