using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class Manager : Node
{
    public static Manager Singleton { get; private set; }
    public static Dictionary Settings { get; private set; } = new Dictionary
    {
        //TODO change to true 
        { "show_splash_screen", false },
        { "language", TranslationServer.GetLocale() }
    };
    public static Stack<string> LastScenes = new Stack<string>();



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


    private void SaveSettings() => Save(Settings, "general_settings");


    private void LoadSettings()
    {
        Dictionary data = Load("general_settings");
        
        if (data == null)
        {
            Save(Settings, "general_settings");
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


    /// <summary>
    /// Loads the file of the given name and returns the data as a dictionary.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns>Data as dictionary or null if it fails.</returns>
    public static Dictionary Load(string fileName)
    {
        string dataString;

        using (FileAccess file = FileAccess.Open($"user://{fileName}.json", FileAccess.ModeFlags.Read))
        {
            if (FileAccess.GetOpenError() != Error.Ok) return null;

            dataString = file.GetAsText();
        }

        return (Dictionary)Json.ParseString(dataString);
    }


    public static void Save(Dictionary data, string fileName)
    {
        using (FileAccess file = FileAccess.Open($"user://{fileName}.json", FileAccess.ModeFlags.Write))
        {
            file.StoreString(Json.Stringify(data, "\t"));
        }
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
        LastScenes.Push(Singleton.GetTree().Root.GetChildren().First(node => node.Name != "Manager").Name);
        //TODO add a check to prevent a longer history than 100 entries

        Singleton.GetTree().CallDeferred("change_scene_to_file", $"res://Scenes/{sceneName}.tscn");
    }
}
