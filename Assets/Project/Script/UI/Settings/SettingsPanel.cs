using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{

    [Header("Tab Buttons")]
    public Button graphicsTabButton;
    public Button audioTabButton;
    public Button exitTabButton;

    [Header("Panel GameObjects")]
    public GameObject graphicsPanel;
    public GameObject audioPanel;
    public GameObject exitPanel;

    [Header("Action Buttons")]
    public Button applyButton;
    public Button resetButton;
    public Button backButton;

    [Header("Tab Visual Colors")]
    public Color activeTabColor = Color.white;
    public Color inactiveTabColor = Color.gray;

    private GameObject currentActivePanel;

    void Start()
    {
        // Setup tab buttons
        graphicsTabButton.onClick.AddListener(() => ShowPanel(graphicsPanel));
        audioTabButton.onClick.AddListener(() => ShowPanel(audioPanel));

        // Setup action buttons
        applyButton.onClick.AddListener(OnApply);
        resetButton.onClick.AddListener(OnReset);
        backButton.onClick.AddListener(OnBack);

        // Hi?n th? graphics panel m?c ??nh
        ShowPanel(graphicsPanel);
        RefreshAllPanels();
    }

    public void ShowPanel(GameObject panelToShow)
    {
        // T?t t?t c? panels
        graphicsPanel.SetActive(false);
        audioPanel.SetActive(false);

        // B?t panel ???c ch?n
        panelToShow.SetActive(true);
        currentActivePanel = panelToShow;

        // C?p nh?t màu tab buttons
        UpdateTabColors(panelToShow);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayUISFX(SoundType.UI_Click);
        }
    }

    private void UpdateTabColors(GameObject activePanel)
    {
        // Reset t?t c? màu tab
        SetButtonColor(graphicsTabButton, inactiveTabColor);
        SetButtonColor(audioTabButton, inactiveTabColor);

        // Highlight tab ?ang active
        if (activePanel == graphicsPanel) SetButtonColor(graphicsTabButton, activeTabColor);
        else if (activePanel == audioPanel) SetButtonColor(audioTabButton, activeTabColor);
    }

    private void SetButtonColor(Button button, Color color)
    {
        var colors = button.colors;
        colors.normalColor = color;
        colors.selectedColor = color;
        button.colors = colors;
    }

    private void OnApply()
    {
        Debug.Log("========== APPLY BUTTON CLICKED ==========");

        try
        {
            // Step 1: Apply pending changes from all UI panels to controllers
            Debug.Log("Step 1: Applying pending changes from UI to controllers...");
            ApplyAllPendingChanges();

            // Step 2: Apply settings from controllers to Unity systems
            Debug.Log("Step 2: Applying settings to Unity...");
            if (SettingsManager.Instance != null)
            {
                SettingsManager.Instance.ApplyAllSettings();
            }
            else
            {
                Debug.LogError("SettingsManager.Instance is NULL!");
            }

            // Step 3: Save settings to PlayerPrefs
            Debug.Log("Step 3: Saving settings...");
            if (SettingsManager.Instance != null)
            {
                SettingsManager.Instance.SaveSettings();
            }

            Debug.Log("========== SETTINGS SUCCESSFULLY APPLIED AND SAVED ==========");

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayUISFX(SoundType.UI_Click);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"ERROR in OnApply: {e.Message}\n{e.StackTrace}");
        }
    }

    private void ApplyAllPendingChanges()
    {
        Debug.Log("Applying pending changes from all UI panels...");

        try
        {
            var graphicsUI = graphicsPanel.GetComponent<GraphicsSettingsUI>();
            var audioUI = audioPanel.GetComponent<AudioSettingsUI>();

            if (graphicsUI != null)
            {
                Debug.Log("Calling GraphicsUI.ApplyPendingChanges()...");
                graphicsUI.ApplyPendingChanges();
            }
            else
            {
                Debug.LogWarning("GraphicsSettingsUI component not found!");
            }

            if (audioUI != null)
            {
                Debug.Log("Calling AudioUI.ApplyPendingChanges()...");
                audioUI.ApplyPendingChanges();
            }
            else
            {
                Debug.LogWarning("AudioSettingsUI component not found!");
            }

            Debug.Log("All pending changes applied to controllers");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"ERROR in ApplyAllPendingChanges: {e.Message}\n{e.StackTrace}");
        }
    }

    private void OnReset()
    {
        Debug.Log("========== RESET BUTTON CLICKED ==========");

        try
        {
            if (SettingsManager.Instance != null)
            {
                // Reset settings to defaults
                SettingsManager.Instance.ResetToDefaults();

                // Refresh t?t c? UI panels ?? hi?n th? default values
                RefreshAllPanels();

                Debug.Log("========== SETTINGS RESET TO DEFAULTS ==========");
            }
            else
            {
                Debug.LogError("SettingsManager.Instance is NULL!");
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayUISFX(SoundType.UI_Click);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"ERROR in OnReset: {e.Message}\n{e.StackTrace}");
        }
    }

    private void RefreshAllPanels()
    {
        Debug.Log("Refreshing all UI panels...");

        var graphicsUI = graphicsPanel.GetComponent<GraphicsSettingsUI>();
        var audioUI = audioPanel.GetComponent<AudioSettingsUI>();

        if (graphicsUI != null) graphicsUI.LoadCurrentSettings();
        if (audioUI != null) audioUI.LoadCurrentSettings();

        Debug.Log("All panels refreshed");
    }

    private void OnBack()
    {
        gameObject.SetActive(false);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayUISFX(SoundType.UI_Click);
        }
    }
}