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
        { "language", TranslationServer.GetLocale()}
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


    private void LoadSettings()
    {
        string dataString;

        using (FileAccess file = FileAccess.Open("user://general_settings.json", FileAccess.ModeFlags.Read))
        {
            if (FileAccess.GetOpenError() != Error.Ok)
            {
                SaveSettings();
                return;
            }

            dataString = file.GetAsText();
        }

        Settings = (Dictionary)Json.ParseString(dataString);
    }


    private void SaveSettings()
    {
        using (FileAccess file = FileAccess.Open("user://general_settings.json", FileAccess.ModeFlags.Write))
        {
            file.StoreString(Json.Stringify(Settings, "\t"));
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
        window.TransparentBg = false;
        window.Transparent =
        window.Unfocusable = false;
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
