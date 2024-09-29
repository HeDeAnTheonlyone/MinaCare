using Godot;

public partial class StatusPanel : MarginContainer
{
	private TextureProgressBar snack;
	private TextureProgressBar fwootPunch;
	private TextureProgressBar prayers;
	private TextureProgressBar walkies;
	private TextureProgressBar tomfoolery;



	public override void _Ready()
	{
		snack = GetNode<TextureProgressBar>("BarList/Snacks/Bar");
		fwootPunch = GetNode<TextureProgressBar>("BarList/FwootPunch/Bar");
		prayers = GetNode<TextureProgressBar>("BarList/Prayers/Bar");
		walkies = GetNode<TextureProgressBar>("BarList/Walkies/Bar");
		tomfoolery = GetNode<TextureProgressBar>("BarList/Tomfoolery/Bar");

		Manager.ApplyDefaultWindowSettings();
		AppliyValues();
	}


	private void AppliyValues()
	{
		snack.Value = Manager.MinawanStats.Snacks;
		fwootPunch.Value = Manager.MinawanStats.FwootPunch;
		prayers.Value = Manager.MinawanStats.Prayer;
		walkies.Value = Manager.MinawanStats.Walkies;
		tomfoolery.Value = Manager.MinawanStats.Tomfoolery;
	}
}
