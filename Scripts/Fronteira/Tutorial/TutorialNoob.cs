using Server.Commands;
using Server.Fronteira.Tutorial.WispGuia;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
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
            } else if(player.Young && player.Wisp != null)
            {
                player.Wisp.MoveToWorld(player.Location, player.Map);
            } else if(player.Young && player.Wisp == null)
            {
                InicializaWisp(player);
            }
        }
        
        public static void InicializaWisp(PlayerMobile player)
        {
            if (player.RP)
                return;

            if ((player.Wisp == null || player.Wisp.Deleted) && player.Young && !player.RP)
            {
                if(player.PassoWispGuia != (int)PassoTutorial.FIM)
                    Cria(player);
            } else if(!player.Young && player.Profession==0)
            {
                player.SendGump(new NonRPClassGump());
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
            guia.SetCooldown("passo", TimeSpan.FromSeconds(10));

            if (player.PassoWispGuia == 0)
            {
                guia.Fala("Oi !! Eu sou uma fadinha, e vou te ajudar a iniciar no jogo ! :D");
                guia.Fala("Tem muuuuita coisa pra te mostrar, mas vamos com calma, ok ? Hi hi");

                foreach (var pl in NetState.GetOnlinePlayerMobiles())
                    if (pl != player)
                        pl.SendMessage(78, "[NOVO JOGADOR] " + player.Name + " chegou no shard");
            }
        }
    }
}
