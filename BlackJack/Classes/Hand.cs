using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Engine.Classes
{
    public class Hand
    {
        public List<Card> Cards = new List<Card>();
        public int Points => Cards.Sum(x => x.Value);
    }
}
