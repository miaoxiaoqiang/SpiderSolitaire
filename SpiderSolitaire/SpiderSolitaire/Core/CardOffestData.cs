using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SpiderSolitaire.Core
{
    /// <summary>
    /// 卡牌偏移量数据
    /// </summary>
    /// <remarks>
    /// 偏移量不一样！！！不知为什么会出现这种情况
    /// </remarks>
    public static class CardOffestData
    {
        private static readonly IReadOnlyDictionary<int, double> XOffsets;

        static CardOffestData()
        {
            XOffsets = new Dictionary<int, double>()
            {
                { 0, 11.5 },
                { 1, 148.5 },
                { 2, 285.5 },
                { 3, 422 },
                { 4, 559.5 },
                { 5, 696.5 },
                { 6, 833 },
                { 7, 970 },
                { 8, 1107.5 },
                { 9, 1244.5 }
            };
        }

        /// <summary>
        /// 发牌区卡牌之间的左右间距
        /// </summary>
        public static int CardSpaceInDealDeck
        {
            get { return 20; }
        }

        /// <summary>
        /// 游戏区卡牌之间的上下间距
        /// </summary>
        public static int CardSpaceInGameDeck
        {
            get { return 32; }
        }

        /// <summary>
        /// 获取卡牌需要X轴偏移的数值
        /// </summary>
        /// <param name="gameDeckOrFinishDeck">指示牌堆是属于哪个序列（游戏区序列或已完成序列）</param>
        /// <param name="deckIndex">牌堆索引</param>
        public static double GetOffsetXInDeck(bool gameDeckOrFinishDeck, int deckIndex)
        {
            if (gameDeckOrFinishDeck)
            {
                return XOffsets[deckIndex];
            }

            return XOffsets[deckIndex + 2];
        }

        /// <summary>
        /// 获取卡牌要在游戏区序列Y轴偏移的数值
        /// </summary>
        /// <remarks>
        /// 卡牌按照K到A顺序排列
        /// </remarks>
        /// <param name="cardIndex">卡牌在所属牌堆的位置索引</param>
        public static double GetOffsetYGameDeck(bool gameDeckOrFinishDeck, int cardIndex)
        {
            if (gameDeckOrFinishDeck)
            {
                return 176D + CardSpaceInGameDeck * cardIndex;
            }

            return 0.5D;
        }
    }
}
