namespace BlackJack;
using System.Linq;



public class Card
    {
    public int Seed { get; set; }
    public  int Value { get; set; } 
    public int RealValue { get; set; }
}



public class BalckJackEngine
{
    int roundCounter = 0;
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
    //----------------------------------------
    public void StartRound(int players)
    {
        if (roundCounter == 2 || roundCounter == 0) 
        {
            CreateDeckAndShuffleIt();
            roundCounter = 0;
        }

        

    }


    


}