using UnityEngine;
public enum QuestType
{
    Delivery,
    Collection,
    Transfer,
    Service,
    Special
}
[CreateAssetMenu(menuName = "Game/Quest")]
public class QuestSO : ScriptableObject
{
    public string id;

    public string title;

    [TextArea(4, 10)]
    public string description;

    public QuestType type;

    public string giverNPC;
    public LocationData receiveLocation;

    public QuestStep[] steps;

    public int rewardCoin;
    public int rewardRep;

    public bool repeatable;
}