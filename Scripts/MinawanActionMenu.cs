using Godot;

public partial class MinawanActionMenu : Window
{
	private void OnAboutToPopUp()
	{
		Position = DisplayServer.MouseGetPosition();
		Title = TranslationServer.Translate("MINAWAN_ACTION_MENU");
		AlwaysOnTop = true;
	}


	private void OnCloseRequest()
	{
		AlwaysOnTop = false;
		Hide();
	}
}
