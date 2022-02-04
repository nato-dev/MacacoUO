using System;

namespace Server.Items
{
    public class PieceWhiteKing : BasePiece
    {
        public PieceWhiteKing(BaseBoard board)
            : base(0x3587, board)
        {
        }

        public PieceWhiteKing(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "rei branco";
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class PieceBlackKing : BasePiece
    {
        public PieceBlackKing(BaseBoard board)
            : base(0x358E, board)
        {
        }

        public PieceBlackKing(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "rei preto";
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class PieceWhiteQueen : BasePiece
    {
        public PieceWhiteQueen(BaseBoard board)
            : base(0x358A, board)
        {
        }

        public PieceWhiteQueen(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "rainha branca";
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class PieceBlackQueen : BasePiece
    {
        public PieceBlackQueen(BaseBoard board)
            : base(0x3591, board)
        {
        }

        public PieceBlackQueen(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "rainha preta";
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class PieceWhiteRook : BasePiece
    {
        public PieceWhiteRook(BaseBoard board)
            : base(0x3586, board)
        {
        }

        public PieceWhiteRook(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "torre branca";
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class PieceBlackRook : BasePiece
    {
        public PieceBlackRook(BaseBoard board)
            : base(0x358D, board)
        {
        }

        public PieceBlackRook(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "torre preta";
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class PieceWhiteBishop : BasePiece
    {
        public PieceWhiteBishop(BaseBoard board)
            : base(0x3585, board)
        {
        }

        public PieceWhiteBishop(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "bispo branco";
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class PieceBlackBishop : BasePiece
    {
        public PieceBlackBishop(BaseBoard board)
            : base(0x358C, board)
        {
        }

        public PieceBlackBishop(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "bispo preto";
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class PieceWhiteKnight : BasePiece
    {
        public PieceWhiteKnight(BaseBoard board)
            : base(0x3588, board)
        {
        }

        public PieceWhiteKnight(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "cavalo branco";
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class PieceBlackKnight : BasePiece
    {
        public PieceBlackKnight(BaseBoard board)
            : base(0x358F, board)
        {
        }

        public PieceBlackKnight(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "cavalo preto";
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class PieceWhitePawn : BasePiece
    {
        public PieceWhitePawn(BaseBoard board)
            : base(0x3589, board)
        {
        }

        public PieceWhitePawn(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "peao branco";
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class PieceBlackPawn : BasePiece
    {
        public PieceBlackPawn(BaseBoard board)
            : base(0x3590, board)
        {
        }

        public PieceBlackPawn(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "peao preto";
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
