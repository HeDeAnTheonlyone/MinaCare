using Godot;

public partial class Settings : Control
{
	private CheckButton splashScreen;
	private OptionButton language;
	private string[] language_codes;



	public override void _Ready()
	{
		splashScreen = GetNode<CheckButton>("MarginContainer/ScrollContainer/OptionsList/SplashScreen/Input");
		language = GetNode<OptionButton>("MarginContainer/ScrollContainer/OptionsList/Language/Input");
		language_codes = TranslationServer.GetLoadedLocales();

		SetUpWindow();
		FetchSettings();
	}


	private void SetUpWindow()
	{
		GetWindow().Title = TranslationServer.Translate("GENERAL_SETTINGS");
		Manager.ApplyDefaultWindowSettings();
	}


	private void FetchSettings()
	{
		splashScreen.ButtonPressed = (bool)Manager.Settings["show_splash_screen"];
		
		string[] allLanguages = TranslationServer.GetLoadedLocales();
		for (int i = 0; i < allLanguages.Length; i++)
		{
			language.AddItem(TranslationServer.GetLanguageName(allLanguages[i]));
			if ((string)Manager.Settings["language"] == language_codes[i]) language.Selected = i;
		}
	}


	private void SaveSettings() => DataManager.Save(Manager.Settings, "general_settings");


	private void OnSplashScreenToggle(bool value) => Manager.Settings["show_splash_screen"] = value;


	private void OnLanguageSelected(int index)
	{
		Manager.Settings["language"] = language_codes[index];
		TranslationServer.SetLocale(language_codes[index]);
		GetTree().ReloadCurrentScene();
	}


	private void OnBackPressed()
	{
		SaveSettings();
		Manager.SwitchScene("Walkies");
	}
}
