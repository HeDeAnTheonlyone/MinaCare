using System;
using Godot;

public partial class MapMinawan : CharacterBody2D, IMinawan
{
	private AnimatedSprite2D sprite;
	private RichTextLabel enterInfo;

	public float MinaScale { get; set; } = 0.12f;
	public float MaxSpeed { get; set; } = 175f;
	public float Acceleration { get; set; } = 2f;
	public float DecelerationDistance { get; set; } = 100f;
	private float speed;
	private Vector2 prevPos;
	string currentLocation = null;



	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("Sprite");
		enterInfo = GetNode<RichTextLabel>("EnterInfo");
		enterInfo.Text = $"[center]{TranslationServer.Translate("PRESS")} [img=32]res://Assets/Sprites/Icons/PressLeftButton.png[/img] {TranslationServer.Translate("TO ENTER")}[/center]";
		MinaScale = Scale.X;
		IMinawan.UseRandomMinawanTexture(sprite.SpriteFrames);
	}


    public override void _Process(double delta)
    {
        MoveMinawan((float)delta);
		if (currentLocation != null) enterInfo.Visible = true;
		else enterInfo.Visible = false;

		GD.Print(currentLocation);
    }


    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("LeftClick") && currentLocation != null) Manager.SwitchScene(currentLocation);
    }


    private void MoveMinawan(float delta)
	{
		prevPos = Position;
		Vector2 mousePos = GetGlobalMousePosition();

		if (Position.DistanceTo(mousePos) > DecelerationDistance) speed = Math.Clamp(speed + Acceleration, 0, MaxSpeed);
		else speed = Math.Clamp(speed - Acceleration * 2, 0, MaxSpeed);

		MoveAndCollide(Position.MoveToward(mousePos, speed * (float)delta) - Position);
		sprite.FlipH = mousePos.X - Position.X < 0;

		if (Position == prevPos)
		{
			sprite.Stop();
			speed = 0f;
		}
		else sprite.Play();
	}


	private void EnterArea(Area2D area) => currentLocation = area.GetParent().Name;


	private void LeaveArea(Area2D area) => currentLocation = null;
}
