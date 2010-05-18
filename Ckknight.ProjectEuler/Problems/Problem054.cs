using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Ckknight.ProjectEuler.Collections;

namespace Ckknight.ProjectEuler.Problems
{
    [Problem(54,
        @"In the card game poker, a hand consists of five cards and are ranked, from lowest to highest, in the following way:

        High Card: Highest value card.
        One Pair: Two cards of the same value.
        Two Pairs: Two different pairs.
        Three of a Kind: Three cards of the same value.
        Straight: All cards are consecutive values.
        Flush: All cards of the same suit.
        Full House: Three of a kind and a pair.
        Four of a Kind: Four cards of the same value.
        Straight Flush: All cards are consecutive values of same suit.
        Royal Flush: Ten, Jack, Queen, King, Ace, in same suit.

        The cards are valued in the order:
        2, 3, 4, 5, 6, 7, 8, 9, 10, Jack, Queen, King, Ace.

        If two players have the same ranked hands then the rank made up of the
        highest value wins; for example, a pair of eights beats a pair of fives
        (see example 1 below). But if two ranks tie, for example, both players
        have a pair of queens, then highest cards in each hand are compared
        (see example 4 below); if the highest cards tie then the next highest
        cards are compared, and so on.

        Consider the following five hands dealt to two players:

        Hand          Player 1        Player 2        Winner
        1         5H 5C 6S 7S KD   2C 3S 8S 8D TD    Player 2
                  Pair of Fives    Pair of Eights

        2         5D 8C 9S JS AC   2C 5C 7D 8S QH    Player 1
                 Highest card Ace Highest card Queen

        3         2D 9C AS AH AC   3D 6D 7D TD QD    Player 2
                    Three Aces   Flush with Diamonds

        4         4D 6S 9H QH QC   3D 6D 7H QD QS    Player 1
                  Pair of Queens   Pair of Queens
                Highest card Nine Highest card Seven

        5         2H 2D 4C 4D 4S   3C 3D 3S 9S 9D    Player 1
                    Full House       Full House
                 With Three Fours  with Three Threes
        
        The file, poker.txt (http://projecteuler.net/project/poker.txt),
        contains one-thousand random hands dealt to two players. Each line of
        the file contains ten cards (separated by a single space): the first
        five are Player 1's cards and the last five are Player 2's cards. You
        can assume that all hands are valid (no invalid characters or repeated
        cards), each player's hand is in no specific order, and in each hand
        there is a clear winner.

        How many hands does Player 1 win?")]
    public class Problem054 : BaseProblem
    {
        public Problem054()
        {
            GetText();
        }

        public override object CalculateResult()
        {
            return GetText()
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line
                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(c => Card.Parse(c))
                    .ToArray())
                .Select(cards => new Match(
                    new Hand(cards.Take(5)),
                    new Hand(cards.Skip(5))))
                .Count(m => m.Player1 == m.Winner);
        }

        public class Card
        {
            public Card(CardValue value, CardSuit suit)
            {
                if (!Enum.IsDefined(typeof(CardValue), value))
                {
                    throw new ArgumentOutOfRangeException("value", value, "Must be defined");
                }
                else if (!Enum.IsDefined(typeof(CardSuit), suit))
                {
                    throw new ArgumentOutOfRangeException("suit", suit, "Must be defined");
                }

                _value = value;
                _suit = suit;
            }

            private readonly CardValue _value;
            private readonly CardSuit _suit;

            public CardValue Value
            {
                get
                {
                    return _value;
                }
            }

            public CardSuit Suit
            {
                get
                {
                    return _suit;
                }
            }

            public static Card Parse(string text)
            {
                if (text == null)
                {
                    throw new ArgumentNullException("data");
                }
                else if (text.Length != 2)
                {
                    throw new ArgumentException("Must be 2 characters", "text");
                }

                CardValue value;
                if (!_charToCardValue.TryGetValue(text[0], out value))
                {
                    throw new ArgumentException("Unknown CardValue", "text");
                }

                CardSuit suit;
                if (!_charToCardSuit.TryGetValue(text[1], out suit))
                {
                    throw new ArgumentException("Unknown CardSuit", "text");
                }

                return new Card(value, suit);
            }

            private static readonly Dictionary<char, CardValue> _charToCardValue = new Dictionary<char, CardValue>
            {
                { '2', CardValue.Two },
                { '3', CardValue.Three },
                { '4', CardValue.Four },
                { '5', CardValue.Five },
                { '6', CardValue.Six },
                { '7', CardValue.Seven },
                { '8', CardValue.Eight },
                { '9', CardValue.Nine },
                { 'T', CardValue.Ten },
                { 'J', CardValue.Jack },
                { 'Q', CardValue.Queen },
                { 'K', CardValue.King },
                { 'A', CardValue.Ace },
            };

            private static readonly Dictionary<char, CardSuit> _charToCardSuit = new Dictionary<char, CardSuit>
            {
                { 'H', CardSuit.Heart },
                { 'C', CardSuit.Club },
                { 'D', CardSuit.Diamond },
                { 'S', CardSuit.Spade },
            };

            public override string ToString()
            {
                return string.Format("{0} of {1}s", _value, _suit);
            }
        }

        public enum CardValue
        {
            Two = 2,
            Three = 3,
            Four = 5,
            Five = 7,
            Six = 11,
            Seven = 13,
            Eight = 17,
            Nine = 19,
            Ten = 23,
            Jack = 29,
            Queen = 31,
            King = 37,
            Ace = 41,
        }

        public enum CardSuit
        {
            Heart = 1,
            Club = 2,
            Diamond = 3,
            Spade = 4,
        }

        public class Hand : IComparable<Hand>
        {
            public Hand(IEnumerable<Card> cards)
            {
                if (cards == null)
                {
                    throw new ArgumentNullException("cards");
                }

                Card[] array = cards
                    .Where(c => c != null)
                    .OrderByDescending(c => c.Value)
                    .ThenBy(c => c.Suit)
                    .ToArray();

                if (array.Length != 5)
                {
                    throw new ArgumentException("Must contain 5 elements", "cards");
                }

                _cards = array;
            }

            private readonly Card[] _cards;
            public Card[] Cards
            {
                get
                {
                    return _cards;
                }
            }

            public override string ToString()
            {
                return string.Format("{{ {0} }}", string.Join(", ", (IEnumerable<Card>)_cards));
            }

            private Tuple<HandStatus, CardValue[]> _status;
            private Tuple<HandStatus, CardValue[]> Status
            {
                get
                {
                    if (_status == null)
                    {
                        CardValue[] values = _cards.Select(c => c.Value).OrderByDescending(v => v).ToArray();
                        if (IsStraight && IsFlush)
                        {
                            _status = Tuple.Create(values[0] == CardValue.Ace ? HandStatus.RoyalFlush : HandStatus.StraightFlush, values);
                            return _status;
                        }

                        Dictionary<CardValue, int> valueToCount = values
                            .Distinct()
                            .ToDictionary(v => v, v => values.Count(x => x == v));

                        CardValue popularValue = valueToCount
                            .Aggregate((a, b) => {
                                if (a.Value != b.Value)
                                {
                                    return a.Value > b.Value ? a : b;
                                }
                                else
                                {
                                    return a.Key > b.Key ? a : b;
                                }
                            })
                            .Key;
                        
                        CardValue secondPopularValue = valueToCount
                            .Where(p => p.Key != popularValue)
                            .Aggregate((a, b) => {
                                if (a.Value != b.Value)
                                {
                                    return a.Value > b.Value ? a : b;
                                }
                                else
                                {
                                    return a.Key > b.Key ? a : b;
                                }
                            })
                            .Key;

                        if (valueToCount.Count == 2) // Full house or Four-of-a-kind
                        {
                            switch (valueToCount[popularValue])
                            {
                                case 4:
                                    _status = Tuple.Create(HandStatus.FourOfAKind, new[] { popularValue, popularValue, popularValue, popularValue, secondPopularValue });
                                    break;
                                case 3:
                                    _status = Tuple.Create(HandStatus.FullHouse, new[] { popularValue, popularValue, popularValue, secondPopularValue, secondPopularValue });
                                    break;
                                default:
                                    throw new InvalidOperationException();
                            }

                            return _status;
                        }

                        if (IsFlush)
                        {
                            _status = Tuple.Create(HandStatus.Flush, values);
                            return _status;
                        }

                        if (IsStraight)
                        {
                            _status = Tuple.Create(HandStatus.Straight, values);
                            return _status;
                        }

                        if (valueToCount.Count == 3) // Three-of-a-kind or Two pair
                        {
                            CardValue thirdPopularValue = valueToCount
                                .Where(p => p.Key != popularValue && p.Key != secondPopularValue)
                                .Aggregate((a, b) =>
                                {
                                    if (a.Value != b.Value)
                                    {
                                        return a.Value > b.Value ? a : b;
                                    }
                                    else
                                    {
                                        return a.Key > b.Key ? a : b;
                                    }
                                })
                                .Key;

                            switch (valueToCount[popularValue])
                            {
                                case 3:
                                    _status = Tuple.Create(HandStatus.ThreeOfAKind, new[] { popularValue, popularValue, popularValue, secondPopularValue, thirdPopularValue });
                                    return _status;
                                case 2:
                                    _status = Tuple.Create(HandStatus.TwoPairs, new[] { popularValue, popularValue, secondPopularValue, secondPopularValue, thirdPopularValue });
                                    return _status;
                                default:
                                    throw new InvalidOperationException();
                            }
                        }

                        if (valueToCount.Count == 4) // One pair
                        {
                            _status = Tuple.Create(HandStatus.OnePair, new[] { popularValue, popularValue }.Concat(values.Where(v => v != popularValue).OrderByDescending(v => v)).ToArray());
                            return _status;
                        }

                        // High card
                        _status = Tuple.Create(HandStatus.HighCard, values);
                        return _status;
                    }
                    return _status;
                }
            }

            private static readonly HashSet<int> _possibleStraights = new[]
            {
                new[] { CardValue.Ace, CardValue.Two, CardValue.Three, CardValue.Four, CardValue.Five },
                new[] { CardValue.Two, CardValue.Three, CardValue.Four, CardValue.Five, CardValue.Six },
                new[] { CardValue.Three, CardValue.Four, CardValue.Five, CardValue.Six, CardValue.Seven },
                new[] { CardValue.Four, CardValue.Five, CardValue.Six, CardValue.Seven, CardValue.Eight },
                new[] { CardValue.Five, CardValue.Six, CardValue.Seven, CardValue.Eight, CardValue.Nine },
                new[] { CardValue.Six, CardValue.Seven, CardValue.Eight, CardValue.Nine, CardValue.Ten },
                new[] { CardValue.Seven, CardValue.Eight, CardValue.Nine, CardValue.Ten, CardValue.Jack },
                new[] { CardValue.Eight, CardValue.Nine, CardValue.Ten, CardValue.Jack, CardValue.Queen },
                new[] { CardValue.Nine, CardValue.Ten, CardValue.Jack, CardValue.Queen, CardValue.King },
                new[] { CardValue.Ten, CardValue.Jack, CardValue.Queen, CardValue.King, CardValue.Ace },
            }
                .Select(x => x
                    .Select(v => (int)v)
                    .Product())
                .ToHashSet();
            private bool? _isStraight;
            public bool IsStraight
            {
                get
                {
                    if (!_isStraight.HasValue)
                    {
                        _isStraight = _possibleStraights.Contains(_cards
                            .Select(c => (int)c.Value)
                            .Product());
                    }
                    return _isStraight.Value;
                }
            }

            private bool? _isFlush;
            public bool IsFlush
            {
                get
                {
                    if (!_isFlush.HasValue)
                    {
                        CardSuit suit = _cards[0].Suit;

                        _isFlush = _cards.Skip(1).All(c => c.Suit == suit);
                    }
                    return _isFlush.Value;
                }
            }

            #region IComparable<Hand> Members

            public int CompareTo(Hand other)
            {
                if (other == null)
                {
                    return 1;
                }
                else
                {
                    Tuple<HandStatus, CardValue[]> alpha = Status;
                    Tuple<HandStatus, CardValue[]> bravo = other.Status;

                    int cmp = alpha.Item1.CompareTo(bravo.Item1);
                    if (cmp != 0)
                    {
                        return cmp;
                    }

                    return alpha.Item2.SequenceCompare(bravo.Item2);
                }
            }

            #endregion
        }

        public enum HandStatus
        {
            HighCard = 1,
            OnePair = 2,
            TwoPairs = 3,
            ThreeOfAKind = 4,
            Straight = 5,
            Flush = 6,
            FullHouse = 7,
            FourOfAKind = 8,
            StraightFlush = 9,
            RoyalFlush = 10,
        }

        public class Match
        {
            public Match(Hand player1, Hand player2)
            {
                if (player1 == null)
                {
                    throw new ArgumentNullException("player1");
                }
                else if (player2 == null)
                {
                    throw new ArgumentNullException("player2");
                }

                _player1 = player1;
                _player2 = player2;
            }

            private readonly Hand _player1;
            private readonly Hand _player2;

            public Hand Player1
            {
                get
                {
                    return _player1;
                }
            }

            public Hand Player2
            {
                get
                {
                    return _player2;
                }
            }

            public Hand Winner
            {
                get
                {
                    return _player1.CompareTo(_player2) > 0 ? _player1 : _player2;
                }
            }
        }

        private readonly string Url = "http://projecteuler.net/project/poker.txt";
        private string _textCache;
        public string GetText()
        {
            if (_textCache == null)
            {
                WebClient client = new WebClient();
                _textCache = client.DownloadString(Url);
            }

            return _textCache;
        }
    }
}
