using Godot;
using System;

public partial class Player : CharacterBody3D
{
    // --- Variable
    // Variable de la souris / regard
    float lookAngle = 90.0f;
    float mouseSensivity = 2.0f;
    Vector3 mouseDelta = new Vector3();
    // FIN souris

    // Références
    Camera camera;

    // --- Fonction de GODOT
    // Ready
    public override void _Ready()
    {
        base._Ready();
        camera = GetNode("Camera3D") as Camera;
    }
    // Gestion des inputs et du regard (camera)
    public override void _Input(InputEvent ev)
    {
        if(ev is InputEventMouseMotion eventMouse)
        {
            mouseDelta = eventMouse.Relative;
        }
    }

    public override void _Process(float delta)
    {
        camera.RotationDegrees -= new Vector3(Mathf.Rad2Deg)
    }
}