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


	private void OnMinawanSettingsPressed()
	{
		AlwaysOnTop = false;
		Manager.SwitchScene("MinawanSettings");
	}


	private void OnSettingsPressed()
	{
		AlwaysOnTop = false;
		Manager.SwitchScene("Settings");
	}


	private void TEMP()
	{
		AlwaysOnTop = false;
		Manager.SwitchScene("StatusPanel");
	}


	[Signal] public delegate void RequestChangeMinawanEventHandler();
	private void OnChangeMinawanPressed() => EmitSignal(SignalName.RequestChangeMinawan);
}
