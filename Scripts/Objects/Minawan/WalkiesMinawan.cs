using Godot;
using Godot.Collections;
using System.Collections;
using System;
using System.Linq;
using System.Reflection;



public partial class WalkiesMinawan : AnimatedSprite2D, IMinawan
{
public float MinaScale { get; set; } = 0.2f;
public float MaxSpeed { get; set; } = 250f;
public float Acceleration { get; set; } = 5f;
public float DecelerationDistance { get; set; } = 250f;
private Vector2[] interactablePolygon;
private AnimatedSprite2D patPat;
private AudioStreamPlayer wanWanSFX;
private Line2D breadcrumbs;
private Line2D summonShape;
private ArrayList last3Breadcrumbs = new ArrayList();
private MinawanActionMenu actionMenu;
private Window window;
private float speed = 0;
private Vector2 prevPos;
private SelectedAction action = SelectedAction.Wan;



	public override void _Ready()
	{
		wanWanSFX = GetNode<AudioStreamPlayer>("WanWanSFX");
		// breadcrumbs = GetNode<Line2D>("../Breadcrumbs");a
		// summonShape = GetNode<Line2D>("../SummonShape");
		actionMenu = GetNode<MinawanActionMenu>("ActionMenu");
		patPat = GetNode<AnimatedSprite2D>("PatPat");
		actionMenu.RequestChangeMinawan += () => IMinawan.UseRandomMinawanTexture(SpriteFrames);
		actionMenu.SelectWanAction += () => action = SelectedAction.Wan;
		actionMenu.SelectPatAction += () => action = SelectedAction.Pat;
		window = GetWindow();

		LoadMinawanData();
		Scale = new Vector2(MinaScale, MinaScale);
		GenerateInteractionPolygon();
		Position = GetGlobalMousePosition();
		last3Breadcrumbs.AddRange(Enumerable.Repeat(Position, 3).ToArray());
	}


    public override void _ExitTree()
    {
        actionMenu.RequestChangeMinawan -= () => IMinawan.UseRandomMinawanTexture(SpriteFrames);
		actionMenu.SelectWanAction -= () => action = SelectedAction.Wan;
		actionMenu.SelectPatAction -= () => action = SelectedAction.Pat;
	}


    public override void _Process(double delta)
	{
		MoveMinawan((float)delta);
	}


	public enum SelectedAction
	{
		Wan,
		Pat,
	}


	private void MoveMinawan(float delta)
	{
		Vector2 mousePos = GetGlobalMousePosition();

		if (Position.DistanceTo(mousePos) > DecelerationDistance) speed = Math.Clamp(speed + Acceleration, 0, MaxSpeed);  
		else speed = Math.Clamp(speed - Acceleration * 2, 0, MaxSpeed);

		Position = Position.MoveToward(mousePos, speed * (float)delta);

		Scale = mousePos.X - Position.X > 0 ? new Vector2(MinaScale, MinaScale) : Scale = new Vector2(-MinaScale, MinaScale); ;

		if (Position == prevPos) Stop();
		else
		{
			UpdateInteractablePolygon();
			Play();
		}

		prevPos = Position;
	}


private void LoadMinawanData()
{
	Dictionary data = DataManager.Load("minawan_settings");

	if (data == null)
	{
		SaveMinawanData();
		return;
	}

	PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.DeclaringType == GetType()).ToArray();
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
	PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.DeclaringType == GetType()).ToArray();

	foreach (PropertyInfo property in properties) data.Add(property.Name, (float)property.GetValue(this));

	DataManager.Save(data, "minawan_settings");
}


	private void UpdateInteractablePolygon()
	{
		Vector2[] newPoints = new Vector2[interactablePolygon.Length];

		for (int i = 0; i < newPoints.Length; i++) newPoints[i] = Position + interactablePolygon[i];
		
		window.MousePassthroughPolygon = newPoints;
	}


	private void GenerateInteractionPolygon()
	{
		Vector2 spriteHalfSideLength = SpriteFrames.GetFrameTexture("default", 0).GetSize() * MinaScale / 2;

		interactablePolygon = new Vector2[] {
			spriteHalfSideLength * -1,
			new Vector2(spriteHalfSideLength.X, -spriteHalfSideLength.Y),
			spriteHalfSideLength,
			new Vector2(-spriteHalfSideLength.X, spriteHalfSideLength.Y)
		};
	}


	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("RightClick")) actionMenu.Popup();
		if (@event.IsActionPressed("LeftClick"))
		{
			switch (action)
			{
				case SelectedAction.Wan:
					wanWanSFX.Play();
					break;

				case SelectedAction.Pat:
					patPat.Visible = true;
					patPat.Play();
					break;
			}
		}
		else if (@event.IsActionReleased("LeftClick"))
		{
			switch (action)
			{
				case SelectedAction.Pat:
					patPat.Visible = false;
					patPat.Stop();
					break;
			}
		}
	}
}
