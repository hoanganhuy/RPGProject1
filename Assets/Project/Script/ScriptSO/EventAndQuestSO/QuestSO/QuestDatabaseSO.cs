using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Quest Database")]
public class QuestDatabaseSO : ScriptableObject
{
    public List<QuestSO> quests;
}