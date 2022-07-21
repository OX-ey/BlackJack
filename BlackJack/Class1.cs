namespace BlackJack;
using System.Linq;



public class Card
    {
    public int Seed { get; set; }
    public  int Value { get; set; } 
    public int RealValue { get; set; }
}

public class Hand
{
    public List<Card> Cards { get; set; }
}

public class Player
{
    public Hand hand { get; set; } 
    public string name { get; set; }    
    public float balance { get; set; }  


}

public class BalckJackEngine
{
    int roundCounter = 0;

    List<Player> playerList = new List<Player>();               //lista player
    public List<Card> Deck { get; set; } = new();


    public void  CreateDeckAndShuffleIt()
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
                        Seed = i,
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

    public Card  PickCard()
    {
        Card card;
        card = Deck.First();
        Deck.Remove(card);
        return card;
    }

    public void playerBuild(string name, float balance) 
    {
        Hand h = new Hand();
        h = null;
        Player player = new Player();
        player.hand = h;
        player.name = name; 
        player.balance = balance;
        for (int i = 0; i < 4; i++)
        {
            if (playerList[i].name == "bot")
            {
                playerList[i] = player;
            }
        }
    }

    //----------------------------------------
    public void StartRound(int players)
    {
        for(int i = 0; i < 4; i++)
        {
            playerList.Add(new Player { balance = 1000, name = "bot" });
        }
        int counterPlayers = 0;
        do
        {
            if (roundCounter == 2 || roundCounter == 0)
            {
                CreateDeckAndShuffleIt();
                roundCounter = 0;
            }
            

        } while (counterPlayers < 4);
    }

}