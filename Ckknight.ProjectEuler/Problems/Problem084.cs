using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(84,
        @"In the game, Monopoly, the standard board is set up in the following
        way:

        GO    A1    CC1    A2    T1    R1    B1    CH1    B2    B3    JAIL
        H2                                                            C1
        T2                                                            U1
        H1                                                            C2
        CH3                                                           C3
        R4                                                            R2
        G3                                                            D1
        CC3                                                           CC2
        G2                                                            D2
        G1                                                            D3
        G2J   F3    U2     F2    F1    R3    E3    E2     CH2   E1    FP

        A player starts on the GO square and adds the scores on two 6-sided
        dice to determine the number of squares they advance in a clockwise
        direction. Without any further rules we would expect to visit each
        square with equal probability: 2.5%. However, landing on G2J (Go To
        Jail), CC (community chest), and CH (chance) changes this distribution.

        In addition to G2J, and one card from each of CC and CH, that orders
        the player to go directly to jail, if a player rolls three consecutive
        doubles, they do not advance the result of their 3rd roll. Instead they
        proceed directly to jail.

        At the beginning of the game, the CC and CH cards are shuffled. When a
        player lands on CC or CH they take a card from the top of the
        respective pile and, after following the instructions, it is returned
        to the bottom of the pile. There are sixteen cards in each pile, but
        for the purpose of this problem we are only concerned with cards that
        order a movement; any instruction not concerned with movement will be
        ignored and the player will remain on the CC/CH square.

        * Community Chest (2/16 cards):
          1. Advance to GO
          2. Go to JAIL
        * Chance (10/16 cards):
          1. Advance to GO
          2. Go to JAIL
          3. Go to C1
          4. Go to E3
          5. Go to H2
          6. Go to R1
          7. Go to next R (railway company)
          8. Go to next R
          9. Go to next U (utility company)
          10. Go back 3 squares.
        
        The heart of this problem concerns the likelihood of visiting a
        particular square. That is, the probability of finishing at that square
        after a roll. For this reason it should be clear that, with the
        exception of G2J for which the probability of finishing on it is zero,
        the CH squares will have the lowest probabilities, as 5/8 request a
        movement to another square, and it is the final square that the player
        finishes at on each roll that we are interested in. We shall make no
        distinction between ""Just Visiting"" and being sent to JAIL, and we
        shall also ignore the rule about requiring a double to ""get out of
        jail"", assuming that they pay to get out on their next turn.

        By starting at GO and numbering the squares sequentially from 00 to 39
        we can concatenate these two-digit numbers to produce strings that
        correspond with sets of squares.

        Statistically it can be shown that the three most popular squares, in
        order, are JAIL (6.24%) = Square 10, E3 (3.18%) = Square 24, and
        GO (3.09%) = Square 00. So these three most popular squares can be
        listed with the six-digit modal string: 102400.

        If, instead of using two 6-sided dice, two 4-sided dice are used, find
        the six-digit modal string.")]
    public class Problem084 : BaseProblem
    {
        private const int NumTurnsPerGame = 100000;
        private const int NumGames = 100;
        private const int DieSize = 4;
        public override object CalculateResult()
        {
            return new Range(NumGames)
                .AsParallel()
                .Select(x =>
                {
                    Random random = MathUtilities.CreateRandom();
                    var communityChestCards = new[]
                        {
                            CommunityChestCard.AdvanceToGo,
                            CommunityChestCard.GoToJail,
                            CommunityChestCard.Other,
                            CommunityChestCard.Other,
                            CommunityChestCard.Other,
                            CommunityChestCard.Other,
                            CommunityChestCard.Other,
                            CommunityChestCard.Other,
                            CommunityChestCard.Other,
                            CommunityChestCard.Other,
                            CommunityChestCard.Other,
                            CommunityChestCard.Other,
                            CommunityChestCard.Other,
                            CommunityChestCard.Other,
                            CommunityChestCard.Other,
                            CommunityChestCard.Other,
                        }
                        .Shuffle(random)
                        .ToQueue();
                    var chanceCards = new[]
                        {
                            ChanceCard.AdvanceToGo,
                            ChanceCard.GoToJail,
                            ChanceCard.GoToC1,
                            ChanceCard.GoToE3,
                            ChanceCard.GoToH2,
                            ChanceCard.GoToRailroad1,
                            ChanceCard.GoToNextRailroad,
                            ChanceCard.GoToNextRailroad,
                            ChanceCard.GoToNextUtility,
                            ChanceCard.GoBackThreeSquares,
                            ChanceCard.Other,
                            ChanceCard.Other,
                            ChanceCard.Other,
                            ChanceCard.Other,
                            ChanceCard.Other,
                            ChanceCard.Other,
                        }
                        .Shuffle(random)
                        .ToQueue();
                    Square currentPosition = Square.Go;
                    Action refixPosition = () =>
                    {
                        while (currentPosition < Square.Go)
                        {
                            currentPosition += BoardSize;
                        }
                        while (currentPosition > Square.H2)
                        {
                            currentPosition -= BoardSize;
                        }
                    };
                    DefaultDictionary<Square, int> landings = new DefaultDictionary<Square, int>(s => 0);
                    for (int i = 0; i < NumTurnsPerGame; i++)
                    {
                        int numDoubles = 0;
                        int die1 = random.Next(1, 1 + DieSize);
                        int die2 = random.Next(1, 1 + DieSize);
                        if (die1 == die2)
                        {
                            numDoubles++;
                        }
                        else
                        {
                            numDoubles = 0;
                        }
                        if (numDoubles == 3)
                        {
                            numDoubles = 0;
                            currentPosition = Square.Jail;
                        }
                        else
                        {
                            currentPosition += die1 + die2;
                            refixPosition();

                        checkPosition:
                            if (currentPosition == Square.GoToJail)
                            {
                                currentPosition = Square.Jail;
                            }
                            else if (Chances.Contains(currentPosition))
                            {
                                ChanceCard card = chanceCards.Dequeue();
                                chanceCards.Enqueue(card);

                                switch (card)
                                {
                                    case ChanceCard.AdvanceToGo:
                                        currentPosition = Square.Go;
                                        break;
                                    case ChanceCard.GoToJail:
                                        currentPosition = Square.GoToJail;
                                        break;
                                    case ChanceCard.GoToC1:
                                        currentPosition = Square.C1;
                                        break;
                                    case ChanceCard.GoToE3:
                                        currentPosition = Square.E3;
                                        break;
                                    case ChanceCard.GoToH2:
                                        currentPosition = Square.H2;
                                        break;
                                    case ChanceCard.GoToRailroad1:
                                        currentPosition = Square.Railroad1;
                                        break;
                                    case ChanceCard.GoToNextRailroad:
                                        while (!Railroads.Contains(currentPosition))
                                        {
                                            currentPosition++;
                                            refixPosition();
                                        }
                                        break;
                                    case ChanceCard.GoToNextUtility:
                                        while (!Utilities.Contains(currentPosition))
                                        {
                                            currentPosition++;
                                            refixPosition();
                                        }
                                        break;
                                    case ChanceCard.GoBackThreeSquares:
                                        currentPosition -= 3;
                                        refixPosition();

                                        goto checkPosition; // I hate gotos, but it's the cleanest way in this case.
                                }
                            }
                            else if (CommunityChests.Contains(currentPosition))
                            {
                                CommunityChestCard card = communityChestCards.Dequeue();
                                communityChestCards.Enqueue(card);

                                switch (card)
                                {
                                    case CommunityChestCard.AdvanceToGo:
                                        currentPosition = Square.Go;
                                        break;
                                    case CommunityChestCard.GoToJail:
                                        currentPosition = Square.Jail;
                                        break;
                                }
                            }
                        }
                        landings[currentPosition]++;
                    }
                    return landings;
                })
                .Aggregate((a, b) => {
                    foreach (var pair in b)
                    {
                        if (!a.ContainsKey(pair.Key))
                        {
                            a[pair.Key] = 0;
                        }
                        a[pair.Key] += pair.Value;
                    }
                    return a;
                })
                .OrderByDescending(p => p.Value)
                .Select(p => p.Key)
                .Take(3)
                .Select(s => ((int)s).ToString("00"))
                .StringJoin(string.Empty);
        }

        enum CommunityChestCard
        {
            Other,
            AdvanceToGo,
            GoToJail,
        };

        enum ChanceCard
        {
            Other,
            AdvanceToGo,
            GoToJail,
            GoToC1,
            GoToE3,
            GoToH2,
            GoToRailroad1,
            GoToNextRailroad,
            GoToNextUtility,
            GoBackThreeSquares,
        };

        private const int BoardSize = 40;
        enum Square
        {
            Go,
            A1,
            CommunityChest1,
            A2,
            Tax1,
            Railroad1,
            B1,
            Chance1,
            B2,
            B3,
            Jail,
            C1,
            Utility1,
            C2,
            C3,
            Railroad2,
            D1,
            CommunityChest2,
            D2,
            D3,
            FreeParking,
            E1,
            Chance2,
            E2,
            E3,
            Railroad3,
            F1,
            F2,
            Utility2,
            F3,
            GoToJail,
            G1,
            G2,
            CommunityChest3,
            G3,
            Railroad4,
            Chance3,
            H1,
            Tax2,
            H2,
        }

        private static readonly HashSet<Square> CommunityChests = new HashSet<Square>
        {
            Square.CommunityChest1,
            Square.CommunityChest2,
            Square.CommunityChest3,
        };

        private static readonly HashSet<Square> Chances = new HashSet<Square>
        {
            Square.Chance1,
            Square.Chance2,
            Square.Chance3,
        };

        private static readonly HashSet<Square> Utilities = new HashSet<Square>
        {
            Square.Utility1,
            Square.Utility2,
        };

        private static readonly HashSet<Square> Railroads = new HashSet<Square>
        {
            Square.Railroad1,
            Square.Railroad2,
            Square.Railroad3,
            Square.Railroad4,
        };
    }
}
