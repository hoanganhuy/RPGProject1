using UnityEngine;

[CreateAssetMenu(menuName = "Game/NPC")]
public class NPCDataSO : ScriptableObject
{
    public string id;
    public string displayName;
    public Sprite portrait;

    [TextArea(3, 6)]
    public string defaultDialogue;
}