using System;
using System.Linq;
using System.Reflection;
using Godot;
using Godot.Collections;

public partial class MinawanSettings : Control
{
    private float minaScale;
    private float MinaScale { get => minaScale; set => minaScale = Math.Clamp(value, 0.05f, 2f); }
	private float maxSpeed;
    private float MaxSpeed { get => maxSpeed; set => maxSpeed = Math.Clamp(value, 0f, 1000f); }
	private float acceleration;
    private float Acceleration { get => acceleration; set => acceleration = Math.Clamp(value, 0f, 100f); }
	private float decelerationDistance;
    private float DecelerationDistance { get => decelerationDistance; set => decelerationDistance = Math.Clamp(value, 0f, 1000f); }

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
		leMinaScale = GetNode<LineEdit>("MarginContainer/ScrollContainer/SettingsList/Scale/Input/LineEdit");
		hsMinaScale = GetNode<HSlider>("MarginContainer/ScrollContainer/SettingsList/Scale/Input/HSlider");

		leMaxSpeed = GetNode<LineEdit>("MarginContainer/ScrollContainer/SettingsList/MaxSpeed/Input/LineEdit");
		hsMaxSpeed = GetNode<HSlider>("MarginContainer/ScrollContainer/SettingsList/MaxSpeed/Input/HSlider");
		
		leAcceleration = GetNode<LineEdit>("MarginContainer/ScrollContainer/SettingsList/Acceleration/Input/LineEdit");
		hsAcceleration = GetNode<HSlider>("MarginContainer/ScrollContainer/SettingsList/Acceleration/Input/HSlider");
		
		leDecelerationDistance = GetNode<LineEdit>("MarginContainer/ScrollContainer/SettingsList/DecelerationDistance/Input/LineEdit");
		hsDecelerationDistance = GetNode<HSlider>("MarginContainer/ScrollContainer/SettingsList/DecelerationDistance/Input/HSlider");

		SetUpWindow();
		FetchSettings();

		OnMinaScaleChange(MinaScale);
		OnMaxSpeedChange(MaxSpeed);
		OnAccelerationChange(Acceleration);
		OnDecelerationDistanceChange(DecelerationDistance);
	}


	private void SetUpWindow()
	{
		GetWindow().Title = TranslationServer.Translate("MINAWAN_SETTINGS");
		Manager.ApplyDefaultWindowSettings();
	}


	private void FetchSettings()
	{
		Dictionary data = DataManager.Load("minawan_settings");

		if (data == null)
		{
			SaveSettings();
			return;
		}

		PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic).Where(p => p.DeclaringType == GetType()).ToArray();
		bool hasUnknownKey = false;

		foreach (PropertyInfo property in properties)
		{
			if (!data.ContainsKey(property.Name))
			{
				hasUnknownKey = true;
				continue;
			}

			GetType().GetProperty(property.Name, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, (float)data[property.Name]);
		}

		if (hasUnknownKey) SaveSettings();
	}


	private void SaveSettings()
	{
		Dictionary data = new Dictionary();
		PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic).Where(p => p.DeclaringType == GetType()).ToArray();

		foreach (PropertyInfo property in properties) data.Add(property.Name, (float)property.GetValue(this));

		DataManager.Save(data, "minawan_settings");
	}


	private void OnMinaScaleChange(float value)
    {
        MinaScale = value;
		hsMinaScale.Value = MinaScale;
		leMinaScale.Text = MinaScale.ToString().Replace(',', '.');
    }


    private void OnMaxSpeedChange(float value)
    {
		MaxSpeed = value;
		hsMaxSpeed.Value = MaxSpeed;
		leMaxSpeed.Text = MaxSpeed.ToString().Replace(',', '.');
    }


    private void OnAccelerationChange(float value)
    {
        Acceleration = value;
		hsAcceleration.Value = Acceleration;
        leAcceleration.Text = Acceleration.ToString().Replace(',', '.');
    }


    private void OnDecelerationDistanceChange(float value)
    {
        DecelerationDistance = value;
		hsDecelerationDistance.Value = DecelerationDistance;
        leDecelerationDistance.Text = DecelerationDistance.ToString().Replace(',', '.');
    }


	private void OnBackPressed()
	{
		SaveSettings();
		Manager.SwitchScene("Walkies");
	}
}
