using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Reflection;



public partial class Minawan : AnimatedSprite2D
{
[Export] public float MinaScale { get; set; } = 0.2f;
[Export] public float MaxSpeed { get; set; } = 250f;
[Export] public float Acceleration { get; set; } = 5f;
[Export] public float DecelerationDistance { get; set; } = 125f;
private Vector2[] passthroughPolygon;
private AudioStreamPlayer wanWanSFX;
private Window actionMenu;
private Window window;
private float speed = 0;
private Vector2 prevPos = new Vector2();



	public override void _Ready()
	{
		wanWanSFX = GetNode<AudioStreamPlayer>("WanWanSFX");
		actionMenu = GetNode<Window>("ActionMenu");
		window = GetWindow();

		LoadMinawanData();
		UseRandomMinawanTexture();
		Scale = new Vector2(MinaScale, MinaScale);
		GeneratePassthroughPolygon();
		SetUpWindow();
		Position = GetGlobalMousePosition();
	}


    public override void _Process(double delta)
	{
		MoveMinawan((float)delta);
	}


	private void MoveMinawan(float delta)
	{
		Vector2 mousePos = GetGlobalMousePosition();

		if (Position.DistanceTo(mousePos) > DecelerationDistance) speed = Math.Clamp(speed + Acceleration, 0, MaxSpeed);
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

		if (minawanCollectionDir == null) return;

		string[] availableMinawans = minawanCollectionDir.GetDirectories();
		var rng = new Random();
		string minawan = availableMinawans[rng.Next(0, availableMinawans.Length)];

		string[] texturesFiles = DirAccess.Open($"./Minawan/{minawan}").GetFiles();
		
		if (!(texturesFiles.Contains("Stand.png") && texturesFiles.Contains("Walk.png"))) return;

		SpriteFrames.SetFrame("default", 0, ImageTexture.CreateFromImage(Image.LoadFromFile($"./Minawan/{minawan}/Stand.png")));
		SpriteFrames.SetFrame("default", 1, ImageTexture.CreateFromImage(Image.LoadFromFile($"./Minawan/{minawan}/Walk.png")));
	}


private void LoadMinawanData()
{
	string dataString; 
	using (FileAccess file = FileAccess.Open("user://minawan_settings.json", FileAccess.ModeFlags.Read))
	{
		if (FileAccess.GetOpenError() != Error.Ok)
		{
			SaveMinawanData();
			return;
		}
		
		dataString = file.GetAsText();
	}

	Dictionary data = (Dictionary)Json.ParseString(dataString);
	
	PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.DeclaringType == typeof(Minawan)).ToArray();

	foreach (PropertyInfo property in properties)
	{
		GetType().GetProperty(property.Name).SetValue(this, (float)data[property.Name]);
	}
}


private void SaveMinawanData()
{
	Dictionary data = new Dictionary();
	PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.DeclaringType == typeof(Minawan)).ToArray();

	foreach (PropertyInfo property in properties)
	{
		data.Add(property.Name, (float)property.GetValue(this));
	}

	using (FileAccess file = FileAccess.Open("user://minawan_settings.json", FileAccess.ModeFlags.Write))
	{
		file.StoreString(Json.Stringify(data, "\t"));
	};
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


	private void SetUpWindow()
	{
		Vector2I windowSize = Vector2I.Zero;
		for (int i = 0; i < DisplayServer.GetScreenCount(); i++) windowSize += DisplayServer.ScreenGetSize(i);
		window.Position = Vector2I.Zero;
		window.Size = windowSize;
		window.Borderless = true;
		window.TransparentBg = true;
		window.Transparent =
		window.Unfocusable = true;
		window.Unfocusable = true;
		window.ContentScaleMode = Window.ContentScaleModeEnum.Disabled;
		window.ContentScaleAspect = Window.ContentScaleAspectEnum.Keep;
		window.Title = TranslationServer.Translate("WALKIES");
	}


	private void OnClickMinawan(Node viewport, InputEvent evnt, int shape_idx)
	{
		if (evnt.IsActionPressed("AltLeftClick")) actionMenu.Popup();
		else if (evnt.IsActionPressed("LeftClick")) wanWanSFX.Play();
	}
}
