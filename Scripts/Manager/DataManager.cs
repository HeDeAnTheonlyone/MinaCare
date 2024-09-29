using Godot;
using Godot.Collections;



public class DataManager
{
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


    //TODO: 
    // - Init all Minawan stats
    // - 
    public static void NewGame()
    {

    }
}