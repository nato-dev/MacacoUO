#region References
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Server.Commands.Generic;
using Server.Engines.BulkOrders;
using Server.Engines.Points;
using Server.Items;
using Server.Network;
using Server.Targeting;
#endregion

namespace Server.Commands
{
    public class DarPontosRP
    {
        public static void Initialize()
        {
            CommandSystem.Register("darpontosrp", AccessLevel.Administrator, OnAction);
        }

        [Usage("Action")]
        private static void OnAction(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Selecione um jogador para dar pontos RP numa area de 20x20 em volta dele");
            e.Mobile.Target = new IT();
        }

        public class IT : Target {
            public IT() : base(12, true, TargetFlags.None)
            {

            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if(targeted is IPoint3D)
                {
                  
                    var ponto = ((IPoint3D)targeted).ToPoint3D();
                    var galera = from.Map.GetClientsInRange(ponto, 25);
                    foreach(var mano in galera)
                    {
                        PointsSystem.PontosRP.AwardPoints(mano.Mobile, 1);
                        mano.Mobile.SendMessage(78, "Voce ganhou um Ponto RP por participar do RP !!");
                        mano.Mobile.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                    }
                    galera.Free();
                    from.SendMessage("Todos na area ganharam um ponto RP");
                } 
            }
        }
    }
}
