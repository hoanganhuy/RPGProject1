using TMPro;
using UnityEngine;

public class UIDataSystem : MonoBehaviour
{
    public static UIDataSystem Instance;

    public TextMeshProUGUI locationText;
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI questTargetText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI repText;

    void Awake()
    {
        Instance = this;
    }

    public void Refresh()
    {
        var data = GameDataSystem.Instance;

        // ===== DAY =====
        dayText.text = "Day " + data.day;

        // ===== TIME (FLOAT → HH:MM) =====
        int hour = Mathf.FloorToInt(data.hour);
        int minute = Mathf.FloorToInt((data.hour - hour) * 60f);

        timeText.text = $"{hour:00}:{minute:00}";

        // ===== RESOURCE =====
        coinText.text = data.coin.ToString();
        repText.text = data.reputation.ToString();

        // ===== QUEST =====
        var quest = QuestRuntimeSystem.Instance.GetActiveQuest();

        if (quest != null)
        {
            questNameText.text = quest.data.title;

            // 🔥 HIỂN THỊ STEP
            questTargetText.text =
                $"Step {quest.currentStep} / {quest.data.endStep}";
        }
        else
        {
            questNameText.text = "";
            questTargetText.text = "";
        }
    }

    public void UpdateLocation(string loc)
    {
        locationText.text = loc;
    }
}