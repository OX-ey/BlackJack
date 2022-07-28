using System;
using BlackJack.Engine;
using BlackJack.Engine.Classes;

namespace BlackJack.Interfaces
{
	public interface IPlayer
    {
        public Hand hand { get; set; }
        public string name { get; set; }
        public bool bj { get; set; }


    }
}

