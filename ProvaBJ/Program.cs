using BlackJack;
using BlackJack.Classes;
using BlackJack.Engine;
using System.Text;


BlackJackEngine bj = new BlackJackEngine();

Console.WriteLine("Inserisci il numero dei giocatori: ");
int np = int.Parse(Console.ReadLine());

for(int i = 0; i < np; i++)
{
    Console.WriteLine("\nInserisci il nome del "+(i+1)+"^ giocatore: ");
    string nome = Console.ReadLine();
    Console.WriteLine("\nInserisci il saldo di " + nome);
    float saldo = float.Parse(Console.ReadLine());
    bj.playerBuild(nome, saldo);
}

while (true)
{

    for (int i = 0; i < np; i++)
    {
        Console.WriteLine("\nInserisci la puntata di " + bj.PlayerList[i].name+", saldo: "+bj.PlayerList[i].balance);
       float puntata = float.Parse(Console.ReadLine());
        if (bj.PlayerList[i].makeBet(puntata))
        {
            Console.WriteLine("\nPuntata effettuata correttamente");
        }
        else
        {
            Console.WriteLine("\nSaldo insufficiente");
            bj.PlayerList[i].stand = true;
        }
    }

    bj.Initilize();
    for (int i = 0; i < np; i++)
    {
        while (bj.PlayerList[i].Busted == false && bj.PlayerList[i].stand == false)
            {
                Console.WriteLine("-----------------------" + " \nTURNO DI: " + bj.PlayerList[i].name);
                Console.WriteLine("\nMANO DEL DEALER : ");
                Console.Write(bj.dealer.ShowCards());
                Console.WriteLine("\n\nMANO : ");
                Console.Write(bj.PlayerList[i].ShowCards());
                Console.WriteLine("\nPunti: " + bj.PlayerList[i].hand.Points);
                Console.WriteLine("\n\n1 - Hit");
                Console.WriteLine("2 - Stand");
                Console.WriteLine("3 - Double");
                Console.WriteLine("\nInserisci il numero della mossa: ");
                int s = int.Parse(Console.ReadLine());
                switch (s)
                {

                    case 1:
                        if (!bj.Hit(bj.PlayerList[i]))
                        {
                            Console.WriteLine("\n" + bj.PlayerList[i].name + " ha pescato");
                        }
                        break;
                    case 2:
                        bj.Stand(bj.PlayerList[i]);
                        Console.WriteLine("\n" + bj.PlayerList[i].name + " sta");
                        break;
                    case 3:
                       if(bj.Double(bj.PlayerList[i])) 
                        {
                            Console.WriteLine("\n" + bj.PlayerList[i].name + " raddoppia");
                        }
                    else
                        {
                            Console.WriteLine("\n" + bj.PlayerList[i].name + " non può raddoppiare");
                        }
                        break;
                }
            if (bj.PlayerList[i].Busted)
            {
                Console.WriteLine("\n" + bj.PlayerList[i].name + " ha sballato");
            }
                Console.WriteLine("-----------------------" + "\nTURNO FINITO");
            }
    }

    bj.AutoPlayAllBots();
    bj.AutoPlayDealer();
    bj.WinCheck();


    if (bj.dealer.Busted)
    {
        Console.WriteLine(bj.dealer.ShowCards());
        Console.WriteLine("\nIl Dealer ha sballato, punti: "+bj.dealer.hand.Points);
    }
    else if (bj.dealer.bj == true)
    {
        Console.WriteLine(bj.dealer.ShowCards());
        Console.WriteLine("\nIl Dealer ha fatto Black Jack ");

    }
    else
    {
        Console.WriteLine(bj.dealer.ShowCards());
        Console.WriteLine("\nIl Dealer ha fatto: " + bj.dealer.hand.Points);
    }

    for (int i = 0; i < 4; i++)
    {
        if (bj.PlayerList[i].Busted)
        {
            Console.WriteLine(bj.PlayerList[i].ShowCards());
            Console.WriteLine("\n" + bj.PlayerList[i].name + " ha sballato, punti: "+ bj.PlayerList[i].hand.Points+", perdita: "+ bj.PlayerList[i].bet);
        }
        else if (bj.PlayerList[i].push)
        {
            Console.WriteLine(bj.PlayerList[i].ShowCards());
            Console.WriteLine("\n" + bj.PlayerList[i].name + " ha pareggiato, punti: " + bj.PlayerList[i].hand.Points+", vincita: "+ bj.PlayerList[i].bet);

        }
        else if (bj.PlayerList[i].bj)
        {
            Console.WriteLine(bj.PlayerList[i].ShowCards());
            Console.WriteLine("\n" + bj.PlayerList[i].name + " ha fatto Black Jack, vincita: " + ((bj.PlayerList[i].bet * 2) + bj.PlayerList[i].bet / 2));

        }
        else if(bj.PlayerList[i].win)
        {
            Console.WriteLine(bj.PlayerList[i].ShowCards());
            Console.WriteLine("\n" + bj.PlayerList[i].name + " ha vinto, punti: " + bj.PlayerList[i].hand.Points+", vincita: "+(bj.PlayerList[i].bet)*2);

        }
        else
        {
            Console.WriteLine(bj.PlayerList[i].ShowCards());
            Console.WriteLine("\n" + bj.PlayerList[i].name + " ha perso, punti: " + bj.PlayerList[i].hand.Points+", perdita: "+ bj.PlayerList[i].bet);
        }
    }
    bj.endRound();
}