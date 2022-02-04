using System;
using System.Linq;
using Server.Engines.XmlSpawner2;
using Server.Mobiles;
using Server.Network;

namespace Server.Fronteira.QualidadeDefeitos.ListaDefeitos
{
    public class AmaldicoadoTimer : Timer
    {
        private const int range = 30;
        public AmaldicoadoTimer() : base( TimeSpan.FromSeconds( 5 ), TimeSpan.FromSeconds( 5 ) )
        {
            Priority = TimerPriority.OneSecond;
        }

        public static void Initialize()
        {
            // TODO: Esse codigo vai ser bem lagante. Netstate tb nao vai listar animais.
            // new AmaldicoadoTimer().Start();
        }

        protected override void OnTick()
        {
            foreach (NetState state in NetState.Instances)
            {
                //Procuro todos os mobiles do tipo BaseAnimal
                if (state != null && state.Mobile != null && state.Mobile is BaseAnimal)
                {
                    BaseAnimal animal = state.Mobile as BaseAnimal;

                    //Verifca no range X da BaseAnimal por todos os mobiles
                    foreach (Mobile m in animal.GetMobilesInRange(range))
                    {
                        if (m == animal || !animal.CanBeHarmful(m))
                            continue;

                        //Se estiver vivo e for player
                        if (m.Alive && m.Player && m.AccessLevel == AccessLevel.Player)
                        {
                            DefeitosXmlAttachment defeitosXmlAttachment =
                                XmlAttach.FindAttachment(m, typeof(DefeitosXmlAttachment)) as DefeitosXmlAttachment;
                            //Verifica pela existencia do defeito
                            if (defeitosXmlAttachment != null && defeitosXmlAttachment.Defeitos.TemDefeito(Defeito.Amaldicoado))
                            {
                                //Ataca o Player Amaldicoado
                                animal.Attack(m);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
