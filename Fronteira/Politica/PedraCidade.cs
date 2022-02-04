namespace Server.Ziden.Politica
{
    public class PedraCidade : Item
    {
        [CommandProperty(AccessLevel.Administrator)]
        public Mobile Governador { get; set; }
    }
}
