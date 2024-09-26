using Godot;
using System;
using System.Linq;



public partial class ChurchMinawan : AnimatedSprite3D
{
	PathFollow3D minaPath;

	public override void _Ready()
	{
		minaPath = GetParent<PathFollow3D>();

		UseRandomMinawanTexture();
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


    private void UseRandomMinawanTexture()
	{
		DirAccess minawanCollectionDir = DirAccess.Open("./Minawan");

		if (minawanCollectionDir == null) return;

		string[] availableMinawans = minawanCollectionDir.GetDirectories();
		var rng = new Random(Guid.NewGuid().ToByteArray()[0]);
		
		string minawan = availableMinawans[rng.Next(0, availableMinawans.Length)];

		string[] texturesFiles = DirAccess.Open($"./Minawan/{minawan}").GetFiles();
		
		if (!(texturesFiles.Contains("Stand.png") && texturesFiles.Contains("Walk.png"))) return;

		SpriteFrames.SetFrame("default", 0, ImageTexture.CreateFromImage(Image.LoadFromFile($"./Minawan/{minawan}/Stand.png")));
		SpriteFrames.SetFrame("default", 1, ImageTexture.CreateFromImage(Image.LoadFromFile($"./Minawan/{minawan}/Walk.png")));
	}
}
