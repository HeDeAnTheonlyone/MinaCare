using System;
using Godot;



[GlobalClass]
public partial class AnimatedTextureRect : TextureRect
{
	[Export] public SpriteFrames SpriteFrames { get; set; }
	private string animation = "default";
	[Export]
	public string Animation
	{
		get => animation;
		set
		{
			animation = value;
			Texture = SpriteFrames.GetFrameTexture(Animation, Frame);
		}
	}
	[Export] public int Frame { get; set; } = 0;
	[Export(PropertyHint.Range, "0, 100, 0.001, or_greater")] public float SpeedScale { get; set; } = 1;
	[Export] public bool AutoPlay { get; set; } = false;
	[Export] public bool Looping { get; set; } = true;
	[Export] public bool Playing { get; set; } = false;

	private double refreshRate = 1;
	private double fps = 30;
	private double frameDelta = 0;




	public override void _Ready()
	{
		if (AutoPlay) Play();
	}



	public override void _Process(double delta)
	{
		if (SpriteFrames != null && Playing)
		{
			if (!SpriteFrames.HasAnimation(Animation))
				throw new ArgumentException($"The Animation '{Animation}' in '{GetPath()}' does not exist!");

			frameDelta += SpeedScale * delta;
			if (frameDelta >= refreshRate / fps) Texture = GetNextFrame();
		}
	}



	private Texture2D GetNextFrame()
	{
		Frame++;

		if (Frame >= SpriteFrames.GetFrameCount(Animation))
		{
			Frame = 0;
			if (!Looping) Playing = false;
		}

		refreshRate = SpriteFrames.GetFrameDuration(Animation, Frame);
		frameDelta = 0;

		return SpriteFrames.GetFrameTexture(Animation, Frame);
	}


	/// <summary>
	/// Plays the animation with key name.
	/// If this method is called with that same animation name, or with no name parameter, the assigned animation will resume playing if it was paused.
	/// </summary>
	/// <param name="animationName"></param>
	public void Play(string animationName = null)
	{
		Frame = 0;
		frameDelta = 0;

		if (animationName != null) Animation = animationName;

		fps = SpriteFrames.GetAnimationSpeed(Animation);
		refreshRate = SpriteFrames.GetFrameDuration(Animation, Frame);
		Playing = true;
	}



	public void Resume() => Playing = true;



	public void Pause() => Playing = false;



	public void Stop()
	{
		Frame = 0;
		Playing = false;
	}
}