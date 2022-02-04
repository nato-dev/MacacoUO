using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{


    public class Rota
    {
        public static void FalaComNpc(PlayerMobile from)
        {

        }
    }


    public class RotaDaSeda : BaseQuest
    {
        public override bool DoneOnce { get { return false; } }

        private string Cidade;

        public override object Title
        {
            get
            {
                return "Roda da Seda";
            }
        }
        public override object Description
        {
            get
            {
                return @"Ola viajante ! Estou precisando levar alguns recursos a outra cidade, poderia me ajudar ?
Voce tera de levar minhas mercadorias a outra cidade, mas nao podera usar teleportes ou entrar em portais, entao pode ser um pouco
perigoso... poderia me ajudar por favor ?";
            }
        }
        public override object Refuse
        {
            get
            {
                return "Ah uma pena";
            }
        }
        public override object Uncomplete
        {
            get
            {
                return "Voce ja entregou os recursos a cidade de "+Cidade+" ? ";
            }
        }
        public override object Complete
        {
            get
            {
                return "Muito obrigado por fazer esta entrega !";
            }
        }

        public RotaDaSeda()
            : base()
        {
            this.AddObjective(new InternalObjective());
        }

        private class InternalObjective : BaseObjective
        {
            public InternalObjective()
                : base()
            {
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

        public override void OnCompleted()
        {
            this.Owner.PlaySound(this.CompleteSound);
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
