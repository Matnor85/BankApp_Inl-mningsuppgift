using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.UI;

// Klassen hanterar visning av en meny i konsolen och navigering med piltangenter.
internal class ConsoleMenu
{
    private readonly List<MenuItem> _items; // Listan över alla alternativ i menyn
    private int _selected; // Index för det objekt som just nu är markerat

    // Konstruktor: Tar in en lista med menyalternativ
    public ConsoleMenu(IEnumerable<MenuItem> items)
    {
        _items = new List<MenuItem>(items);
        // Hitta det första objektet som faktiskt går att välja (inte bara en rubrik)
        _selected = FindFirstSelectable();
    }

    // Huvudloopen som kör menyn tills användaren avslutar.
    public void Run()
    {
        // Om menyn är tom eller inget valbart objekt finns, avbryt direkt
        if (_items.Count == 0 || _selected == -1) return;

        while (true)
        {
            Console.Clear();
            // Rita upp menyn på skärmen
            Render();

            // Läs in en knapptryckning från användaren (utan att visa tecknet i konsolen)
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                // Flytta markering uppåt
                case ConsoleKey.UpArrow:
                    _selected = MoveSelectable(_selected, -1);
                    break;
                // Flytta markering nedåt
                case ConsoleKey.DownArrow:
                    _selected = MoveSelectable(_selected, +1);
                    break;
                case ConsoleKey.Enter:
                    // Om det markerade objektet går att välja, kör dess action
                    if (_selected >= 0 && _items[_selected].IsSelectable)
                    {
                        Console.Clear();
                        try
                        {
                            // Invoke() kör själva metoden (lambda-uttrycket) som är kopplad till objektet
                            _items[_selected].Action?.Invoke();
                        }
                        catch (OperationCanceledException)
                        {
                            // Detta exception används för att bryta sig ur menyer och gå tillbaka
                            return;
                        }
                    }
                    break;
                case ConsoleKey.Escape:
                    // Avslutar menyn
                    return;
            }
        }
    }

    // Ritar upp menyn i konsolfönstret.
    private void Render()
    {
        Console.Clear();
        for (int i = 0; i < _items.Count; i++)
        {
            var item = _items[i]; 

            // Hantera objekt som inte är valbara (t.ex. rubriker)
            if (!item.IsSelectable)
            {
                // Visa rubrik/meddelande utan markering och går till nästa objekt.
                Console.WriteLine(item.Text);
                continue;
            }

            // Hanterar objekt som är valbara
            if (i == _selected)
            {
                // Om detta är det markerade objektet, ge det ett annan färg
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"-> {item.Text} <-");
                Console.ResetColor();
            }
            else
            {
                // Inte markerat: Skriver ut dom normalt.
                Console.WriteLine($"  {item.Text}");
            }
        }

        // 2. Flytta markören till nederkanten av konsolen
        // Console.WindowHeight ger oss det totala antalet rader
        int bottomLine = Console.WindowHeight - 4; // -4 för att ha lite marginal (instrument + tomma rader)

        // Se till att vi inte försöker sätta markören utanför fönstret
        if (bottomLine > 0)
        {
            Console.SetCursorPosition(0, bottomLine);
        }

        // 3. Skriv ut instruktionerna längst ner
        Console.WriteLine("-----------------------------------");
        Console.WriteLine("Navigera med [↑] [↓] | [↵] Välj | [Esc] Tillbaka");

        // Instruktionerna i botten av menyn.
        //Console.WriteLine("\n\n");
        //Console.WriteLine("Navigera med piltangenterna. Tryck Enter för att välja, Esc för att gå tillbaka.");
        //Console.WriteLine("Navigera med [↑] [↓] " +
        //"\nTryck [↵] för att välja " +
        //"\n[Esc] för att gå tillbaka.");
    }

    // Hittar indexet för det första menyalternativet som är markerbart.
    private int FindFirstSelectable()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].IsSelectable) return i;
        }
        // Om inget valbart hittades.
        return -1;
    }

    // Flyttar markeringen upp eller ner i listan, och hoppar över ej-valbara objekt.
    private int MoveSelectable(int start, int dir)
    {
        if (_items.Count == 0) return -1;
        int i = start;

        // Loopa tills vi hittar ett valbart objekt
        while (true)
        {
            // Flytta indexet i önskad riktning (+1 eller -1)
            i += dir;

            // Om vi når utanför listan, stanna kvar på nuvarande position
            if (i < 0 || i >= _items.Count)
                return start;

            // Om objektet är valbart, returnera dess index
            if (_items[i].IsSelectable)
                return i;
            // Om objektet inte var valbart, fortsätt loopen (letar vidare)
        }
    }
}

