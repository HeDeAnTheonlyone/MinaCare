using Godot;
using Godot.Collections;

public partial class Manager : Node
{
    public static Manager Singleton { get; private set; }
    public static Dictionary Settings { get; private set; } = new Dictionary
    {
        { "show_splash_screen", true }
    };



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

    public void SwitchScene(string sceneName) => GetTree().ChangeSceneToFile($"res://Scenes/{sceneName}.tscn"); 
}
