﻿using BlackJack.Engine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Classes
{
    public class Player 
    {
        public Hand hand { get ; set; } = new Hand();
        public string name { get; set ; }
        public float balance { get ; set ; }
        public float bet { get; set; } = 0;

        public bool busted { get; set; } = false;
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
    }
}
