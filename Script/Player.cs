using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export]
	float mouseSensitivity = 0.2f;
	float cameraRotationX = 0f;
	// Variable clavier / déplacement
	float moveSpeed = 8.0f;
	float sprintSpeed = 12.0f;
	float currentSpeed;
	float gravity = 20.0f;
	float jumpForce = 8.0f;

	Vector3 velocity = new Vector3();

	SpotLight3D spotLight;
	
	Camera3D camera;

	public override void _Ready()
	{
		camera = GetNode<Camera3D>("Camera3D");
		Input.MouseMode = Input.MouseModeEnum.Captured;
		spotLight = GetNode<SpotLight3D>("Camera3D/SpotLight3D");
	}

	public override void _Input(InputEvent ev)
	{
		if (ev is InputEventMouseMotion motion)
		{
			// Rotation du joueur gauche / droite
			RotateY(Mathf.DegToRad(-motion.Relative.X * mouseSensitivity));

			// Rotation caméra haut / bas
			cameraRotationX -= motion.Relative.Y * mouseSensitivity;
			cameraRotationX = Mathf.Clamp(cameraRotationX, -80f, 80f);

			camera.RotationDegrees = new Vector3(cameraRotationX, 0, 0);
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		velocity.X = 0;
		velocity.Z = 0;

		var direction = new Vector2();

		// Test des touches du clavier ZQSD
		if (Input.IsActionPressed("forward"))
		{
			direction.Y -= 1;
		}
		if (Input.IsActionPressed("back"))
		{
			direction.Y += 1;
		}
		if(Input.IsActionPressed("left"))
		{
			direction.X -= 1;
		}
		if(Input.IsActionPressed("right"))
		{
			direction.X += 1;
		}
		if(Input.IsActionPressed("sprint"))
		{
			currentSpeed = sprintSpeed;
		} else
		{
			currentSpeed = moveSpeed;
		}
		if(Input.IsActionJustPressed("EnableLight"))
		{
			spotLight.Visible = !spotLight.Visible;
		}

			direction = direction.Normalized();

		var forward = GlobalTransform.Basis.Z;
		var right = GlobalTransform.Basis.X;

		// on calculme la vélocité
		// avant / arrière
		velocity.Z = (forward * direction.Y + right * direction.X).Z * currentSpeed;
		// gauche / droite
		velocity.X = (forward * direction.Y + right * direction.X).X * currentSpeed;
		// haut bas / saut
		velocity.Y -= gravity * (float)delta;

		// on applique le mouvement
		Velocity = velocity;
		MoveAndSlide();
		// Gestion du saut
		// SI touche espace + on est au sol... On peut sauter
		if(Input.IsActionPressed("jump") && IsOnFloor())
		{
			velocity.Y = jumpForce;
		}
	}
}
