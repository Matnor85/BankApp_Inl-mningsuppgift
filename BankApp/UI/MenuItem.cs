using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.UI;

internal class MenuItem
{
    // Visa bara texten, ingen setter så att den inte kan ändras efter skapandet.
    public string Text { get; } 
    // Action är en delegerad typ som representerar en metod utan parametrar och utan returvärde.
    public Action Action { get; } 
    public bool IsSelectable { get; }

    // Konstruktor som tar texten att visa och den action som ska utföras när menyalternativet väljs.
    // För vanliga val
    public MenuItem(string text, Action action) : this(text, action, true) { }

    // För icke-markerbara rader (t.ex. rubriker/meddelanden)
    public MenuItem(string text) : this(text, null!, false) { }

    public MenuItem(string text, Action action, bool isSelectable)
    {
        Text = text; // Sätt texten som ska visas i menyn.
        Action = action; // Sätt den action som ska utföras när menyalternativet väljs.
        IsSelectable = isSelectable;
    }
}

