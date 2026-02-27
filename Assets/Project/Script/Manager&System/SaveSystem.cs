using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class GameData
{
    // Add your game data fields here
    public int playerScore;
    public float playerHealth;
    // etc.
}

public static class SaveSystem
{
    public static void SaveGame(int slot, GameData data)
    {
        string path = Path.Combine(Application.persistentDataPath, $"save_slot{slot}.dat");
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }
    }

    public static GameData LoadGame(int slot)
    {
        string path = Path.Combine(Application.persistentDataPath, $"save_slot{slot}.dat");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                return formatter.Deserialize(stream) as GameData;
            }
        }
        else
        {
            Debug.LogError("Save file not found in slot " + slot);
            return null;
        }
    }
}