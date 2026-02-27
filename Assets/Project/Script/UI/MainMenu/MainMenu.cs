using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject saveSlotsPanel;

    private void Start()
    {
        // Ensure panels are closed at start
        optionsPanel.SetActive(false);
        saveSlotsPanel.SetActive(false);
    }

    public void OnNewGame()
    {
        // Set flag: new game (no save slot)
        GameDataHolder.selectedSaveSlot = -1;
        SceneManager.LoadScene("GameScene");
    }

    public void OnContinue()
    {
        // Show save slots panel instead of loading directly
        saveSlotsPanel.SetActive(true);
    }

    public void OnOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void OnQuit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    public void CloseSaveSlots()
    {
        saveSlotsPanel.SetActive(false);
    }
}