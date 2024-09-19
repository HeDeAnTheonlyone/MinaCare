using Godot;

public partial class Splash : Control
{
	public override void _Ready()
	{
		Tween tween = CreateTween();

		if ((bool)Manager.Settings["show_splash_screen"])
		{
			var minaProd = GetNode<TextureRect>("CenterContainer/MinawanProductions");
			var godot = GetNode<TextureRect>("CenterContainer/Godot");

			tween.TweenProperty(godot, "modulate:a", 1, 1.1f);
			tween.TweenProperty(godot, "modulate:a", 0, 1.1f);
			tween.TweenCallback(Callable.From(() => GetNode<AudioStreamPlayer>("YippeeSFX").Play(0.4f)));
			tween.TweenProperty(minaProd, "modulate:a", 1, 1.1f);
			tween.TweenProperty(minaProd, "modulate:a", 0, 1.1f);
		}
		
		tween.TweenCallback(Callable.From(() => Manager.SwitchScene("Walkies")));
	}
}