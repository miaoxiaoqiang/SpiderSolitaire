using System.ComponentModel;

namespace SpiderSolitaire.Model
{
    /// <summary>
    /// 纸牌花色
    /// </summary>
    public enum Suit
    {
        /// <summary>
        /// 黑桃
        /// </summary>
        [Description("黑桃")]
        Spade,
        /// <summary>
        /// 红桃
        /// </summary>
        [Description("红桃")]
        Heart,
        /// <summary>
        /// 梅花
        /// </summary>
        [Description("梅花")]
        Club,
        /// <summary>
        /// 方块
        /// </summary>
        [Description("方块")]
        Diamond,
    }
}
