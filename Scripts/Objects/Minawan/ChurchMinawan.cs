using Godot;



public partial class ChurchMinawan : AnimatedSprite3D, IMinawan
{
	PathFollow3D minaPath;

	public override void _Ready()
	{
		minaPath = GetParent<PathFollow3D>();

		IMinawan.UseRandomMinawanTexture(SpriteFrames);
		Play();
	}


    public override void _Process(double delta)
    {
        if (IsPlaying())
		{
			if (minaPath.ProgressRatio == 1)
			{
				Stop();
				Timer timer = new Timer();
				AddChild(timer);
				timer.Start(0.2);
				timer.Timeout += () => {
					GetNode<Sprite3D>("Pray").Visible = true;
					timer.QueueFree();
				};
			}
		}
    }
}
