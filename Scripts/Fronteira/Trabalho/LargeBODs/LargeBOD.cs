using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Ziden.Traducao;

namespace Server.Engines.BulkOrders
{
    [TypeAlias("Scripts.Engines.BulkOrders.LargeBOD")]
    public abstract class LargeBOD : BaseDecayingItem, IBOD
    {
        public abstract BODType BODType { get; }

        public static int BOD_MULTIPLIER = 2;

        public override int Lifespan { get { return 60*60*24*15; } }
        public override bool UseSeconds { get { return false; } }

        public bool giant = false;
        private int m_AmountMax;
        private bool m_RequireExceptional;
        private BulkMaterialType m_Material;
        private LargeBulkEntry[] m_Entries;

        public LargeBOD(int hue, int amountMax, bool requireExeptional, BulkMaterialType material, LargeBulkEntry[] entries)
            : base(0x2258)
        {
            Name = "Ordem de trabalho Grande";
            Weight = 1.0;
            Hue = hue; // Blacksmith: 0x44E; Tailoring: 0x483
            LootType = LootType.Blessed;

            m_AmountMax = amountMax * BOD_MULTIPLIER;
            m_RequireExceptional = requireExeptional;
            m_Material = material;
            m_Entries = entries;
        }

        public LargeBOD()
            : base(0x2258)
        {
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public LargeBOD(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int AmountMax
        {
            get
            {
                return m_AmountMax;
            }
            set
            {
                m_AmountMax = value * BOD_MULTIPLIER;
                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool RequireExceptional
        {
            get
            {
                return m_RequireExceptional;
            }
            set
            {
                m_RequireExceptional = value;
                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public BulkMaterialType Material
        {
            get
            {
                return m_Material;
            }
            set
            {
                m_Material = value;
                InvalidateProperties();
            }
        }
        public LargeBulkEntry[] Entries
        {
            get
            {
                return m_Entries;
            }
            set
            {
                m_Entries = value;
                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool Complete
        {
            get
            {
                for (int i = 0; i < m_Entries.Length; ++i)
                {
                    if (m_Entries[i].Amount < m_AmountMax)
                        return false;
                }

                return true;
            }
            set
            {
                if (value)
                {
                    for (int i = 0; i < m_Entries.Length; ++i)
                    {
                        m_Entries[i].Amount = m_AmountMax;
                    }
                }
            }
        }
        public override string DefaultName
        {
            get
            {
                return "Ordem de trabalho Grande";
            }
        }// a bulk order deed
        public static BulkMaterialType GetRandomMaterial(BulkMaterialType start, double[] chances)
        {
            double random = Utility.RandomDouble();

            for (int i = 0; i < chances.Length; ++i)
            {
                if (random < chances[i])
                    return (i == 0 ? BulkMaterialType.None : start + (i - 1));

                random -= chances[i];
            }

            return BulkMaterialType.None;
        }

        public abstract List<Item> ComputeRewards(bool full);

        public abstract int ComputeGold();

        public abstract int ComputeFame();

        public virtual void GetRewards(out Item reward, out int gold, out int fame)
        {
            reward = null;
            gold = ComputeGold();
            fame = ComputeFame();

            if (!BulkOrderSystem.NewSystemEnabled)
            {
                List<Item> rewards = ComputeRewards(false);

                if (rewards.Count > 0)
                {
                    reward = rewards[Utility.Random(rewards.Count)];

                    for (int i = 0; i < rewards.Count; ++i)
                    {
                        if (rewards[i] != reward)
                            rewards[i].Delete();
                    }
                }
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add("Ordem de trabalho grande"); // large bulk order

            base.GetProperties(list);

            if (m_RequireExceptional)
                list.Add("Apenas Excepcionais"); // All items must be exceptional.

            if (m_Material != BulkMaterialType.None)
                list.Add("Feito de "+m_Material); // All items must be made with x material.

            list.Add("Fazer "+ m_AmountMax.ToString() +" dos seguinte items:"); // amount to make: ~1_val~
           
            for (int i = 0; i < m_Entries.Length; ++i)
            {
                var nome = Trads.GetNome(m_Entries[i].Details.Type);
                if(nome == null)
                    list.Add(1060658 + i, "#{0}\t{1}", m_Entries[i].Details.Number, m_Entries[i].Amount); // ~1_val~: ~2_val~
                else
                    list.Add(m_Entries[i].Amount + " x " + nome);
            }
            //list.Add(1060658 + i, "#{0}\t{1}", m_Entries[i].Details.Number, m_Entries[i].Amount); // ~1_val~: ~2_val~
        }

        public override void OnDoubleClickNotAccessible(Mobile from)
        {
            OnDoubleClick(from);
        }

        public override void OnDoubleClickSecureTrade(Mobile from)
        {
            OnDoubleClick(from);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack) || InSecureTrade || RootParent is PlayerVendor)
			{
				EventSink.InvokeBODUsed(new BODUsedEventArgs(from, this));
				from.SendGump(new LargeBODGump(from, this));
                if (this.Complete)
                {
                    from.SendMessage(78, "Esta ordem esta completa. Arresta ela para o NPC que pediu a ordem para completa-la !");
                }
            }
			else
			{
				from.SendMessage("Voce precisa colocar isso na sua mochila"); // You must have the deed in your backpack to use it.
			}
		}

        public void BeginCombine(Mobile from)
        {
            if (!Complete)
            {
                from.Target = new LargeBODTarget(this);
                from.SendMessage("Selecione um item do tipo para adicionar");
            }
            else
                from.SendMessage("A ordem ja esta completa"); // The maximum amount of requested items have already been combined to this deed.
        }

        public void EndCombine(Mobile from, object o, bool initial = false)
        {
            if (o is Item && ((Item)o).IsChildOf(from.Backpack))
            {
                if(!giant)
                {
                    if (o is Item)
                    {
                        bool combinou = false;
                        Shard.Debug("Combine item");
                        var item = (Item)o;
                        BulkMaterialType material = BulkMaterialType.None;
                        if (o is IResource)
                            material = SmallBOD.GetMaterial(((IResource)o).Resource);

                        ItemQuality quality = ItemQuality.Normal;

                        if (o is IQuality)
                        {
                            quality = ((IQuality)item).Quality;
                        }

                        foreach(var entry in m_Entries)
                        {
                            Shard.Debug("Loop entry "+entry.Details.Type+" "+Trads.GetNome(entry.Details.Type));

                            if (item.GetType() != entry.Details.Type)
                            {
                                Shard.Debug("Tipo Diferente precisa "+entry.Details.Type);
                                //continue;
                            }
                            else if (m_RequireExceptional && quality != ItemQuality.Exceptional)
                            {
                                Shard.Debug("Precisava ser exp");
                                //continue; //from.SendMessage("Precisam ser excepcionais"); // Both orders must be of exceptional quality.
                            }
                            else if (material != m_Material && m_Material != BulkMaterialType.None)
                            {
                                Shard.Debug("Material diferente");
                                //continue; //from.SendMessage("Precisam ser do mesmo material"); // Both orders must use the same resource type.
                            }
                            else if (m_AmountMax <= entry.Amount)
                            {
                                Shard.Debug("Ja ta completa");
                                //continue; // The two orders have different requested amounts and cannot be combined.
                            } else
                            {

                                var toAdd = item.Amount;
                                var remaining = m_AmountMax - entry.Amount;


                                if (toAdd > remaining)
                                    toAdd = remaining;

                                combinou = true;
                                entry.Amount += toAdd;
                                item.Consume(toAdd);
                                if(initial)
                                {
                                    this.PrivateMessage("* trabalhou *", from);
                                    from.SendMessage("Voce adicionou os item a ordem de trabalho");
                                    var items = from.Backpack.FindItemsByType(item.GetType(), false);
                                    foreach(var it in items)
                                    {
                                        EndCombine(from, it, false);
                                    }
                                    from.SendGump(new LargeBODGump(from, this));
                                } 
                            }
                        }

                        if (!combinou && initial)
                        {
                            from.SendMessage("Este item nao atende aos requisitos da ordem.");
                        }

                    }
                    else
                    {
                        from.SendMessage("Selecione o item que deseja combinar.");
                    }
                } else
                {
                    if (o is SmallBOD)
                    {
                        SmallBOD small = (SmallBOD)o;

                        LargeBulkEntry entry = null;

                        for (int i = 0; entry == null && i < m_Entries.Length; ++i)
                        {
                            if (CheckType(small, m_Entries[i].Details.Type))
                            {
                                entry = m_Entries[i];

                            }
                        }

                        if (entry == null)
                        {
                            from.SendMessage("Essa ordem nao eh pra isso"); // That is not a bulk order for this large request.
                        }
                        else if (m_RequireExceptional && !small.RequireExceptional)
                        {
                            from.SendMessage("Precisam ser excepcionais"); // Both orders must be of exceptional quality.
                        }
                        else if (small.Material != m_Material && m_Material != BulkMaterialType.None)
                        {
                            from.SendMessage("Precisam ser do mesmo material"); // Both orders must use the same resource type.
                        }
                        else if (m_AmountMax != small.AmountMax)
                        {
                            from.SendMessage("Precisam ter pedido a mesma qtd de items"); // The two orders have different requested amounts and cannot be combined.
                        }
                        else if (small.AmountCur < small.AmountMax)
                        {
                            from.SendMessage("Uma das ordens nao esta completa"); // The order to combine with is not completed.
                        }
                        else if (entry.Amount >= m_AmountMax)
                        {
                            from.SendMessage("Ja foi combinada demais"); // The maximum amount of requested items have already been combined to this deed.
                        }
                        else
                        {
                            entry.Amount += small.AmountCur;
                            small.Delete();

                            from.SendMessage("Combinou as ordens"); // The orders have been combined.

                            from.SendGump(new LargeBODGump(from, this));

                            if (!Complete)
                                BeginCombine(from);
                        }
                    }
                    else
                    {
                        from.SendMessage("Isto nao eh uma ordem de compra. Combine com ordens de compra pequenas do tipo escolhido, voce ganhara pontos para todas ordens."); // That is not a bulk order.
                    }
                }
            }
            else
            {
                from.SendMessage("Nao esta em sua mochila"); // You must have the item in your backpack to target it.
            }
        }

        public virtual bool CheckType(SmallBOD small, Type type)
        {
            return small.CheckType(type);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)2); // version


            writer.Write(m_AmountMax);
            writer.Write(m_RequireExceptional);
            writer.Write((int)m_Material);

            writer.Write((int)m_Entries.Length);

            for (int i = 0; i < m_Entries.Length; ++i)
                m_Entries[i].Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version <= 1)
                return;

            switch ( version )
            {
                case 2:
                case 1:
                case 0:
                    {
                        m_AmountMax = reader.ReadInt();
                        m_RequireExceptional = reader.ReadBool();
                        m_Material = (BulkMaterialType)reader.ReadInt();

                        m_Entries = new LargeBulkEntry[reader.ReadInt()];

                        for (int i = 0; i < m_Entries.Length; ++i)
                            m_Entries[i] = new LargeBulkEntry(this, reader, version);
                        break;
                    }
            }

            if (Weight == 0.0)
                Weight = 1.0;

            if (Core.AOS && ItemID == 0x14EF)
                ItemID = 0x2258;

            if (Parent == null && Map == Map.Internal && Location == Point3D.Zero)
                Delete();

            if(version == 1)
            {
                m_AmountMax *= BOD_MULTIPLIER;
                //Delete();
            }
        }
    }
}
