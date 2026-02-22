using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text locationText;
    public TMP_Text goodsText;
    public TMP_Text coinText;
    public Button mainButton;   // "Take Quest" / "Continue" / "Complete"
    public GameObject eventPanel;
    public TMP_Text eventDescText;
    public Button[] choiceButtons;  // 3 buttons

    void Awake() => Instance = this;

    public void UpdateUI()
    {
        locationText.text = GameManager.Instance.currentLocation?.locationName ?? "None";
        goodsText.text = $"Bread: {GameManager.Instance.goods}";
        coinText.text = $"Coins: {GameManager.Instance.coins}";
    }

    public void ShowEventPanel(EventData evt)
    {
        eventPanel.SetActive(true);
        eventDescText.text = evt.description;
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < evt.choices.Count)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = evt.choices[i].buttonText;
                // Remove old listeners and add new one
                choiceButtons[i].onClick.RemoveAllListeners();
                int index = i; // capture for closure
                choiceButtons[i].onClick.AddListener(() => {
                    EventManager.Instance.ExecuteChoice(evt.choices[index]);
                    eventPanel.SetActive(false);
                });
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShowEventResult(string resultText)
    {
        // Optionally show a temporary message
        Debug.Log(resultText);
        UpdateUI();
    }

    public void OnArrival()
    {
        mainButton.GetComponentInChildren<TMP_Text>().text = "Complete Quest";
        mainButton.onClick.RemoveAllListeners();
        mainButton.onClick.AddListener(() => FindObjectOfType<QuestManager>().CompleteQuest());
        UpdateUI();
    }
}