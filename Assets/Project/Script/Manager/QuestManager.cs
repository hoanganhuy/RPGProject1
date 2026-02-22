using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public QuestData testQuest;  // drag Mom'sDelivery asset here in Inspector

    public void AcceptQuest()
    {
        GameManager.Instance.activeQuest = testQuest;
        GameManager.Instance.currentLocation = testQuest.startLocation;
        GameManager.Instance.goods = 9;
        UIManager.Instance.UpdateUI();
        TravelManager.Instance.SetCurrentLocation(testQuest.startLocation);
    }

    public void CompleteQuest()
    {
        if (GameManager.Instance.currentLocation != GameManager.Instance.activeQuest.destination)
        {
            Debug.Log("Not at destination!");
            return;
        }

        string dialogue;
        if (GameManager.Instance.goods >= 9)
            dialogue = GameManager.Instance.activeQuest.completionDialogueHappy;
        else
            dialogue = GameManager.Instance.activeQuest.completionDialogueSad;

        UIManager.Instance.ShowEventResult(dialogue);
        GameManager.Instance.coins += GameManager.Instance.activeQuest.rewardCoins;
        GameManager.Instance.activeQuest = null;
        UIManager.Instance.UpdateUI();
    }
}