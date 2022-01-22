using Server.Commands;
using Server.Fronteira.Tutorial.WispGuia;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System;

namespace Server.Ziden.Tutorial
{
    public class TutorialNoob
    {

        public static void Configure()
        {
            if (Shard.RP)
                return;

            EventSink.Login += OnLogin;
        }


        public static void Initialize()
        {
            CommandSystem.Register("dawisp", AccessLevel.Administrator, new CommandEventHandler(CMD));
        }

        public static void CMD(CommandEventArgs arg)
        {
            arg.Mobile.Target = new IT();
        }

        private class IT : Target
        {
            public IT(): base(10, false, TargetFlags.None)
            {

            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                var pl = targeted as PlayerMobile;
                if(pl!=null)
                {
                    Cria(pl);
                } else
                {
                    from.SendMessage("Escolha um player");
                }
            }
        }

        public static void OnLogin(LoginEventArgs e)
        {
            var player = e.Mobile as PlayerMobile;
            if (player == null)
                return;

            if(!player.Young && player.Wisp != null)
            {
                player.Wisp.Jogador = null;
                player.Wisp.Delete();                
                player.Wisp = null;
            } else
            {
                InicializaWisp(player);
            }
        }
        
        public static void InicializaWisp(PlayerMobile player)
        {
            if (player.RP)
                return;

            if (player.Wisp == null && player.Young && !player.RP)
            {
                if(player.PassoWispGuia != (int)PassoTutorial.FIM)
                    Cria(player);
            }
        }

        private static void Cria(PlayerMobile player)
        {
            var guia = new NovoWispGuia(player);
            player.Wisp = guia;
            guia.SetControlMaster(player);
            if (!guia.IsPetFriend(player))
                guia.AddPetFriend(player);
            guia.AIObject.EndPickTarget(player, player, OrderType.Follow);
            guia.AceitaOrdens = false;
            guia.OnAfterTame(player);
            if (!guia.Owners.Contains(player))
            {
                guia.Owners.Add(player);
            }
            guia.MoveToWorld(player.Location, player.Map);
            guia.Fala("Oi !! Eu sou uma fadinha, e vou te ajudar a iniciar no jogo ! :D");
            guia.Fala("Tem muuuuita coisa pra te mostrar, mas vamos com calma, ok ? Hi hi");
            guia.SetCooldown("passo", TimeSpan.FromSeconds(10));
        }
    }
}
