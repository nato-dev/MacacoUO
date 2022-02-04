using System;
using Server;
using Server.Network;
using Server.Targeting;
using Server.Regions;

namespace Server.Items
{
    public class GoblinFloorTrap : BaseTrap, IRevealableItem
	{
		private Mobile m_Owner;
		
		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Owner { get { return m_Owner; } set { m_Owner = value; } }
		
        public override int LabelNumber { get { return 1113296; } } // Armed Floor Trap
        public bool CheckWhenHidden { get { return true; } }

		[Constructable]
		public GoblinFloorTrap() : this( null )
		{
		}
		
		[Constructable]
		public GoblinFloorTrap(Mobile from) : base( 0x4004 )
		{
			m_Owner = from;
            Visible = false;
		}

		public override bool PassivelyTriggered{ get{ return true; } }
		public override TimeSpan PassiveTriggerDelay{ get{ return TimeSpan.FromSeconds( 1.0 ); } }
		public override int PassiveTriggerRange{ get{ return 1; } }
		public override TimeSpan ResetDelay{ get{ return TimeSpan.FromSeconds( 1.0 ); } }

		public override void OnTrigger( Mobile from )
		{
            if (from.AccessLevel > AccessLevel.VIP || !from.Alive)
                return;
            
			if( m_Owner != null )
			{
				if( !m_Owner.CanBeHarmful( from ) || m_Owner == from )
					return;
					
				if( m_Owner.Guild != null && m_Owner.Guild == from.Guild )
					return;
			}

			from.SendSound(0x22B);
            from.SendMessage("Voce pisou em uma armadilha"); // You stepped onto a goblin trap!

            Spells.SpellHelper.Damage(TimeSpan.FromSeconds(0.30), from, from, Utility.RandomMinMax(50, 75), 100, 0, 0, 0, 0);
				
			if(m_Owner != null)
				from.DoHarmful(m_Owner);
					
			Visible = true;
			Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(Rehide_Callback));

            PublicOverheadMessage(Server.Network.MessageType.Regular, 0x65, false, "* ativou uma armadilha *"); // [Trapped]

            new Blood().MoveToWorld(from.Location, from.Map);
		}

        public virtual bool CheckReveal(Mobile m)
        {
            return m.CheckTargetSkillMinMax(SkillName.DetectHidden, this, 50.0, 100.0);
        }

        public virtual void OnRevealed(Mobile m)
        {
            Unhide();
        }

        public virtual bool CheckPassiveDetect(Mobile m)
        {
            if (Visible && 0.05 > Utility.RandomDouble())
            {
                if (m.NetState != null)
                {
                    Packet p = new MessageLocalized(this.Serial, this.ItemID, Network.MessageType.Regular, 0x65, 3, 500813, this.Name, String.Empty);
                    p.Acquire();
                    m.NetState.Send(p);
                    Packet.Release(p);

                    return true;
                }
            }

            return false;
        }

		public void Unhide()
		{
			Visible = true;
			
			Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(Rehide_Callback));
		}
		
		public void Rehide_Callback()
		{
			Visible = false;
		}

		public GoblinFloorTrap( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write(m_Owner);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_Owner = reader.ReadMobile();
		}
	}
	
	public class GoblinFloorTrapKit : Item
	{
		[Constructable]
		public GoblinFloorTrapKit() : base (16704)
		{
            Name = "Armadilha";
		}
		
		public override void OnDoubleClick(Mobile from)
		{
            Region r = from.Region;

			if(!IsChildOf(from.Backpack))
			{
                from.SendMessage("O item precisa estar em sua mochila"); // This item must be in your backpack.
			}
            else if (from.Skills[SkillName.Tinkering].Value < 80)
            {
                from.SendMessage("Voce nao tem habilidade suficiente para armar a armadilha (Tinker > 80)"); // You do not have enough skill to set the trap.
            }
            else if (from.Mounted || from.Flying)
            {
                from.SendMessage("Nao pode estar montado"); // You cannot set the trap while riding or flying.
            }
            else if (r is GuardedRegion && !((GuardedRegion)r).IsDisabled())
            {
                from.SendMessage("Se voce colocar uma armadilha aqui os guardas nao vao ter piedade de voce.");
            }
            else
            {
                from.Target = new InternalTarget(this);
            }
		}
		
		private class InternalTarget : Target
		{
			private GoblinFloorTrapKit m_Kit;
			
			public InternalTarget(GoblinFloorTrapKit kit) : base(-1, false, TargetFlags.None)
			{
				m_Kit = kit;
			}
			
			protected override void OnTarget(Mobile from, object targeted)
			{
				if(targeted is IPoint3D)
				{
					Point3D p = new Point3D((IPoint3D)targeted);
                    Region r = Region.Find(p, from.Map);

                    if (from.Skills[SkillName.Tinkering].Value < 80)
                    {
                        from.SendMessage("Voce nao tem habilidade suficiente para colocar a armadilha"); // You do not have enough skill to set the trap.
                    }
                    else if (from.Mounted || from.Flying)
                    {
                        from.SendMessage("Nao pode estar montado"); // You cannot set the trap while riding or flying.
                    }
                    else if (r is GuardedRegion && !((GuardedRegion)r).IsDisabled())
                    {
                        from.SendMessage("Aqui nao.");
                    }
                    if (from.InRange(p, 2))
                    {
                        GoblinFloorTrap trap = new GoblinFloorTrap(from);

                        trap.MoveToWorld(p, from.Map);
                        from.SendMessage("Voce colocou a armadilha cuidadosamente");  // You carefully arm the goblin trap.

                        m_Kit.Consume();
                    }
                    else
                        from.SendMessage("Muito longe"); // That is too far away.
				}
			}
		}
		
		public GoblinFloorTrapKit( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}

