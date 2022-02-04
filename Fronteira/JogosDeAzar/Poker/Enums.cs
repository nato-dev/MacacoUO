// **********
// RunUO Shard - Enums.cs
// **********

namespace Server.Engines.TexasHoldem
{
    public enum PlayerAction
    {
        Fold=1,
        Check=2,
        Call=3,
        Bet=4,
        Raise=5,
        AllIn=6,
        None=0
    }

    public enum PokerGameState
    {
        Inactive,
        DealHoleCards,
        PreFlop,
        Flop,
        PreTurn,
        Turn,
        PreRiver,
        River,
        PreShowdown,
        Showdown,
        DetermineWinners,
        Intermission
    }

    public enum Suit
    {
        Ouros = 1,
        Copas = 2,
        Paus = 3,
        Espadas = 4
    }

    public enum Rank
    {
        Dois = 2,
        Tres = 3,
        Quatro = 4,
        Cinco = 5,
        Seis = 6,
        Sete = 7,
        Oito = 8,
        Nove = 9,
        Dez = 10,
        Valete = 11,
        Dama = 12,
        Rei = 13,
        As = 14
    }

    public enum HandRank
    {
        None,
        UmaDupla,
        DuasDuplas,
        Trinca,
        Straight,
        Flush,
        FullHouse,
        Quadra,
        StraightFlush,
        RoyalFlush
    }

    public enum RankResult
    {
        Melhor,
        Pior,
        Igual
    }
}
