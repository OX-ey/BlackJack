using BlackJack.Engine.Classes;
namespace BlackJack;
using System.Linq;
using System;
using System.Text;
using BlackJack.Classes;
using BlackJack.Interfaces;

public class BlackJackEngine
{
    int roundCounter = 0;
    public List<Player> PlayerList { get; } = playerListFill();  //lista di player
    public Dealer dealer = new Dealer();

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
                        RealValue = (RealValues)j,
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
            pList.Add(new Player { balance = 1000, name = $"bot {i}" });
        }
        return pList;
    }
    public void AutoPlayAllBot()                               //metod0 che fa giocare i bot in automatico
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

                    } while (stand != true && bot.Busted != true);
                }
            }
        }
    }

    public void AutoPlayDealer()                                            //metod0 che fa giocare il dealer in automatico
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
            else if (dealer.hand.Points==21 && dealer.hand.Cards.Count == 2)
            {
                dealer.bj = true;
            }

        } while (stand != true && dealer.busted != true);
    }

    public bool Hit(Player p)               //il metodo hitta e controlla che non si sballi
    {
        if (p.Busted==true || p.stand==true)
        {
            p.stand = true;
            return false;
        }
        else
        {
            p.hand.Cards.Add(PickCard());
            return true;
        }
    }
    public void Stand(Player p)
    {
        p.stand = true;
    }

    public void Double(Player p)
    {
        p.stand = true;
        p.hand.Cards.Add(PickCard());
        
    }

    public StringBuilder ShowCards(Player p)
    {
        StringBuilder mano = new StringBuilder();

        foreach(Card card in p.hand.Cards)
        {
            mano.AppendLine("");
            mano.Append(card.RealValue);
            mano.Append(" of ");
            mano.Append(card.Seed);
            
        }
        return mano;

    }
    public void WinCheck()                                          //Metodo che scorre tutti i player e controlla vincita, perdita, push e distribuisce la vincita
    {
        foreach (Player p in PlayerList)
        {
            if (p.Busted == false)
            {
                if (p.hand.Points > dealer.hand.Points)
                {
                    p.win = true;
                    p.balance += p.bet * 2;
                    if (p.hand.Cards.Count == 2 && p.hand.Points == 21)
                    { 
                        p.bj = true;
                        p.balance += p.bet / 2;
                    }
                }
                else if (p.hand.Points == dealer.hand.Points)
                {
                    p.push = true;
                    p.balance += p.bet;
                }
            }
        }
    }
    public void endRound()  //reset dei giocatori
    {
        roundCounter++;
        dealer.hand.Cards.Clear();
        foreach (Player player in PlayerList)
        {
            player.win = false;
            player.stand = false;
            player.bj = false;
            player.push = false;
            player.hand.Cards.Clear();
            player.bet = 0;
        }
    }
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
            if (PlayerList[i].stand == false)
            {
                PlayerList[i].hand.Cards.Add(PickCard());
                PlayerList[i].hand.Cards.Add(PickCard());
            }
        }
    }
}
public enum Choice
{
    hit,
    stand,
    multiply
}