using Server.Mobiles;

namespace Server.Commands
{
    public class Murder
    {

        public static void Initialize()
        {
            CommandSystem.Register("Murder", AccessLevel.Player, Murder_OnCommand);
        }

        public static void Murder_OnCommand(CommandEventArgs t)
        {
            var pl = t.Mobile as PlayerMobile;
            t.Mobile.SendMessage(0x00FE, $"Assinatos recentes: (Shorts) { t.Mobile.ShortTermMurders }");
            t.Mobile.SendMessage(0x00FE, $"Assassinatos Em Aberto: (Longs) {t.Mobile.Kills}");
            if(t.Mobile.Kills > 0)
                t.Mobile.SendMessage(0x00FE, $"Proximo decay de long: { (pl.m_LongTermElapse - pl.GameTime).TotalHours} horas");
        }
    }
}
