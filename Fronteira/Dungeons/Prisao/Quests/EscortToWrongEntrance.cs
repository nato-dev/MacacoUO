using System;
using Server.Items;

namespace Server.Engines.Quests
{
    public class EscortToWrongEntrance : BaseQuest
    {
        public EscortToWrongEntrance()
            : base()
        {
            this.AddObjective(new EscortObjective("Rhodes"));
            this.AddReward(new BaseReward("Compaixao"));
            this.AddReward(new BaseReward(typeof(Gold), 1000, "1000 Moedas de Ouro"));
        }

        /*We Who Are About To Die*/
        public override object Title
        {
            get
            {
                return "Estamos prestes para morrer";
            }
        }
        /*Take the prisoner to the entrance of Wrong Dungeon so they can escape.  The brigands will try to kill them,
         * so you must defend them.<br><center>-----</center><br>I don't know what to do, they took my weapons and 
         * broke my hands.  They follow these two necromancers and I think they intend to eat us as part of some 
         * dark ritual!  Will you help me?  Please?  Get me out of this prison, please....*/
        public override object Description
        {
            get
            {
                return "Leve o prisioneiro a entrada da dungeon";
            }
        }
        /*I am surely doomed....*/
        public override object Refuse
        {
            get
            {
                return "ok";
            }
        }
        /*I couldn't see when they brought me here, how much farther to the entrance of this horrid place?*/
        public override object Uncomplete
        {
            get
            {
                return "Nao consegui ver quando me trouxeram aqui. Como sair daqui ??";
            }
        }/*I am indebted to you, friend.  Please, take this as a token of my thanks.  I'm sorry it is all 
          * I have, they took everything else from me.*/
        public override object Complete
        {
            get
            {
                return "Voce eh uma boa pessoa. Aqui, pegue isto.";
            }
        }

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
    }
}
