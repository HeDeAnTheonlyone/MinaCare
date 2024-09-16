using Godot;
using System;
using System.Linq;



public partial class Minawan : AnimatedSprite2D
{
[Export] public float MinaScale { get; set; } = 0.2f;
[Export] public float MaxSpeed { get; set; } = 250;
[Export] public float Acceleration { get; set; } = 5f;
[Export] public float StoppingDistance { get; set; } = 125;
[Export] public Vector2[] PassthroughPolygon { get; private set; }
private AudioStreamPlayer wanWanSFX;
private Line2D minaShape;
private float speed = 0;
private Vector2 prevPos = new Vector2();
private Vector2 centerOffset;
private Vector2I windowSize = Vector2I.Zero;



	public override void _Ready()
	{
		UseRandomMinawanTexture();

		wanWanSFX = GetNode<AudioStreamPlayer>("WanWanSFX");

		Scale = new Vector2(MinaScale, MinaScale);
		centerOffset = SpriteFrames.GetFrameTexture("default", 0).GetSize() * MinaScale / 2;

		PassthroughPolygon = new Vector2[] {
			centerOffset * -1,
			new Vector2(centerOffset.X, -centerOffset.Y),
			centerOffset,
			new Vector2(-centerOffset.X, centerOffset.Y)
		};

		for (int i = 0; i < DisplayServer.GetScreenCount(); i++)
		{
			windowSize += DisplayServer.ScreenGetSize(i);
		}
		GetWindow().Position = Vector2I.Zero;
		GetWindow().Size = windowSize;

		Position = GetGlobalMousePosition();
	}

	public override void _Process(double delta)
	{
		Vector2 mousePos = GetGlobalMousePosition();

		if (Position.DistanceTo(mousePos) > StoppingDistance) speed = Math.Clamp(speed + Acceleration, 0, MaxSpeed);
		else speed = Math.Clamp(speed - Acceleration * 2, 0, MaxSpeed);

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

	private void UseRandomMinawanTexture()
	{
		DirAccess minawanCollectionDir = DirAccess.Open("./Minawan");
		GD.Print(minawanCollectionDir.GetCurrentDir());

		if (minawanCollectionDir == null) return;

		string[] availableMinawans = minawanCollectionDir.GetDirectories();
		var rng = new Random();
		string minawan = availableMinawans[rng.Next(0, availableMinawans.Length)];

		string[] texturesFiles = DirAccess.Open($"./Minawan/{minawan}").GetFiles();
		
		if (!(texturesFiles.Contains("Stand.png") && texturesFiles.Contains("Walk.png"))) return;

		SpriteFrames.SetFrame("default", 0, ImageTexture.CreateFromImage(Image.LoadFromFile($"./Minawan/{minawan}/Stand.png")));
		SpriteFrames.SetFrame("default", 1, ImageTexture.CreateFromImage(Image.LoadFromFile($"./Minawan/{minawan}/Walk.png")));
	}

	private void UpdateInteractableShape()
	{
		Vector2[] newPoints = new Vector2[PassthroughPolygon.Length];

		for (int i = 0; i < newPoints.Length; i++)
		{
			newPoints[i] = Position + PassthroughPolygon[i];
		}

		GetWindow().MousePassthroughPolygon = newPoints;
	}

	private void OnClickMinawan(Node viewport, InputEvent evnt, int shape_idx)
	{
		if (evnt.IsActionPressed("LeftClick")) wanWanSFX.Play();  
	}
}
