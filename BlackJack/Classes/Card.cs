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
        public int RealValue { get; set; }
    }
    public enum Seeds
    {
        hearts,
        diamonds,
        clubs,
        spades
    }
}
