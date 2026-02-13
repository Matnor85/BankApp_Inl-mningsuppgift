using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.UI;

internal class ConsoleMenu
{
    private readonly List<MenuItem> _items;
    private int _selected;

    public ConsoleMenu(IEnumerable<MenuItem> items)
    {
        _items = new List<MenuItem>(items);
        _selected = FindFirstSelectable();
    }

    public void Run()
    {
        if (_items.Count == 0 || _selected == -1) return;

        while (true)
        {
            Render();
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    _selected = MoveSelectable(_selected, -1);
                    break;
                case ConsoleKey.DownArrow:
                    _selected = MoveSelectable(_selected, +1);
                    break;
                case ConsoleKey.Enter:
                    if (_selected >= 0 && _items[_selected].IsSelectable)
                    {
                        Console.Clear();
                        try
                        {
                            _items[_selected].Action?.Invoke();
                        }
                        catch (OperationCanceledException)
                        {
                            return;
                        }
                    }
                    break;
                case ConsoleKey.Escape:
                    return;
            }
        }
    }

    private void Render()
    {
        Console.Clear();
        for (int i = 0; i < _items.Count; i++)
        {
            var item = _items[i];
            if (!item.IsSelectable)
            {
                // Visa rubrik/meddelande utan markering
                Console.WriteLine(item.Text);
                continue;
            }

            if (i == _selected)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"-> {item.Text} <-");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"  {item.Text}");
            }
        }
        Console.WriteLine();
        //Console.WriteLine("Navigera med piltangenterna. Tryck Enter för att välja, Esc för att gå tillbaka.");
        Console.WriteLine("Navigera med [↑] [↓] " +
            "\nTryck [↵] för att välja " +
            "\n[Esc] för att gå tillbaka.");
    }

    private int FindFirstSelectable()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].IsSelectable) return i;
        }
        return -1;
    }

    private int MoveSelectable(int start, int dir)
    {
        if (_items.Count == 0) return -1;
        int i = start;

        // Flytta ett steg i given riktning utan att wrappa.
        while (true)
        {
            i += dir;

            // Om vi når utanför listan, stanna kvar på start (ingen wrap).
            if (i < 0 || i >= _items.Count)
                return start;

            if (_items[i].IsSelectable)
                return i;
            // Annars fortsätt leta i samma riktning tills start eller en selectable hittas.
        }
    }
}

