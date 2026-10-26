//  The actual schema for the user save
using System.Text.Json;
using System;
using Godot;
public class UserSave
{
    public int Xp { get; set; }
    public string username { get; set; }
    public int money { get; set; }
    public int deaths { set; get; }
    public int wins { set; get; }
}
// Save manager - has a predefined save path which it checks for
public static class SaveManager
{
    // Save Path
    private const string SavePath = "user://save_file.json";
    // Next we implement these 3 functions: check,load,save/create
    //
    // The check save exists functionm
    public static bool SaveExists()
    {
        if (Godot.FileAccess.FileExists(SavePath))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    // Load Save
    //
    public static UserSave LoadSave()
    {
        UserSave emptyUserSave = new UserSave {Xp= 0, deaths=0, username="", money=0, wins=0 };
        // 1. Check if save exists
        if (!SaveExists())
        {
            return  emptyUserSave;
        }
        else
        {
            // If the save file exists we can read its content
            // I had to search up the file actions cause i dont really arguments and stuff
            using var file = Godot.FileAccess.Open(SavePath, Godot.FileAccess.ModeFlags.Read);
            if (file == null) { GD.PrintErr(Godot.FileAccess.GetOpenError()); return emptyUserSave; }
            string JsonFromLoadFile = file.GetAsText();
            try
            {
                UserSave DeserializedJson = JsonSerializer.Deserialize<UserSave>(JsonFromLoadFile);
                return DeserializedJson ?? emptyUserSave;
            }
            catch (JsonException)
            {
                GD.PrintErr("Save file is corrupt");
                return emptyUserSave;
            }
        }
    }
    //  Create/update save file
    // Ok so lets define codes -
    // 101 - no save file
    // 202 - Save error
    // 201 - success
    public static int Save(UserSave PlayerProfile){
        // Lets Write to the file
        using var file = Godot.FileAccess.Open(SavePath, Godot.FileAccess.ModeFlags.Write);
        if (file == null) { GD.PrintErr(Godot.FileAccess.GetOpenError()); return 202; }
        file.StoreString(JsonSerializer.Serialize(PlayerProfile));
        return 201;
    }


}
