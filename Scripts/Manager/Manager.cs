using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class Manager : Node
{
    public static Manager Singleton { get; private set; }
    public static Dictionary Settings { get; private set; } = new Dictionary
    {
        { "show_splash_screen", true },
        { "language", TranslationServer.GetLocale() }
    };
    public static List<string> LastScenes = new List<string>();


    public override void _Ready()
    {
        SingletonSetup();
        LoadSettings();
    }


    private void SingletonSetup() {
        if (Singleton != null) QueueFree();

        Singleton = this;
        ProcessMode = ProcessModeEnum.Always;
        SetProcess(false);
        SetPhysicsProcess(false);

        Engine.MaxFps = 60;
        GetWindow().Unfocusable = true;
    }


    private void SaveSettings() => DataManager.Save(Settings, "general_settings");


    private void LoadSettings()
    {
        Dictionary data = DataManager.Load("general_settings");
        
        if (data == null)
        {
            DataManager.Save(Settings, "general_settings");
            return;
        }

        bool hasUnknownKey = false;

        foreach (string key in Settings.Keys)
        {
            if (!data.ContainsKey(key))
            {
                hasUnknownKey = true;
                continue;
            }

            Settings[key] = data[key];
        }

        if (hasUnknownKey) SaveSettings();

        TranslationServer.SetLocale((string)Settings["language"]);
    }


    public static void ApplyDefaultWindowSettings()
    {
        Window window = Singleton.GetWindow();
        int primaryScreen = DisplayServer.GetPrimaryScreen();
        Vector2I screenOrigin = DisplayServer.ScreenGetPosition(primaryScreen);
        Vector2I screenSize = DisplayServer.ScreenGetSize(primaryScreen);
        window.Size = screenSize / 2;
        window.Position = screenOrigin + screenSize / 4;
        window.Borderless = false;
        window.Unresizable = false;
        window.TransparentBg = false;
        window.Transparent =
        window.Unfocusable = false;
        window.AlwaysOnTop = false;
        window.MousePassthroughPolygon = new Vector2[] { };
        window.ContentScaleMode =  Window.ContentScaleModeEnum.CanvasItems;
        window.ContentScaleAspect =  Window.ContentScaleAspectEnum.Expand;
    }


    public static void SwitchScene(string sceneName)
    {
        LastScenes.Add(Singleton.GetTree().Root.GetChildren().First(node => node.Name != "Manager").Name);
        if (LastScenes.Count > 20) LastScenes.RemoveAt(0);

        Singleton.GetTree().CallDeferred("change_scene_to_file", $"res://Scenes/{sceneName}.tscn");
    }
}
