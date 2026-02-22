using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;   // Assign your Settings panel
    public GameObject loadingPanel;    // Assign your Loading panel (optional)

    [Header("Buttons")]
    public Button loadingButton;      // Optional: to disable if no save
    public Button settingButton;      // Optional: to disable if no save

    [Header("Loading Settings")]
    public string gameSceneName = "GameScene";  // Name of the scene to load
    public Slider progressBar;          // Optional: assign a Slider for progress

    private void Start()
    {
        // Optional: Disable continue button if no save file exists
        if (loadingButton != null)
        {
            bool saveExists = System.IO.File.Exists(Application.persistentDataPath + "/save.json");
            loadingButton.interactable = saveExists;
        }
    }

    // Called by New Game button
    public void NewGame()
    {
        // Optionally set a flag so GameManager knows it's a new game
        PlayerPrefs.SetInt("NewGame", 1);
        LoadScene();
    }

    // Called by Continue button
    public void Continue()
    {
        PlayerPrefs.SetInt("NewGame", 0);
        LoadScene();
    }

    // Common loading method
    private void LoadScene()
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        // If you want a simple synchronous load (no loading panel progress)
        // SceneManager.LoadScene(gameSceneName);

        // Better: asynchronous load with optional progress bar
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(gameSceneName);
        operation.allowSceneActivation = false; // Wait until progress reaches 0.9

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if (progressBar != null)
                progressBar.value = progress;

            // When progress is almost complete (0.9), we can activate the scene
            if (operation.progress >= 0.9f)
            {
                // Optional: wait a moment or let player press a key
                yield return new WaitForSeconds(0.5f);
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    // Called by Settings button
    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    // Called by Back button inside settings panel
    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    // Called by Quit button
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}