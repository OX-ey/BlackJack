using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Engine.Classes
{
    public class Card
    {
        public Seeds Seed { get; set; }
        public int Value { get; set; }
        public RealValues RealValue { get; set; }
    }
    public enum Seeds
    {
        hearts,
        diamonds,
        clubs,
        spades
    }
    public enum RealValues
    {
        king = 13,
        queen = 12,
        jack = 11,
        ace = 1
    }
}

