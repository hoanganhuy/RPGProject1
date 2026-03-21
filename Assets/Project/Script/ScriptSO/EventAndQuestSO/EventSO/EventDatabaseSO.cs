using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Event Database")]
public class EventDatabaseSO : ScriptableObject
{
    public List<EventSO> travelEvents;
    public List<EventSO> locationEvents;
    public List<EventSO> npcEvents;
}