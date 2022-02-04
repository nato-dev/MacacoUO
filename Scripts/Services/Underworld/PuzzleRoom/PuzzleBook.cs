using System;

namespace Server.Items
{
    public class PuzzleBook : BrownBook
    {
        public static readonly BookContent Content = new BookContent(
            "Instructions", "Sir Wilber",
            new BookPageInfo(
                "Viajante!",
                "Gostaria de convida-lo",
                "para um pequeno jogo :)"),
            new BookPageInfo(
                "Ve aquela chave ali ?",
                "Te dara accesso a sala do puzzle",
                "Apos pegar a chave, voce tera 30 minutos"),
            new BookPageInfo(
                "Se voce falhar, seu progresso se perdera !",
                "Voce precisa completar 2 dos 3 puzzles !"),
            new BookPageInfo(
                "Se conseguir, voce estara mais proximo dos tesouros perdidos"));
				
        [Constructable]
        public PuzzleBook() : base(false)
        {
            Movable = false;
            ItemID = 4030;
        }

        public PuzzleBook(Serial serial)
            : base(serial)
        {
        }

        public override BookContent DefaultContent { get { return Content; } }
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}
