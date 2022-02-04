using Server.Commands;
using Server.Guilds;
using System;
using System.Linq;

namespace Server.Ziden.GuerraTotal
{
    public class Comandos
    {
        public static void Initialize()
        {
            CommandSystem.Register("VerDivisao", AccessLevel.Administrator, VerDivisao);
        }

        public static void VerDivisao(CommandEventArgs e)
        {
            var m = e.Mobile;
            if (e.Arguments.Count() == 0)
            {
                m.SendMessage(38, "Use .verdivisao 1 por exemplo");
                return;
            }

            int divisao = Int32.Parse(e.Arguments[0]);

            var guildas = "";
            foreach(var guilda in Guerra.Dados.Participantes.Values)
            {
                if((int)guilda.Divisao == divisao)
                {
                    var g = Guild.Find(guilda.Id);
                    if(g!=null)
                    {
                        guildas += g.Abbreviation + " | ";
                    }
                }
            }
            m.SendMessage("Guildas nessa divisao:");
            m.SendMessage(guildas);
        } 
    }
}
