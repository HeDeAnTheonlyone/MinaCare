using Godot;
using System;



public partial class Minawan : AnimatedSprite2D
{
[Export] public float MinaScale { get; set; } = 0.2f;
[Export] public float MaxSpeed { get; set; } = 250;
[Export] public float Acceleration { get; set; } = 5f;
[Export] public float StoppingDistance { get; set; } = 125;
private Line2D minaShape;
private float speed = 0;
private Vector2 prevPos = new Vector2();
private float centerOffset;



	public override void _Ready()
	{
		minaShape = GetNode<Line2D>("MinaShape");
		Scale = new Vector2(MinaScale, MinaScale);
		Position = DisplayServer.WindowGetSize() / 2;
		centerOffset = SpriteFrames.GetFrameTexture("default", 0).GetSize().X * MinaScale / 2;
	}

	
	public override void _Process(double delta)
	{
		Vector2 mousePos = GetGlobalMousePosition();

		if (Position.DistanceTo(mousePos) > StoppingDistance) speed = Math.Clamp(speed + Acceleration, 0, MaxSpeed);
		else speed = Math.Clamp(speed - Acceleration * 2, 0, MaxSpeed);
		
		GD.Print("Pos: " + Position);
		GD.Print("Mouse " + mousePos);
		GD.Print("Speed " + speed);

		Position = Position.MoveToward(mousePos, speed * (float)delta);

		FlipH = mousePos.X - Position.X > 0 ? false : true;

		if (Position - prevPos == Vector2.Zero) Stop();
		else
		{
			UpdateInteractableShape();
			Play();
		}

		prevPos = Position;
	}

	private void UpdateInteractableShape()
	{
		Vector2[] newPoints = new Vector2[minaShape.Points.Length];

		for (int i = 0; i < newPoints.Length; i++)
		{
			newPoints[i] = Position + minaShape.Points[i] * MinaScale;
		}

		GetWindow().MousePassthroughPolygon = newPoints;
	}
}
