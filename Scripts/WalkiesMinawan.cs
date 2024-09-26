using Godot;
using Godot.Collections;
using System.Collections;
using System;
using System.Linq;
using System.Reflection;



public partial class WalkiesMinawan : AnimatedSprite2D
{
[Export] public float MinaScale { get; set; } = 0.2f;
[Export] public float MaxSpeed { get; set; } = 250f;
[Export] public float Acceleration { get; set; } = 5f;
[Export] public float DecelerationDistance { get; set; } = 250f;
private Vector2[] passthroughPolygon;
private AudioStreamPlayer wanWanSFX;
private Line2D breadcrumbs;
private Line2D summonShape;
private ArrayList last3Breadcrumbs = new ArrayList();
private MinawanActionMenu actionMenu;
private Window window;
private float speed = 0;
private Vector2 prevPos;



	public override void _Ready()
	{
		wanWanSFX = GetNode<AudioStreamPlayer>("WanWanSFX");
		// breadcrumbs = GetNode<Line2D>("../Breadcrumbs");a
		// summonShape = GetNode<Line2D>("../SummonShape");
		actionMenu = GetNode<MinawanActionMenu>("ActionMenu");
		actionMenu.RequestChangeMinawan += UseRandomMinawanTexture;
		window = GetWindow();

		LoadMinawanData();
		Scale = new Vector2(MinaScale, MinaScale);
		GeneratePassthroughPolygon();
		Position = GetGlobalMousePosition();
		last3Breadcrumbs.AddRange(Enumerable.Repeat(Position, 3).ToArray());
	}


    public override void _ExitTree()
    {
        actionMenu.RequestChangeMinawan -= UseRandomMinawanTexture;
    }


    public override void _Process(double delta)
	{
		MoveMinawan((float)delta);
	}


	private void UseRandomMinawanTexture()
	{
		DirAccess minawanCollectionDir = DirAccess.Open("./Minawan");

		if (minawanCollectionDir == null) return;

		string[] availableMinawans = minawanCollectionDir.GetDirectories();
		var rng = new Random();
		string minawan = availableMinawans[rng.Next(0, availableMinawans.Length)];

		string[] texturesFiles = DirAccess.Open($"./Minawan/{minawan}").GetFiles();

		if (!(texturesFiles.Contains("Stand.png") && texturesFiles.Contains("Walk.png"))) return;

		SpriteFrames.SetFrame("default", 0, ImageTexture.CreateFromImage(Image.LoadFromFile($"./Minawan/{minawan}/Stand.png")));
		SpriteFrames.SetFrame("default", 1, ImageTexture.CreateFromImage(Image.LoadFromFile($"./Minawan/{minawan}/Walk.png")));
	}


	private void MoveMinawan(float delta)
	{
		Vector2 mousePos = GetGlobalMousePosition();

		if (Position.DistanceTo(mousePos) > DecelerationDistance) speed = Mathf.Clamp(speed + Acceleration, 0, MaxSpeed);
		else speed = Mathf.Clamp(speed - Acceleration * 2, 0, MaxSpeed);

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


private void LoadMinawanData()
{
	Dictionary data = Manager.Load("minawan_settings");

	if (data == null)
	{
		SaveMinawanData();
		return;
	}

	PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.DeclaringType == typeof(WalkiesMinawan)).ToArray();
	bool hasUnknownKey = false;

	foreach (PropertyInfo property in properties)
	{
		if (!data.ContainsKey(property.Name))
		{
			hasUnknownKey = true;
			continue;
		}

		GetType().GetProperty(property.Name).SetValue(this, (float)data[property.Name]);
	}

	if (hasUnknownKey) SaveMinawanData();
}


private void SaveMinawanData()
{
	Dictionary data = new Dictionary();
	PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.DeclaringType == typeof(WalkiesMinawan)).ToArray();

	foreach (PropertyInfo property in properties) data.Add(property.Name, (float)property.GetValue(this));

	Manager.Save(data, "minawan_settings");
}


	private void UpdateInteractableShape()
	{
		Vector2[] newPoints = new Vector2[passthroughPolygon.Length];

		for (int i = 0; i < newPoints.Length; i++) newPoints[i] = Position + passthroughPolygon[i];
		
		window.MousePassthroughPolygon = newPoints;
	}


	private void GeneratePassthroughPolygon()
	{
		Vector2 spriteHalfSideLength = SpriteFrames.GetFrameTexture("default", 0).GetSize() * MinaScale / 2;

		passthroughPolygon = new Vector2[] {
			spriteHalfSideLength * -1,
			new Vector2(spriteHalfSideLength.X, -spriteHalfSideLength.Y),
			spriteHalfSideLength,
			new Vector2(-spriteHalfSideLength.X, spriteHalfSideLength.Y)
		};
	}


	private void OnClickMinawan(Node viewport, InputEvent evnt, int shape_idx)
	{
		if (evnt.IsActionPressed("AltLeftClick")) actionMenu.Popup();
		else if (evnt.IsActionPressed("LeftClick")) wanWanSFX.Play();
	}
}
