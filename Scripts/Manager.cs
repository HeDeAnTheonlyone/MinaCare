using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class Manager : Node
{
    public static Manager Singleton { get; private set; }
    public static Dictionary Settings { get; private set; } = new Dictionary
    {
        //TODO change to true 
        { "show_splash_screen", false }
    };
    public static Stack<string> LastScenes = new Stack<string>();



    public override void _Ready()
    {
        SingletonSetup();
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


    public static void ApplyDefaultWindowSettings(Window window)
    {
        int primaryScreen = DisplayServer.GetPrimaryScreen();
        Vector2I screenOrigin = DisplayServer.ScreenGetPosition(primaryScreen);
        Vector2I screenSize = DisplayServer.ScreenGetSize(primaryScreen);
        window.Size = screenSize / 2;
        window.Position = screenOrigin + screenSize / 4;
        window.Borderless = false;
        window.TransparentBg = false;
        window.Transparent = false;
        window.MousePassthroughPolygon = new Vector2[] { };
        window.Unfocusable = false;
        window.ContentScaleMode =  Window.ContentScaleModeEnum.CanvasItems;
        window.ContentScaleAspect =  Window.ContentScaleAspectEnum.Expand;
    }


    public void SwitchScene(string sceneName) => GetTree().ChangeSceneToFile($"res://Scenes/{sceneName}.tscn");
}
