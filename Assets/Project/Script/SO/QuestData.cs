using NUnit.Framework.Interfaces;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Game/Quest")]
public class QuestData : ScriptableObject
{
    public string questName;
    public string giverName;
    public LocationData startLocation;    // where player accepts
    public LocationData destination;      // where to deliver
    //public ItemData requiredItem;         // later
    public int rewardCoins;
    [TextArea] public string description;
    [TextArea] public string completionDialogueHappy;
    [TextArea] public string completionDialogueSad;
}