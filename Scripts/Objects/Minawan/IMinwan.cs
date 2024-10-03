using System;
using System.Linq;
using Godot;



public interface IMinawan
{
    protected static void UseRandomMinawanTexture(SpriteFrames spriteFrames)
    {
        DirAccess minawanCollectionDir = DirAccess.Open("./Minawan");

        if (minawanCollectionDir == null) return;

        string[] availableMinawans = minawanCollectionDir.GetDirectories();
        var rng = new Random();
        string minawan = availableMinawans[rng.Next(0, availableMinawans.Length)];

        string[] texturesFiles = DirAccess.Open($"./Minawan/{minawan}").GetFiles();

        if (!(texturesFiles.Contains("Stand.png") && texturesFiles.Contains("Walk.png"))) return;

        spriteFrames.SetFrame("default", 0, ImageTexture.CreateFromImage(Image.LoadFromFile($"./Minawan/{minawan}/Stand.png")));
        spriteFrames.SetFrame("default", 1, ImageTexture.CreateFromImage(Image.LoadFromFile($"./Minawan/{minawan}/Walk.png")));
    }
}