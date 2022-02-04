using Server.Bounds;
using Server.Misc;
using Server.Network;
using System;

namespace Server.Ziden
{
    class ClientBlocker
    {
        public static void Initialize()
        {
            EventSink.Login += OnLogin;
        }

        public static void OnLogin(LoginEventArgs e)
        {
            e.Mobile.DecideMusic(null, e.Mobile.Region);

            if(e.Mobile.Name == CharacterCreation.NICK_INVALIDO)
            {
                e.Mobile.SendGump(new AtualizaNick());
            }

            /*
            if(!Shard.SPHERE_STYLE)
            {
                Shard.Debug("Login Version: " + e.Mobile.NetState.Version);
                var major = e.Mobile.NetState.Version.Major;
                if (major < 10 && e.Mobile.AccessLevel == AccessLevel.Player)
                {
                    e.Mobile.SendMessage(38, "----------- ATENÇÃO -----------");
                    e.Mobile.SendMessage(38, "Você não está conectado no servidor com nosso client.");
                    e.Mobile.SendMessage(38, "Acesse www.ultimafronteirashard.com.br e baixe nosso launcher para jogar!");
                    e.Mobile.SendMessage(38, "Em caso de dúvidas solicite auxílio no Discord.");
                    e.Mobile.SendMessage(38, "Desconectando em breve...");
                    Timer.DelayCall(TimeSpan.FromSeconds(30), m => Kick(m), e.Mobile);
                }
                else
                {
                    //e.Mobile.SendMessage(78, "Você está conectado com o Client Ultima Rise :)");
                }
            }
            */
          
            e.Mobile.SendMessage("Bem vindo.");

            if (e.Mobile.AccessLevel >= AccessLevel.VIP)
                return;

            if(Online.GetOnlinePlayers() > Shard.MaxOnline)
            {
                e.Mobile.SendMessage(38, "----------- ATENÇÃO -----------");
                e.Mobile.SendMessage(38, "Estamos em um host provisório.");
                e.Mobile.SendMessage(38, "O shard está muito cheio e você será desconectado.");
                e.Mobile.SendMessage(38, "Desconectando em breve...");
                e.Mobile.SendMessage(38, "-------------------------------");
                Timer.DelayCall(TimeSpan.FromSeconds(30), m => Kick(m), e.Mobile);
            }
        }

        public static void Kick(Mobile m)
        {
            if (!m.NetState.IsNull())
            {
                m.NetState.Dispose();
            }
        }
    }
}
