using Godot;

public partial class Orber : Sprite3D
{
	public override void _Ready()
	{
		Tween tween = CreateTween().SetLoops();

		tween.TweenProperty(this, "position", new Vector3(0, 7, 0), 2.5).AsRelative().SetTrans(Tween.TransitionType.Sine);
		tween.TweenProperty(this, "position", new Vector3(0, -7, 0), 2.5).AsRelative().SetTrans(Tween.TransitionType.Sine);
	}
}
