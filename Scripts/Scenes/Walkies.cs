using Godot;

public partial class Walkies : Node2D
{
    public override void _EnterTree()
    {
		SetUpWindow();
	}


    private void SetUpWindow()
	{
		Window window = GetWindow();

		Vector2I windowSize = Vector2I.Zero;
		for (int i = 0; i < DisplayServer.GetScreenCount(); i++) windowSize += DisplayServer.ScreenGetSize(i);
		window.Position = Vector2I.Zero;
		window.Size = windowSize;
		window.AlwaysOnTop = true;
		window.Borderless = true;
		window.Unresizable = true;
		window.TransparentBg = true;
		window.Transparent =
		window.Unfocusable = true;
		window.Unfocusable = true;
		window.ContentScaleMode = Window.ContentScaleModeEnum.Disabled;
		window.ContentScaleAspect = Window.ContentScaleAspectEnum.Keep;
		window.Title = TranslationServer.Translate("WALKIES");
	}
}
