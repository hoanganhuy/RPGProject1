using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("UI References")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private AudioController controller;
    private bool isInitialized = false;

    // Pending changes
    private float pendingMasterVolume;
    private float pendingMusicVolume;
    private float pendingSfxVolume;
    private bool pendingMuted;

    void OnEnable()
    {
        // Delay initialization to next frame to ensure SettingsManager is ready
        if (!isInitialized)
        {
            StartCoroutine(DelayedInitialize());
        }
        else
        {
            LoadCurrentSettings();
        }
    }

    void Start()
    {
        if (!isInitialized)
        {
            StartCoroutine(DelayedInitialize());
        }
    }

    private IEnumerator DelayedInitialize()
    {
        // Wait until SettingsManager is ready
        while (SettingsManager.Instance == null)
        {
            yield return null;
        }

        EnsureInitialized();
        if (isInitialized)
        {
            LoadCurrentSettings();
        }
    }

    // Make sure controller is always initialized
    private void EnsureInitialized()
    {
        if (isInitialized) return;

        if (SettingsManager.Instance != null)
        {
            controller = SettingsManager.Instance.Audio;
            InitializeUI();
            isInitialized = true;
            Debug.Log("AudioSettingsUI: Initialized");
        }
        else
        {
            Debug.LogError("AudioSettingsUI: SettingsManager.Instance is null!");
        }
    }

    private void InitializeUI()
    {
        // Xóa listeners cũ
        masterVolumeSlider.onValueChanged.RemoveAllListeners();
        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        sfxVolumeSlider.onValueChanged.RemoveAllListeners();

        // Add listeners mới - CHỈ lưu vào pending
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    public void LoadCurrentSettings()
    {
        EnsureInitialized();

        if (controller == null)
        {
            Debug.LogError("AudioSettingsUI: Cannot load settings - controller is null");
            return;
        }

        var data = controller.GetData();

        masterVolumeSlider.SetValueWithoutNotify(data.masterVolume);
        musicVolumeSlider.SetValueWithoutNotify(data.musicVolume);
        sfxVolumeSlider.SetValueWithoutNotify(data.sfxVolume);

        // Load vào pending values
        pendingMasterVolume = data.masterVolume;
        pendingMusicVolume = data.musicVolume;
        pendingSfxVolume = data.sfxVolume;
        pendingMuted = data.muted;
    }

    // CHỈ lưu vào pending, KHÔNG apply ngay
    private void OnMasterVolumeChanged(float value)
    {
        pendingMasterVolume = value;
    }

    private void OnMusicVolumeChanged(float value)
    {
        pendingMusicVolume = value;
    }

    private void OnSFXVolumeChanged(float value)
    {
        pendingSfxVolume = value;
    }

    private void OnMuteChanged(bool value)
    {
        pendingMuted = value;
    }

    private void UpdateVolumeText(TMP_Text text, float value)
    {
        text.text = $"{Mathf.RoundToInt(value * 100)}%";
    }

    // Method này được gọi bởi SettingsMenuUI khi nhấn Apply
    public void ApplyPendingChanges()
    {
        EnsureInitialized();

        if (controller == null)
        {
            Debug.LogError("AudioSettingsUI: Cannot apply - controller is null!");
            return;
        }

        Debug.Log("Audio: Applying pending changes...");
        controller.SetMasterVolume(pendingMasterVolume);
        controller.SetMusicVolume(pendingMusicVolume);
        controller.SetSFXVolume(pendingSfxVolume);
        controller.SetMuted(pendingMuted);
        Debug.Log($"Audio pending applied - Master: {pendingMasterVolume}, Music: {pendingMusicVolume}");
    }
}