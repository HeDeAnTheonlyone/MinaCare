using Godot;

public partial class Settings : Control
{
	private Window window;



	public override void _Ready()
	{
		SetUpWindow();
	}


	private void SetUpWindow()
	{
		window = GetWindow();
		window.Title = TranslationServer.Translate("GENERAL_SETTINGS");
	}
}
