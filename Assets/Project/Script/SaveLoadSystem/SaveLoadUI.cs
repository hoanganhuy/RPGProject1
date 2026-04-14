using TMPro;
using UnityEngine;

public class SaveLoadUI : MonoBehaviour
{
    public SaveLoadMode mode;
    public SaveSlotUI[] slots;
    public TextMeshProUGUI modeText;
    void Start()
    {
        //  Ẩn panel khi bắt đầu game
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        RefreshAllSlots();
    }
    void UpdateModeText()
    {
        if (mode == SaveLoadMode.Save)
        {
            modeText.text = "SAVE GAME";
        }
        else
        {
            modeText.text = "LOAD GAME";
        }
    }
    // ===== SAVE =====
    public void OpenSave()
    {
        // Nếu đang mở và cùng mode → tắt
        if (gameObject.activeSelf && mode == SaveLoadMode.Save)
        {
            gameObject.SetActive(false);
            return;
        }

        // Nếu đang mở nhưng khác mode → đổi mode
        mode = SaveLoadMode.Save;
        UpdateModeText();
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        else
            RefreshAllSlots();
    }

    // ===== LOAD =====
    public void OpenLoad()
    {
        // Nếu đang mở và cùng mode → tắt
        if (gameObject.activeSelf && mode == SaveLoadMode.Load)
        {
            gameObject.SetActive(false);
            return;
        }

        // Nếu đang mở nhưng khác mode → đổi mode
        mode = SaveLoadMode.Load;
        UpdateModeText();
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        else
            RefreshAllSlots();
    }

    // ===== REFRESH =====
    public void RefreshAllSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Setup(i, this);
        }
    }
}