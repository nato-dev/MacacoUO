namespace Server.Gumps
{
    public class QuestEditorGump : Server.Gumps.Gump
    {
        public QuestEditorGump() : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(144, 134, 225, 146, 9200);
            this.AddHtml(158, 145, 193, 19, @"Editor de Quests", (bool)true, (bool)false);
            this.AddButton(160, 183, 2472, 2472, (int)Buttons.NovaQuest, GumpButtonType.Reply, 0);
            this.AddHtml(191, 187, 116, 18, @"Nova Quest", (bool)false, (bool)false);
            this.AddButton(159, 226, 2472, 2472, (int)Buttons.ListarQuests, GumpButtonType.Reply, 0);
            this.AddHtml(191, 231, 116, 18, @"Listar Quests", (bool)false, (bool)false);
        }

        public enum Buttons
        {
            Nada,
            NovaQuest,
            ListarQuests,
        }



    }
}
