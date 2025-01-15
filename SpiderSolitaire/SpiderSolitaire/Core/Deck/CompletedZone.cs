using System;
using System.Collections.Generic;
using System.Linq;

using SpiderSolitaire.Core.PlayingCard;

namespace SpiderSolitaire.Core
{
    /// <summary>
    /// 牌堆所属已完成区域
    /// </summary>
    internal sealed class CompletedZone : BaseZone
    {
        public CompletedZone() : base(8)
        {

        }

        /// <summary>
        /// 获取匹配牌堆空序列的索引。如果有多个空序列，则返回第一个
        /// </summary>
        /// <returns>
        /// 若返回 -1，则没有空序列；若返回大于等于 0，则有空序列，返回值就是牌堆空序列的索引。
        /// </returns>
        public int GetEmptyDeck()
        {
            for (int i = 0; i < _Decks.Count; i++)
            {
                var deck = _Decks[i];

                if(deck.DeckCards.Count == 0)
                {
                    return i;
                }
            }

            return -1;
        }

        public override int GetCardIndex(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card) + " 是空引用。");
            }

            if (_Decks.ContainsKey(card.CompletedDeckNumber))
            {
                return _Decks[card.CompletedDeckNumber].DeckCards.IndexOf(card);
            }

            throw new KeyNotFoundException("未能在集合找到指定键");
        }

        public override void AddCards(int gamedeckindex, IList<Card> cards)
        {
            if(!_Decks.ContainsKey(gamedeckindex))
            {
                throw new KeyNotFoundException("未能在集合找到指定键");
            }

            if(_Decks[gamedeckindex].DeckCards.Count > 0)
            {
                throw new InvalidOperationException("指定序列已有元素");
            }

            foreach (var card in cards)
            {
                card.CompletedDeckNumber = gamedeckindex;
                _Decks[gamedeckindex].DeckCards.Add(card);
            }
        }
    }
}
