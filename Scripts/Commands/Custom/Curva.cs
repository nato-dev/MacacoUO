using System;

namespace Server.Commands.Custom
{
    public class Curva
    {
        public static void Initialize()
        {
           //CommandSystem.Register("curva", AccessLevel.Administrator, CurvaCmg);
           //CommandSystem.Register("setcurva", AccessLevel.Administrator, SetCurva);
        }

        /*
        public static void SetCurva(CommandEventArgs t)
        {
            try
            {
                var n = t.GetDouble(0);
                if(n < 0 || n > 1)
                {
                    t.Mobile.SendMessage("A curva tem q ser um numero entre 0 e 1");
                    return;
                }
                Shard.CurvaDeBonus = n;
                t.Mobile.SendMessage("Curva de achatamento de combate setada para " + n);
            }
            catch (Exception e)
            {
                t.Mobile.SendMessage("A curva tem q ser um numero entre 0 e 1");
            }

        }

        public static void CurvaCmg(CommandEventArgs t)
        {
            t.Mobile.SendMessage("Curva atual: " + Shard.CurvaDeBonus);
            t.Mobile.SendMessage("Para alterar, use .setcurva <numbero>");
            t.Mobile.SendMessage("1 = Sem Altercao, 0 = todo mundo igual independente da skill");
        }
        */

    }
}
