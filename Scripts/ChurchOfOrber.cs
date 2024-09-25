using Godot;

public partial class ChurchOfOrber : Node3D
{
	public override void _Ready()
	{
	}

	private void SetUpWindow()
	{
		GetWindow().Title = TranslationServer.Translate("CHURCH_OF_ORBER");
		Manager.ApplyDefaultWindowSettings();
		GetTree().ReloadCurrentScene();
	}
}
