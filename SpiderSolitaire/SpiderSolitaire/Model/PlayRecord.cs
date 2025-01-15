using System;
using System.Xml.Serialization;

using MvvmLight;

namespace SpiderSolitaire.Model
{
    [Serializable]
    public sealed class PlayRecord : ViewModelBase
    {
        public PlayRecord()
        {
            
        }

        /// <summary>
        /// 套牌
        /// </summary>
        public int Decks
        {
            get;
            set;
        }

        

        private int rounds;
        [XmlElement(ElementName = "rounds")]
        public int Rounds
        {
            get
            {
                return rounds;
            }
            set
            {
                rounds = value;
                RaisePropertyChanged();
            }
        }

        private int won;
        [XmlElement(ElementName = "won")]
        public int Won
        {
            get
            {
                return won;
            }
            set
            {
                won = value;
                RaisePropertyChanged();
            }
        }

        private int winningstreak;
        [XmlElement(ElementName = "winningstreak")]
        public int WinningStreak
        {
            get
            {
                return winningstreak;
            }
            set
            {
                winningstreak = value;
                RaisePropertyChanged();
            }
        }

        private int losingstreak;
        [XmlElement(ElementName = "losingstreak")]
        public int LosingStreak
        {
            get
            {
                return losingstreak;
            }
            set
            {
                losingstreak = value;
                RaisePropertyChanged();
            }
        }

        public void Reset()
        {

        }
    }
}
