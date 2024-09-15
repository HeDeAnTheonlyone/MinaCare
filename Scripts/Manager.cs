using Godot;

public partial class Manager : Node
{
    public static Manager Singleton { get; private set; }

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
}
