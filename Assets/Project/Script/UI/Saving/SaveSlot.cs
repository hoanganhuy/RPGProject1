using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveSlot : MonoBehaviour
{
    public int slotIndex;              // Set in Inspector (0,1,2...)
    public Text infoText;               // Displays "Empty" or save time

    private string saveFilePath;

    private void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, $"save_slot{slotIndex}.dat");
        UpdateSlotInfo();
    }

    private void UpdateSlotInfo()
    {
        if (File.Exists(saveFilePath))
        {
            // Load just the header info (e.g., save time) – for simplicity we show "Save exists"
            // You can store a timestamp inside the file and display it here.
            infoText.text = $"Slot {slotIndex + 1} - Saved";
        }
        else
        {
            infoText.text = $"Slot {slotIndex + 1} - Empty";
        }
    }

    public void OnClick()
    {
        if (File.Exists(saveFilePath))
        {
            GameDataHolder.selectedSaveSlot = slotIndex;
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.Log("No save file in this slot.");
            // Optionally show a message or just ignore
        }
    }
}