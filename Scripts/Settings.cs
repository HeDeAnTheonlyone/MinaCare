using Godot;

public partial class Settings : Control
{
	private CheckButton splashScreen;
	private OptionButton language;



	public override void _Ready()
	{
		splashScreen = GetNode<CheckButton>("MarginContainer/ScrollContainer/VBoxContainer/SplashScreen");
		// language = GetNode<OptionButton>("MarginContainer/ScrollContainer/VBoxContainer/Language");
		
		SetUpWindow();
		LoadSettings();
	}


	private void SetUpWindow()
	{
		GetWindow().Title = TranslationServer.Translate("GENERAL_SETTINGS");
	}


	private void LoadSettings()
	{
		splashScreen.ButtonPressed = (bool)Manager.Settings["show_splash_screen"];
		
		// string[] allLanguages = TranslationServer.GetLoadedLocales();
		
		// for (int i = 0; i < allLanguages.Length; i++)
		// {
		// 	language.AddItem(allLanguages[i]);
		// 	// if ((string)Manager.Settings["language"] == allLanguages[i]) language.Selected = i;
		// }
	}


	private void SaveSettings()
	{
		using(FileAccess file = FileAccess.Open("user://general_settings.json", FileAccess.ModeFlags.Write))
		{
			file.StoreString(Json.Stringify(Manager.Settings, "\t"));
		}
	}


	private void OnSplashScreenToggle(bool value) => Manager.Settings["show_splash_screen"] = value;


	private void OnLanguageSelected(int index)
	{
		string newLanguage = language.GetItemText(index);
		Manager.Settings["language"] = newLanguage;
		TranslationServer.SetLocale(newLanguage);
	}


	private void OnBackPressed()
	{
		SaveSettings();
		Manager.SwitchScene("Walkies");
	}
}
