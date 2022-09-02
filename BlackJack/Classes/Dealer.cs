using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Engine.Classes;
using BlackJack.Interfaces;

namespace BlackJack.Classes
{
    public class Dealer : IPlayer
    {
        public Hand hand { get; set; }= new Hand();
        public string name { get; set; } = "Dealer";
        public bool Busted => hand.Points > 21;
        public bool bj { get; set; } = false;
        public StringBuilder ShowCards()
        {
            StringBuilder mano = new StringBuilder();

            foreach (Card card in hand.Cards)
            {
                mano.AppendLine("\n");
                mano.Append(card.RealValue);
                mano.Append(" of ");
                mano.Append(card.Seed);
            }
            return mano;

        }
    }
}
