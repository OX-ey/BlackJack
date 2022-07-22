﻿using BlackJack.Engine.Classes;
namespace BlackJack;
using System.Linq;
using System;
using BlackJack.Classes;

public class BlackJackEngine
{
    int roundCounter = 0;
    public List<Player> PlayerList { get; } = playerListFill();  //lista di player
    Dealer dealer = new Dealer();

    public List<Card> Deck { get; set; } = new();  //deck

    internal void CreateDeckAndShuffleIt()
    {
        int decks = 0;
        do
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j <= 13; j++)
                {
                    Deck.Add(new Card
                    {
                        Seed = (Engine.Classes.Seeds)(Seeds)i,
                        Value = j == 1 ? 11 : (j > 10 ? 10 : j),
                        RealValue = j,
                    });
                }
            }
            decks++;
        } while (decks < 4);
        Deck = Deck.OrderBy(card =>
        {
            var randomValue = Guid.NewGuid();
            return randomValue.ToString();
        }).ToList();
    }

    public Card PickCard()
    {
        Card card;
        card = Deck.First();
        Deck.Remove(card);
        return card;
    }
    public void playerBuild(string name, float balance)
    {

        Player player = new Player();
        player.name = name;
        player.balance = balance;
        for (int i = 0; i < 4; i++)
        {
            if (PlayerList[i].name.Contains("bot"))
            {
                PlayerList[i] = player;
                break;
            }
        }
    }
    internal static List<Player> playerListFill()                   //chiamato una volta, prima di startare il primo round
    {
        List<Player> pList = new List<Player>();
        for (int i = 0; i < 4; i++)
        {
            pList.Add(new Player { balance = 1000, name = "$bot {i}" });
        }
        return pList;
    }
    public void endRound()
    {
        roundCounter++;
        dealer.hand.Cards.Clear();
        foreach (Player player in PlayerList)
        {
            player.hand.Cards.Clear();
            player.bet = 0;
        }
    }
    public void playBot()
    {
        foreach (Player bot in PlayerList)
        {
            if (bot.name.Contains("bot"))
            {
                if (bot.makeBet(50))
                {
                    bool stand = false;
                    do
                    {
                        if (bot.hand.Points <= 16)
                        {
                            bot.hand.Cards.Add(PickCard());
                        }
                        else if (bot.hand.Points >= 17 && bot.hand.Points < 22)
                        {
                            stand = true;
                        }

                    } while (stand != true && bot.busted != true);
                }

            }
        }
    }

    public void playUser(Player p, Choice s)
    {
        bool stand = false;
        do
        {
            stand = s != Choice.hit;
            switch (s)
            {
                case Choice.hit:
                    p.hand.Cards.Add(PickCard());
                    break;
                case Choice.stand:
                    break;
                case Choice.multiply:
                    p.hand.Cards.Add(PickCard());
                    break;
            }
        } while (!stand && !p.busted);
    }

    public void playDealer()
    {
        bool stand = false;
        do
        {

            if (dealer.hand.Points <= 16)
            {
                dealer.hand.Cards.Add(PickCard());
            }
            else if (dealer.hand.Points >= 17 && dealer.hand.Points < 22)
            {
                stand = true;
            }

        } while (stand != true && dealer.busted != true);
    }

    public Score WinCheck(Player p)
    {
        if (p.busted)
        {
            return Score.lose;
        }
        else if (p.hand.Points > dealer.hand.Points)
        {
            if (p.hand.Cards.Count > 2)
            {
                return Score.win;
            }
            else { return Score.bj; }
        }
        else 
        {
            return Score.lose;
        }

        
    }
    //-------------------------------------------------------------
    public void Initilize()
    {

        if (roundCounter == 2 || roundCounter == 0)
        {
            CreateDeckAndShuffleIt();
            roundCounter = 0;
        }
        dealer.hand.Cards.Add(PickCard());

        for (int i = 0; i < 4; i++)
        {
            PlayerList[i].hand.Cards.Add(PickCard());
            PlayerList[i].hand.Cards.Add(PickCard());
        }

    }
}
public enum Choice
{
    hit,
    stand,
    multiply
}

public enum Score
{
    win,
    lose,
    bj,
    push
}