using System.Linq;
using Godot;

public partial class Map : Node2D
{
	public override void _Ready()
	{
		SetUpWindow();
		
		Node2D currtentLocation = GetNode<Node2D>($"Markers/{Manager.LastScenes.Last()}");
		GetNode<CharacterBody2D>("MapMinawan").Position = currtentLocation.Position;
	}


	private void SetUpWindow()
	{
		GetWindow().Title = TranslationServer.Translate("MAP");
		Manager.ApplyDefaultWindowSettings();
	}
}
