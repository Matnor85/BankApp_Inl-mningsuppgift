using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.UI;

internal class MenuItem
{
    // Texten som användaren ser i menyn.
    public string Text { get; }

    // Vad som ska hända när användaren väljer detta objekt (metoden som körs).
    // Är null om objektet inte är valbart (t.ex. en rubrik).
    public Action Action { get; }

    // Anger om användaren kan markera och klicka på denna rad.
    // Används för att skilja på klickbara alternativ och statiska rubriker.
    public bool IsSelectable { get; }

    // --- KONSTRUKTORER ---

    // 1. För valbara alternativ (t.ex. "Sätt in pengar").
    // Action behövs, och IsSelectable sätts automatiskt till true.
    public MenuItem(string text, Action action) : this(text, action, true) { }

    // 2. För rubriker eller info-text (t.ex. "--- VÄLJ KONTO ---").
    // Ingen Action behövs, och IsSelectable sätts automatiskt till false.
    public MenuItem(string text) : this(text, null!, false) { }

    // 3. Huvudkonstruktorn som alla andra anropar.
    public MenuItem(string text, Action action, bool isSelectable)
    {
        Text = text;
        Action = action;
        IsSelectable = isSelectable;
    }
}

