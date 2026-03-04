using System.Collections.Generic;

[System.Serializable]
public class GameState
{
    public string currentLocationID;

    public HashSet<string> flags = new HashSet<string>();

    public HashSet<string> completedEventIDs = new HashSet<string>();
}