using Godot;
using System.Collections.Generic;

public partial class InventoryUI : CanvasLayer
{
    Panel panel;
    VBoxContainer itemList;
    bool isOpen = false;
    Inventory inventory;

    public override void _Ready()
    {
        inventory = GetNode<Inventory>("/root/Inventory");

        // Création de l'UI par code
        panel = new Panel();
        panel.SetAnchorsPreset(Control.LayoutPreset.CenterLeft);
        panel.Size = new Vector2(250, 400);
        panel.Position = new Vector2(20, -200);
        panel.Visible = false;
        AddChild(panel);

        var title = new Label();
        title.Text = "=== Inventaire ===";
        title.Position = new Vector2(10, 10);
        panel.AddChild(title);

        itemList = new VBoxContainer();
        itemList.Position = new Vector2(10, 40);
        itemList.Size = new Vector2(230, 350);
        panel.AddChild(itemList);
    }

    public override void _Input(InputEvent ev)
    {
        if (Input.IsActionJustPressed("inventory"))
        {
            isOpen = !isOpen;
            panel.Visible = isOpen;

            // Pause le jeu quand inventaire ouvert
            GetTree().Paused = isOpen;
            Input.MouseMode = isOpen
                ? Input.MouseModeEnum.Visible
                : Input.MouseModeEnum.Captured;

            if (isOpen) RefreshUI();
        }
    }

    void RefreshUI()
    {
        // Vider la liste
        foreach (Node child in itemList.GetChildren())
            child.QueueFree();

        // Compter les objets en double
        var counted = new Dictionary<string, int>();
        foreach (var item in inventory.Items)
        {
            if (counted.ContainsKey(item)) counted[item]++;
            else counted[item] = 1;
        }

        // Afficher chaque objet
        foreach (var entry in counted)
        {
            var label = new Label();
            label.Text = $"- {entry.Key}  x{entry.Value}";
            itemList.AddChild(label);
        }

        // Si inventaire vide
        if (inventory.Items.Count == 0)
        {
            var empty = new Label();
            empty.Text = "Inventaire vide...";
            itemList.AddChild(empty);
        }
    }
}