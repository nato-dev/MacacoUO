using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a sentinel spider corpse" )]
	public class SentinelSpider : BaseCreature
	{
		[Constructable]
		public SentinelSpider() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Sentinel spider";
			Body = 0x9d;
            Hue = 1141;
			BaseSoundID = 0x388;

			SetStr( 95, 100 );
			SetDex( 140, 145 );
			SetInt( 40, 45 );

			SetHits( 260, 265 );

			SetDamage( 15, 22 );

			SetDamageType( ResistanceType.Physical, 100 );
			
			SetResistance( ResistanceType.Physical, 45, 50 );
			SetResistance( ResistanceType.Fire, 30, 35 );
			SetResistance( ResistanceType.Cold, 30, 35 );
			SetResistance( ResistanceType.Poison, 70, 75 );
			SetResistance( ResistanceType.Energy, 30, 35 );

            SetSkill( SkillName.Anatomy, 85.0, 90.0 );
			SetSkill( SkillName.MagicResist, 88.5, 90.0 );
			SetSkill( SkillName.Tactics, 102.9, 105.0 );
			SetSkill( SkillName.Wrestling, 119.1, 120.0 );
            SetSkill( SkillName.Poisoning, 101.0, 102.0 );

			Fame = 775;
			Karma = -775;

			VirtualArmor = 28;

            SetWeaponAbility(WeaponAbility.ArmorIgnore);
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.LV2 );
			AddLoot( LootPack.LV1 );
		}

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

	        if (Controlled)
		        return;
			
			if (Utility.RandomDouble() < 0.03)            
                c.DropItem(new LuckyCoin());
        }

        public override void OnThink()
        {
            if (!this.IsCooldown("teia"))
            {
                this.SetCooldown("teia", TimeSpan.FromSeconds(3));
            }
            else
            {
                return;
            }
            if (this.Combatant != null && this.Combatant.InRange2D(this.Location, 9))
            {
                if (!this.IsCooldown("teiab"))
                {
                    this.SetCooldown("teiab", TimeSpan.FromSeconds(30));
                }
                else
                {
                    return;
                }

                if (!this.InLOS(this.Combatant))
                {
                    return;
                }
                this.PlayAngerSound();
                this.MovingParticles(this.Combatant, 0x10D3, 15, 0, false, false, 9502, 4019, 0x160);
                var m = this.Combatant as Mobile;
                Timer.DelayCall(TimeSpan.FromMilliseconds(400), () =>
                {
                    m.SendMessage("Voce foi preso por uma teia e nao consegue se soltar");
                    m.OverheadMessage("* Preso em uma teia *");
                    var teia = new Teia(m);
                    teia.MoveToWorld(m.Location, m.Map);
                    m.Freeze(TimeSpan.FromSeconds(6));
                    Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
                    {
                        teia.Delete();
                        m.Frozen = false;
                    });
                });
            }
        }

        public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Arachnid; } }

		public SentinelSpider( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

            if (version == 0)
            {
                Body = 0x9d;
                Hue = 1141;
            }

			if ( BaseSoundID == 387 )
				BaseSoundID = 0x388;
		}
	}
}
