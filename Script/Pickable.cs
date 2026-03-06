using Godot;
using System;

public partial class Pickable : Node
{
    [Export] public string ItemName = "Object";
    [Export] public Texture2D ItemIcon;
}
