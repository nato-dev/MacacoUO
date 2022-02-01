using Server.Network;
using Server.Commands;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
    public class SemElementoGump : Gump
    {
        public SemElementoGump() : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(289, 126, 642, 513, 9200);
            AddHtml(551, 130, 123, 20, @"Elementos PvM", (bool)false, (bool)false);
            AddBackground(299, 150, 620, 478, 3500);
            AddBackground(717, 346, 127, 101, 3500);
            AddBackground(712, 458, 138, 101, 3500);
            AddItem(760, 388, 3823, 0);
            AddHtml(715, 419, 131, 22, @"Ouro", (bool)true, (bool)false);
            AddHtml(715, 529, 133, 22, @"Cristal Elemental", (bool)true, (bool)false);
            AddButton(750, 592, 247, 248, (int)Buttons.Button4, GumpButtonType.Reply, 0);
            AddImage(238, 118, 10440);
            AddImage(897, 117, 10441);
            AddHtml(340, 168, 562, 18, @"Seu corpo ainda nao esta conectado com a energia deste mundo. ", (bool)false, (bool)false);
            AddHtml(340, 190, 562, 18, @"Voce eh apenas um fraco, ainda nao descobriu seu poder.", (bool)false, (bool)false);
            AddHtml(340, 213, 562, 24, @"Va a caverna de Shame e descubra...Mas esteja muito bem preparado.", (bool)false, (bool)false);
            AddItem(756, 494, 16395, 2611);
            AddHtml(772, 357, 50, 22, @"20K", (bool)false, (bool)false);
            AddHtml(761, 468, 47, 22, @"100", (bool)false, (bool)false);
            AddImage(341, 249, 1550);
            AddHtml(705, 571, 153, 19, @"Destravar Potencial", (bool)false, (bool)false);
            AddHtml(714, 287, 148, 77, @"Voce precisara dos seguinte items", (bool)false, (bool)false);
        }

        public enum Buttons
        {
            Nada,
            Button4,
        }

        //0xA725
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            var from = sender.Mobile as PlayerMobile;

            switch (info.ButtonID)
            {
                case (int)Buttons.Button4:
                    {
                        if(from.Skills.Total < 6000)
                        {
                            from.SendMessage("Voce precisa de pelo menos 600 pontos de skill para conseguir fazer isto...");
                            return;
                        }
                        if (!sender.Mobile.Backpack.HasItem<CristalElemental>(100, true))
                        {
                            from.SendMessage("Voce precisa de 100 Pedras Elementais na mochila e 20000 Moedas de Ouro no banco. Encontre as pedras em Shame.");
                            return;
                        }
                          
                        if (!Banker.Withdraw(from, 20000))
                        {
                            from.SendMessage("Voce precisa de 20000 Moedas de Ouro no banco.");
                            return;
                        }
                        var res = from.Backpack.ConsumeTotal(new System.Type[] { typeof(CristalElemental), typeof(Gold) }, new int[] { 100, 20000 });
                        if(res < 2)
                        {
                            from.SendMessage("Voce precisa de 100 Pedras Elementais na mochila e 20000 Moedas de Ouro no banco. Encontre as pedras em Shame.");
                            return;
                        }
                        ((PlayerMobile)from).Nivel = 2;

                        Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0, 0, 0, 0, 0, 5060, 0);
                        Effects.PlaySound(from.Location, from.Map, 0x243);

                        Effects.SendMovingParticles(new Entity(Server.Serial.Zero, new Point3D(from.X - 6, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                        Effects.SendMovingParticles(new Entity(Server.Serial.Zero, new Point3D(from.X - 4, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                        Effects.SendMovingParticles(new Entity(Server.Serial.Zero, new Point3D(from.X - 6, from.Y - 4, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);

                        Effects.SendTargetParticles(from, 0x375A, 35, 90, 0x00, 0x00, 9502, (EffectLayer)255, 0x100);

                        from.SendMessage("Voce agora pode canalizar energia elemental em seu corpo.");
                        from.SendMessage("Equipe armaduras elementais para ativar o elemento em seu corpo.");
                        from.SendMessage("Fabrique armaduras elementais usando pedras preciosas.");
                        foreach(var pl in NetState.GetOnlinePlayerMobiles())
                        {
                            if (pl != from)
                                pl.SendMessage(2, from.Name + " acaba de destravar o potencial dos elementos PvM");
                        }
                        from.PlaceInBackpack(new Amber());
                        from.SendGump(new ElementosGump(from));
                        break;
                    }

            }
        }
    }
}
