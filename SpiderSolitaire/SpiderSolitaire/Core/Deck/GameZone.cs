using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using SpiderSolitaire.Core.PlayingCard;

namespace SpiderSolitaire.Core
{
    /// <summary>
    /// 牌堆所属游戏区域
    /// </summary>
    internal sealed class GameZone : BaseZone
    {
        public GameZone() : base(10)
        {

        }

        /// <summary>
        /// 判断能否移动指定纸牌
        /// </summary>
        public Tuple<bool, IList<Card>> CanMove(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException("空引用");
            }

            if (!_Decks.ContainsKey(card.GameDeckNumber))
            {
                throw new KeyNotFoundException("未能在集合找到指定键");
            }

            return GetCardDeck(card.GameDeckNumber).CanMoveCard(card);
        }

        public override int GetCardIndex(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card) + " 是空引用。");
            }

            if (_Decks.ContainsKey(card.GameDeckNumber))
            {
                return _Decks[card.GameDeckNumber].DeckCards.IndexOf(card);
            }

            throw new KeyNotFoundException("未能在集合找到指定键");
        }

        public override void AddCards(int gamedeckindex, IList<Card> cards)
        {
            if (!_Decks.ContainsKey(gamedeckindex))
            {
                throw new KeyNotFoundException("未能在集合找到指定键");
            }

            foreach (var card in cards)
            {
                card.GameDeckNumber = gamedeckindex;
            }

            CardDeck carddeck = _Decks[gamedeckindex];
            carddeck.AddCards(cards);
        }

        /// <summary>
        /// 判断游戏区指定牌堆的纸牌能否构成一组牌（从K到A，数字递减）
        /// </summary>
        /// <param name="gamedeckindex">要检测的牌堆索引</param>
        public Tuple<bool, IList<Card>> CanMerge(int gamedeckindex)
        {
            if (!_Decks.ContainsKey(gamedeckindex))
            {
                throw new KeyNotFoundException("未能在集合找到指定键");
            }

            CardDeck carddeck = _Decks[gamedeckindex];
            if (carddeck.DeckCards.Count >= 13)
            {
                IList<Card> cards = carddeck.DeckCards.Skip(carddeck.DeckCards.Count - 13).ToList();
                bool result = cards.Zip(cards.Skip(1), (a, b) => (a.CardNumber - b.CardNumber, a.Suit == b.Suit)).All(diff => diff.Item1 == 1 && diff.Item2);
                if (result)
                {
                    return Tuple.Create(true, cards);
                }

                return Tuple.Create<bool, IList<Card>>(false, null);
            }

            return Tuple.Create<bool, IList<Card>>(false, null);
        }

        /// <summary>
        /// 判断移动的纸牌能否在游戏区域牌堆处释放
        /// </summary>
        /// <param name="card">移动的纸牌。如果有多个纸牌移动，则是最上方的纸牌</param>
        /// <param name="point">鼠标指针位置</param>
        public (bool CanDrop, int NewGameDeckIndex) CanDrop(Card card, Point point)
        {
            bool result = false;
            int _gamedeckindex = -1;

            foreach (var item in _Decks)
            {
                double _topx = CardOffestData.GetOffsetXInDeck(true, item.Key);
                double _topy = CardOffestData.GetOffsetYGameDeck(true, 0);

                Rect _rect = new(_topx, _topy, 117, CardOffestData.GetOffsetYGameDeck(true, item.Value.DeckCards.Count == 0 ? 3 : item.Value.DeckCards.Count - 1) + 50);

                if (((item.Value.DeckCards.Last().CardNumber - card.CardNumber == 1) || item.Value.DeckCards.Count == 0)
                    && (_rect.Contains(point) || card.GetCornersPoints().Any(p => _rect.Contains(p))))
                {
                    _gamedeckindex = item.Key;
                    result = true;
                    break;
                }
            }

            return (result, _gamedeckindex);
        }

        /// <summary>
        /// 获取指定牌堆上的最后一个背面纸牌
        /// </summary>
        /// <param name="gamedeckindex">牌堆索引</param>
        public Card GetLastBackCard(int gamedeckindex)
        {
            if (!_Decks.ContainsKey(gamedeckindex))
            {
                throw new KeyNotFoundException("未能在集合找到指定键");
            }

            return _Decks[gamedeckindex].DeckCards.FindLast(p => p.CardStatus == Model.CardStatus.DealBack);
        }
    }
}
