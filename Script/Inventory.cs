using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory : Node
{
    public List<string> Items = new List<string>();

    public void AddItem(string itemName)
    {
        Items.Add(itemName);
        GD.Print("Ajouté à l'inventaire : " + itemName);
    }

    public void PrintInventory()
    {
        GD.Print("=== Inventory ===");
        foreach (var item in Items)
        {
            GD.Print($"- {item}");
        }
    }
}
