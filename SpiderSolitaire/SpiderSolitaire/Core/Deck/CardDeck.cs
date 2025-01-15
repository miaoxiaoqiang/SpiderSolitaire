using System;
using System.Collections.Generic;
using System.Linq;

using SpiderSolitaire.Core.PlayingCard;

namespace SpiderSolitaire.Core
{
    /// <summary>
    /// 一个牌堆
    /// </summary>
    internal sealed class CardDeck
    {
        public CardDeck()
        {
            DeckCards = new List<Card>();
        }

        public List<Card> DeckCards
        {
            get;
        }

        public void AddCards(IList<Card> arriver)
        {
            if(DeckCards == null)
            {
                return;
            }

            DeckCards.AddRange(arriver);
        }

        public void RemoveCards(int startindex, int count)
        {
            if (DeckCards == null || DeckCards.Count == 0)
            {
                return;
            }

            DeckCards.RemoveRange(startindex, count);
        }

        /// <summary>
        /// 获取指定纸牌的上一个纸牌
        /// </summary>
        /// <remarks>
        /// 上一个和下一个关系是相对于当前纸牌。顺序是K到A，K是最下层，A是最上层，数字小的纸牌在数字大的纸牌上方显示）
        /// </remarks>
        public Card GetPrevCard(Card card)
        {
            if(card == null)
            {
                throw new ArgumentNullException("空引用");
            }

            if(DeckCards == null || DeckCards.Count == 0)
            {
                return null;
            }

            int _index = DeckCards.IndexOf(card);

            if(_index <= 0)
            {
                return null;
            }
            else
            {
                //Card _card = DeckCards[_index - 1];
                //if(_card.CardStatus == Model.CardStatus.DealBack)
                //{
                //    return _card;
                //}

                return DeckCards[_index - 1];
            }
        }

        /// <summary>
        /// 判断能否移动指定纸牌
        /// </summary>
        /// <remarks>
        /// 如果当前牌堆中的指定纸牌位置到最下方位置之间的所有纸牌能够构成连续的数字（从大到小且花色相同），这些纸牌可以一起移动。
        /// </remarks>
        /// <param name="leaver">要移动的纸牌</param>
        /// <returns>
        /// 返回一个 <seealso cref="Tuple{T1, T2}"/> 二元组实例。<para/>
        /// T1 为 <seealso cref="bool"/> 类型，若为 <see langword="true"/> 则可移动；反之则不可移动<para/>
        /// T2 为 <seealso cref="IList{T}"/> 类型，T 为 <seealso cref="Card"/> 类型，表示可跟随一起移动的纸牌
        /// </returns>
        public Tuple<bool, IList<Card>> CanMoveCard(Card leaver)
        {
            if (DeckCards.Count == 0)
            {
                return Tuple.Create<bool, IList<Card>>(false, null);
            }

            if (leaver == DeckCards.Last())
            {
                return Tuple.Create<bool, IList<Card>>(true, null);
            }
            else
            {
                IList<Card> cards = new List<Card>();

                for (int i = leaver.IndexInDeck; i < DeckCards.Count - 1; i++)
                {
                    if ((int)DeckCards[i].CardNumber - (int)DeckCards[i + 1].CardNumber != 1
                        || DeckCards[i].Suit != DeckCards[i + 1].Suit) //数字差为1且花色相同
                    {
                        cards.Clear();
                        return Tuple.Create<bool, IList<Card>> (false, null);
                    }

                    cards.Add(DeckCards[i + 1]);
                }

                return Tuple.Create(true, cards);
            }
        }
    }
}
