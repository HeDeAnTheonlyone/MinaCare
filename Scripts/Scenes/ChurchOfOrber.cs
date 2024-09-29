using System;
using Godot;

public partial class ChurchOfOrber : Node3D
{
	public override void _Ready()
	{
		SetUpWindow();
		MoveDummyMinawan();
	}


	private void SetUpWindow()
	{
		GetWindow().Title = TranslationServer.Translate("CHURCH_OF_ORBER");
		Manager.ApplyDefaultWindowSettings();
	}


	private void MoveDummyMinawan()
	{
		Tween tween = CreateTween().SetParallel();
		Random rng = new Random();

		foreach (Path3D path in GetNode<Node3D>("WalkPaths").GetChildren())
		{
			PathFollow3D pathFollow = path.GetNode<PathFollow3D>("MinaFollow");

			tween.TweenProperty(pathFollow, "progress_ratio", 1, 3 + 0.1 * rng.Next(0, 21)).SetDelay(0.1 * rng.Next(0, 21));
		}
	}
}
