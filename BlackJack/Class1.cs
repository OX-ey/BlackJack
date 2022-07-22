namespace BlackJack;
using System.Linq;



public class Card
    {
    public Seeds Seed { get; set; }
    public  int Value { get; set; } 
    public int RealValue { get; set; }
}

public class Hand
{
    public List<Card> Cards = new List<Card>(); 
    public int Points => Cards.Sum(x => x.Value);
}

public class Player
{
    public Hand hand = new Hand();
    public string name { get; set; }    
    public float balance { get; set; }

    public float bet = 0;

    public bool busted = false; 
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

public class Dealer
{
    public Hand hand = new Hand();
    public bool busted = false;
}


public class BalckJackEngine
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
                        Seed = (Seeds)i,
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
        Hand h = new Hand();
        Player player = new Player();
        player.hand = h;
        player.name = name;
        player.balance = balance;
        for (int i = 0; i < 4; i++)
        {
            if (PlayerList[i].name == "bot")
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
            pList.Add(new Player { balance = 1000, name = "bot" });
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
            player.busted = false;
        }
    }

    public void playBot()
    {
        foreach (Player bot in PlayerList)
        {
            if (bot.name == "bot")
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
                        else
                        {
                            bot.busted = true;
                        }
                    } while (stand != true && bot.busted != true);
                }
                else
                {
                    bot.busted = true;
                }
            }
        }
    }




    //----------------------------------------
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
    public enum Seeds
    {
        hearts,
        diamonds,
        clubs,
        spades 
    }
