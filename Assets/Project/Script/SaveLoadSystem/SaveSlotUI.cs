using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SaveSlotUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI repText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI saveNameText;

    public Button deleteButton;

    int slotIndex;
    SaveLoadUI parent;

    string GetPath()
    {
        return Application.persistentDataPath + "/save_" + slotIndex + ".json";
    }

    public void Setup(int index, SaveLoadUI parentUI)
    {
        slotIndex = index;
        parent = parentUI;

        if (File.Exists(GetPath()))
        {
            LoadPreview();
            deleteButton.gameObject.SetActive(true);
        }
        else
        {
            ShowEmpty();
            deleteButton.gameObject.SetActive(false);
        }
    }
    void ShowEmpty()
    {
        coinText.text = "-";
        repText.text = "-";
        dayText.text = "Empty";
        timeText.text = "--:--";
        saveNameText.text = "Empty Slot";
    }
    void LoadPreview()
    {
        string json = File.ReadAllText(GetPath());
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        coinText.text = data.coin.ToString();
        repText.text = data.reputation.ToString();
        dayText.text = "Day " + data.day;

        int hour = Mathf.FloorToInt(data.hour);
        int minute = Mathf.FloorToInt((data.hour - hour) * 60);

        timeText.text = $"{hour:00}:{minute:00}";

        saveNameText.text = data.currentLocationId; // sau này có thể đổi đẹp hơn
    }
    public void OnClickSlot()
    {
        if (parent.mode == SaveLoadMode.Save)
        {
            SaveSystem.Instance.SaveGame(slotIndex);
            parent.RefreshAllSlots();
        }
        else
        {
            if (!File.Exists(GetPath()))
            {
                Debug.Log("Empty slot!");
                return;
            }

            SaveSystem.Instance.LoadGame(slotIndex);
            parent.gameObject.SetActive(false);
        }
    }
    public void OnClickDelete()
    {
        if (File.Exists(GetPath()))
        {
            File.Delete(GetPath());
        }

        Setup(slotIndex, parent);
    }
}