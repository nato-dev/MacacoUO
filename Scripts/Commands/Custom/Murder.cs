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
            t.Mobile.SendMessage(0x00FE, "Assinatos recentes: (Shorts) {0}", t.Mobile.ShortTermMurders);
            t.Mobile.SendMessage(0x00FE, "Assassinatos Em Aberto: (Longs) {0}", t.Mobile.Kills);
        }
    }
}
