using System;
using System.Collections.Generic;
using System.Linq;

using SpiderSolitaire.Core.PlayingCard;

namespace SpiderSolitaire.Core
{
    /// <summary>
    /// 牌堆所属区域基类（游戏区和已完成区）
    /// </summary>
    internal abstract class BaseZone
    {
        protected readonly IDictionary<int, CardDeck> _Decks;

        protected BaseZone(int deckCount)
        {
            _Decks = new Dictionary<int, CardDeck>();

            foreach (var item in Enumerable.Range(0, deckCount))
            {
                _Decks.Add(item, new CardDeck());
            }
        }

        /// <summary>
        /// 获取游戏区或已完成区的一个指定索引的牌堆
        /// </summary>
        /// <param name="deckIndex">游戏区域牌堆索引</param>
        public CardDeck GetCardDeck(int deckIndex)
        {
            if (!_Decks.ContainsKey(deckIndex))
            {
                throw new KeyNotFoundException("未能在集合找到指定键");
            }

            return _Decks[deckIndex];
        }

        /// <summary>
        /// 将一张纸牌添加到牌堆上
        /// </summary>
        public virtual void AddCard(Card card)
        {
            if (!_Decks.ContainsKey(card.GameDeckNumber))
            {
                throw new KeyNotFoundException("未能在集合找到指定键");
            }

            CardDeck deck = _Decks[card.GameDeckNumber];
            deck.DeckCards.Add(card);
            card.ZIndex = GetNewIndex(card.GameDeckNumber) + 1;
            card.IndexInDeck = GetNewIndex(card.GameDeckNumber);
        }

        /// <summary>
        /// 获取指定牌堆上的最新纸牌索引位置（顺序位置）
        /// </summary>
        /// <param name="deckIndex">牌堆索引</param>
        public virtual int GetNewIndex(int deckIndex)
        {
            if (!_Decks.ContainsKey(deckIndex))
            {
                throw new KeyNotFoundException("未能在集合找到指定键");
            }

            return _Decks[deckIndex].DeckCards.Count - 1;
        }

        /// <summary>
        /// 获取游戏区或已完成区的所有牌堆的纸牌总数量
        /// </summary>
        public virtual int GetAllCardsCount()
        {
            return _Decks.Sum(p => p.Value.DeckCards.Count);
        }

        /// <summary>
        /// 获取指定纸牌的上一个纸牌
        /// </summary>
        /// <remarks>
        /// 上一个和下一个关系是相对于当前纸牌。顺序是K到A，K是最下层，A是最上层，数字小的纸牌在数字大的纸牌上方显示）
        /// </remarks>
        /// <param name="card">移动的纸牌</param>
        public virtual Card GetPrevCard(Card card)
        {
            if (!_Decks.ContainsKey(card.GameDeckNumber))
            {
                throw new KeyNotFoundException("未能在集合找到指定键");
            }

            return _Decks[card.GameDeckNumber].GetPrevCard(card);
        }

        /// <summary>
        /// 从指定的牌堆移除指定的纸牌集合
        /// </summary>
        /// <param name="gamedeckindex">指定的牌堆索引</param>
        /// <param name="cards">纸牌集合</param>
        public virtual void RemoveCards(int gamedeckindex, IList<Card> cards)
        {
            if (!_Decks.ContainsKey(gamedeckindex))
            {
                throw new KeyNotFoundException("未能在集合找到指定键");
            }

            if (cards == null || cards.Count == 0)
            {
                return;
            }

            CardDeck deck = _Decks[gamedeckindex];
            deck.RemoveCards(cards.First().IndexInDeck, cards.Count);
        }

        /// <summary>
        /// 清空游戏区或已完成区的所有牌堆数据
        /// </summary>
        public virtual void Clear()
        {
            foreach (var item in _Decks)
            {
                item.Value.DeckCards.Clear();
            }
        }

        /// <summary>
        /// 获取纸牌在所属的牌堆上的索引位置（顺序位置）
        /// </summary>
        /// <param name="card">纸牌实例</param>
        /// <returns>
        /// 顺序位置
        /// </returns>
        public abstract int GetCardIndex(Card card);

        /// <summary>
        /// 将纸牌集合添加到游戏区指定牌堆上
        /// </summary>
        /// <param name="gamedeckindex">游戏区牌堆索引</param>
        /// <param name="cards">纸牌集合</param>
        public abstract void AddCards(int gamedeckindex, IList<Card> cards);
    }
}
