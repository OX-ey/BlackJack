using BlackJack.Engine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Interfaces;

namespace BlackJack.Classes
{
    public class Player : IPlayer
    {
        public Hand hand { get ; set; } = new Hand();
        public string name { get ; set; }
        public float balance { get ; set ; }
        public float bet { get; set; } = 0;
        public bool Busted => hand.Points > 21;
        public bool bj { get; set; } = false;
        public bool win { get; set; } = false;
        public bool push { get; set; } = false;
        public bool stand { get; set; } = false;
        public bool makeBet(float amount)
        {
            if (amount > 0 && balance > 0 && balance > amount)
            {
                balance -= amount;
                this.bet = amount;
                return true;
            }
            else { return false; }
        }
        public StringBuilder ShowCards()
        {
            StringBuilder mano = new StringBuilder();

            foreach (Card card in hand.Cards)
            {
                mano.AppendLine("");
                mano.Append(card.RealValue);
                mano.Append(" of ");
                mano.Append(card.Seed);
            }
            return mano;

        }
    }
}
