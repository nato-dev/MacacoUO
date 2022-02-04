namespace Server.Items
{
    public enum MeadQuality
    {
        Low,
        Regular,
        Exceptional
    }

    public enum AleQuality
    {
        Low,
        Regular,
        Exceptional
    }

    public enum CiderQuality
    {
        Low,
        Regular,
        Exceptional
    }

    public enum TeaQuality
    {
        Low,
        Regular,
        Exceptional
    }

    public enum CoffeeQuality
    {
        Low,
        Regular,
        Exceptional
    }

    public enum CocoaQuality
    {
        Low,
        Regular,
        Exceptional
    }

    class BrewMsgs
    {
        public static string extremely = "Voce bebeu mas esta com muita sede ainda.";
        public static string satiated = "Voce bebeu e se sente mais satisfeito.";
        public static string less = "Depois de beber voce se sente muito melhor.";
        public static string full = "Voce esta cheio agora.";
        public static string defualtfull = "Voce conseguiu beber mas ta com maior barrigao de liquido!";
    }
}
