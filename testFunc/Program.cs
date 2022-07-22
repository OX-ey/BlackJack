using BlackJack;

BlackJackEngine bj = new BlackJackEngine();

Console.WriteLine("Inserisci il numero dei player da inserire: ");
int np = int.Parse(Console.ReadLine());
for (int i = 0; i < np; i++) {
    Console.WriteLine("\nInserisci il nome del player");
    string nome = Console.ReadLine();
    Console.WriteLine("\nInserisci il saldo del player: ");
    float saldo = float.Parse(Console.ReadLine());
    bj.playerBuild(nome, saldo);
}

foreach(Player player in bj.PlayerList)
{
    if (player.name != "bot")
    {
        Console.WriteLine("\nInserisci l'importo della scommessa: ");
        float scommessa = float.Parse(Console.ReadLine());
        if (player.makeBet(scommessa))
        {
            Console.WriteLine(player.name + " ha puntato: " + player.bet);
        }
        else
        {
            Console.WriteLine(player.name + " ha un saldo insufficiente.");
        }
    }
}
bj.Initilize();
Console.WriteLine(bj.PlayerList[0].hand.Cards[0].RealValue +" of "+ bj.PlayerList[0].hand.Cards[0].Seed);

MidpointRounding()