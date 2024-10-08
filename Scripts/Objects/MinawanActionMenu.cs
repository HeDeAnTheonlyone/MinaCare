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


	private void OnMapPressed()
	{
		AlwaysOnTop = false;
		Manager.SwitchScene("Map");
	}


	[Signal] public delegate void RequestChangeMinawanEventHandler();
	private void OnChangeMinawanPressed() => EmitSignal(SignalName.RequestChangeMinawan);


	[Signal] public delegate void SelectWanActionEventHandler();
	private void OnWanActionPressed() => EmitSignal(SignalName.SelectWanAction);


	[Signal] public delegate void SelectPatActionEventHandler();
	private void OnPatActionPressed() => EmitSignal(SignalName.SelectPatAction);
}
