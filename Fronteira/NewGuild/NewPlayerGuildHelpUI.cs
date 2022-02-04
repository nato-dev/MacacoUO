#region References

using Server.Guilds;
using Server.Mobiles;
using Server.Network;
using Server.Ziden.Tutorial;

#endregion

namespace Server.Gumps
{
    public class NewPlayerGuildHelpGump : Gump
    {
        public virtual int Sound { get { return 776; } }
        public virtual int Radius { get { return 10; } }

        public virtual int[] Stars
        {
            get { return new[] { 14170, 14155, 14138, 10980, 10296, 10297, 10298, 10299, 10300, 10301 }; }
        }

        public virtual int[] Hues { get { return new int[0]; } }
        private readonly Guild _Guild;

        public NewPlayerGuildHelpGump(Guild guild, Mobile m)
            : base(0, 0)
        {
            _Guild = guild;
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddBackground(27, 83, 424, 357, 9200);
            AddImage(127, 174, 1418);
            AddImageTiled(30, 85, 21, 349, 10464);
            AddImageTiled(426, 87, 21, 349, 10464);
            AddImage(417, 27, 10441);
            AddImage(205, 44, 9000);
            AddLabel(63, 158, 54, ("Parabens, voce entrou na guilda de iniciantes, " + m.Name));
            AddLabel(55, 185, 2047, "Uma coisas que voce devera saber:");
            AddLabel(55, 205, 2047, "-Quem cuida da guilda chama-se " + guild.Leader.Name);
            AddLabel(55, 225, 2047, "-Para falar no chat da guilda digite \\ depois sua mensagens");
            AddLabel(55, 250, 2047, "-Para falar no chat da alianca digite shift+\\ e sua mensagens");
            AddButton(55, 340, 247, 248, 1, GumpButtonType.Reply, 0);
            AddImageTiled(63, 176, 380, 1, 5410);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            var from = sender.Mobile as PlayerMobile;
            TutorialNoob.InicializaWisp(from);
            switch (info.ButtonID)
            {
                default:
                    {
                        if (_Guild != null)
                        {
                            int hue = Utility.RandomList(0x47E, 0x47F, 0x480, 0x482, 0x66D);
                            int renderMode = Utility.RandomList(0, 2, 3, 4, 5, 7);

                            from.SendMessage(54,
                                "Bem vindo a guilda dos iniciantes!  O atual lider da guilda eh {0}.",
                                _Guild.Leader.Name);
                            Effects.SendLocationEffect(from.Location, from.Map, 0x373A + (0x10 * Utility.Random(4)), 16, 10,
                                hue, renderMode);
                        }
                        break;
                    }
            }
        }
    }
}
