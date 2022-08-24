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

    internal void CreateDeckAndShuffleIt()                          //crea e mischia 4 deck insieme
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
    /*metod0 per pescare una carta, restituisce un oggetto di tipo Card.
     */
    public Card PickCard()                                  
    {
        Card card;
        card = Deck.First();
        Deck.Remove(card);
        return card;
    }
    /*//metod0 per la creazione ed inserimento di un nuovo player nella lista.
     * Va richiamato all'inizio della partita, passando nome e saldo
     */
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
    /*metod0 interno che riempie la lista dei player di bot numerati;
     * Viene usato dall'oggetto al momento della creazione.
     */
    internal static List<Player> playerListFill()                   
    {
        List<Player> pList = new List<Player>();
        for (int i = 0; i < 4; i++)
        {
            pList.Add(new Player { balance = 1000, name = $"bot {i}" });
        }
        return pList;
    }
    /*metod0 che fa giocare tutti i bot, purchè ce ne sia uno, contenuti
     * nella lista dei player:
     * il bot fino a 16 punti in mano chiama carta;
     * il bot da 17 punti in poi sta.
     * Va usato dopo aver fatto giocare i player.
     */
    public void AutoPlayAllBots()                               
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
    /*metod0 che fa giocare il bealer in automatico:
     * il dealer fino a 16 punti compresi in mano chiama carta;
     * il dealer da 17 punti in poi sta;
     * Va richiamato dopo aver usato AutoPlayAllBots().
     */
    public void AutoPlayDealer()                                            
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

        } while (stand != true && dealer.Busted != true);
    }
    /*Metod0 che fa pescare una carta al player passato come parametro, 
     * controllando che non sballi.
     * Va richiamato durante le turnazioni
     */
    public bool Hit(Player p)               
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
    /*Metod0 che fa stare il player passato come parametro.
     * Va richiamato durante le turnazioni
     */
    public void Stand(Player p)
    {
        p.stand = true;
    }

    /*Metod0 che fa pescare una carta e raddoppia la puntata al player passato come parametro, 
     * controllando che il saldo sia sufficiente per effettuare il raddoppio.
     * Va richiamato durante le turnazioni
     */
    public bool Double(Player p)
    {
        if (p.balance >= (p.bet * 2))
        {
            p.bet += p.bet;
            p.balance -= p.bet;
            p.stand = true;
            p.hand.Cards.Add(PickCard());
            return true;
        }
        else { return false; }
        
    }
    /*metod0 che mostra le carte del player e del dealer passato come parametro restituendo un oggetto StringBuilder.
     * Va richiamato almeno dopo aver usato Initialize().
     */
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
    /*   Metod0 che scorre tutti i player e ne controlla vincita, perdita, push
     *   confrontandoli con il punteggio del dealer e distribuisce la vincita.
     *   Va richiamato dopo aver usato AutoPlayDealer().
     */
    public void WinCheck()                                          
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
    /*metod0 che svuota le mani di ogni giocatore, resetta le variabili per i vari controlli
     * di gioco, svuota la mano del dealer e incrementa il contatore dei round.
     * Va richiamato alla fine di ogni round come ultimo comando.
     */
    public void endRound()  
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
    /*// metod0 che controlla che ogni 2 round si mischi il mazzo e
     * distribuisce ad ogni giocatore compresi i bot 2 carte all'inizio della partita, 
     * da al dealer una carta.
     * Va chiamato dopo aver fatto le puntate ad ogni round.
     */
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