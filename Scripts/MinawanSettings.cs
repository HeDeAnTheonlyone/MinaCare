using System.Linq;
using Godot;
using Godot.Collections;

public partial class MinawanSettings : Control
{
	private LineEdit leMinaScale;
	private HSlider hsMinaScale;
	private LineEdit leMaxSpeed;
	private HSlider hsMaxSpeed;
	private LineEdit leAcceleration;
	private HSlider hsAcceleration;
	private LineEdit leDecelerationDistance;
	private HSlider hsDecelerationDistance;



	public override void _Ready()
	{
		leMinaScale = GetNode<LineEdit>("MarginContainer/ScrollContainer/SettingsList/Scale/HSplitContainer/LineEdit");
		hsMinaScale = GetNode<HSlider>("MarginContainer/ScrollContainer/SettingsList/Scale/HSplitContainer/HSlider");

		leMaxSpeed = GetNode<LineEdit>("MarginContainer/ScrollContainer/SettingsList/MaxSpeed/HSplitContainer/LineEdit");
		hsMaxSpeed = GetNode<HSlider>("MarginContainer/ScrollContainer/SettingsList/MaxSpeed/HSplitContainer/HSlider");
		
		leAcceleration = GetNode<LineEdit>("MarginContainer/ScrollContainer/SettingsList/Acceleration/HSplitContainer/LineEdit");
		hsAcceleration = GetNode<HSlider>("MarginContainer/ScrollContainer/SettingsList/Acceleration/HSplitContainer/HSlider");
		
		leDecelerationDistance = GetNode<LineEdit>("MarginContainer/ScrollContainer/SettingsList/DecelerationDistance/HSplitContainer/LineEdit");
		hsDecelerationDistance = GetNode<HSlider>("MarginContainer/ScrollContainer/SettingsList/DecelerationDistance/HSplitContainer/HSlider");

		SetUpWindow();
		LoadSettings();
	}


	private void SetUpWindow()
	{
		GetWindow().Title = TranslationServer.Translate("MINAWAN_SETTINGS");
	}


	private void LoadSettings()
	{
		string dataString;

		using (FileAccess file = FileAccess.Open("user://minawan_settings.json", FileAccess.ModeFlags.Read))
		{
			dataString = file.GetAsText();
		}

		Dictionary data = (Dictionary)Json.ParseString(dataString);

		//TODO implement a method that uses reflection 
		foreach (string key in data.Keys.ToArray())
		{
			switch(key)
			{
				case "MinaScale":
					hsMinaScale.Value = (float)data[key];
					break;

				case "MaxSpeed":
					hsMaxSpeed.Value = (float)data[key];
					break;

				case "Acceleration":
					hsAcceleration.Value = (float)data[key];
					break;

				case "DecelerationDistance":
					hsDecelerationDistance.Value = (float)data[key];
					break;
			}
		}
	}


	private void SaveSetings()
	{
		Dictionary data = new Dictionary
		{
			{ "MinaScale", hsMinaScale.Value },
			{ "MaxSpeed", hsMaxSpeed.Value },
			{ "Acceleration", hsAcceleration.Value },
			{ "DecelerationDistance", hsDecelerationDistance.Value }
		};

		using (FileAccess file = FileAccess.Open("user://minawan_settings.json", FileAccess.ModeFlags.Write))
		{
			file.StoreString(Json.Stringify(data, "\t"));
		}
	}


	private void OnMinaScaleChange(float value)	=> leMinaScale.Text = value.ToString();


	private void OnMaxSpeedChange(float value)	=> leMaxSpeed.Text = value.ToString();
	
	
	private void OnAccelerationChange(float value)	=> leAcceleration.Text = value.ToString();


	private void OnDecelerationDistanceChange(float value)	=> leDecelerationDistance.Text = value.ToString();


	private void OnBackPressed()
	{
		SaveSetings();
		Manager.SwitchScene("Walkies");
	}
}
