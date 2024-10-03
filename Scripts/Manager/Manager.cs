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
    public static MinawanStats MinawanStats { get; private set; } = new MinawanStats();


    public override void _Ready()
    {
        SingletonSetup();

        int fps = 60;

        for (int i = 0; i < DisplayServer.GetScreenCount() - 1; i++)
        {
            int newFps = (int)DisplayServer.ScreenGetRefreshRate(i);
            if (newFps > fps) fps = newFps;
        }
        Engine.MaxFps = fps;

        LoadSettings();
        ApplyDefaultWindowSettings();
    }


    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Escape"))
        {
            int count = LastScenes.Count;
            
            if (count > 0)
            {
                SwitchScene(LastScenes.Last());
                LastScenes.RemoveAt(count - 1);
            }
        }
    }


    private void SingletonSetup() {
        if (Singleton != null) QueueFree();

        Singleton = this;
        ProcessMode = ProcessModeEnum.Always;
        SetProcess(false);
        SetPhysicsProcess(false);
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
        string oldScene = Singleton.GetTree().Root.GetChildren().First(node => node.Name != "Manager").Name;
        if (oldScene != "Splash")
        {
            LastScenes.Add(oldScene);
            if (LastScenes.Count > 20) LastScenes.RemoveAt(0);
        }

        Singleton.GetTree().CallDeferred("change_scene_to_file", $"res://Scenes/{sceneName}.tscn");
    }
}
