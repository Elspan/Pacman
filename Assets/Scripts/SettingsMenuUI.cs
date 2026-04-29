using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUI : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject settingsPanel;

    [Header("Sliders")]
    public Slider playerSpeedSlider;
    public Slider ghostNormalSpeedSlider;
    public Slider ghostFrightenedSpeedSlider;
    public Slider powerPelletSlider;
    public Slider prisonTimeSlider;

    [Header("Labels valeur")]
    public Text playerSpeedLabel;
    public Text ghostNormalSpeedLabel;
    public Text ghostFrightenedSpeedLabel;
    public Text powerPelletLabel;
    public Text prisonTimeLabel;

    private bool wasGameRunning = false;

    void Start()
    {
        settingsPanel.SetActive(false);
        SyncSlidersToSettings();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleSettings();
    }

    void SyncSlidersToSettings()
    {
        playerSpeedSlider.value = gameManager.settings.playerSpeed;
        ghostNormalSpeedSlider.value = gameManager.settings.ghostNormalSpeed;
        ghostFrightenedSpeedSlider.value = gameManager.settings.ghostFrightenedSpeed;
        powerPelletSlider.value = gameManager.settings.powerPelletDuration;
        prisonTimeSlider.value = gameManager.settings.ghostPrisonTime;
        UpdateAllLabels();
    }

    void UpdateAllLabels()
    {
        playerSpeedLabel.text = gameManager.settings.playerSpeed.ToString("F1");
        ghostNormalSpeedLabel.text = gameManager.settings.ghostNormalSpeed.ToString("F1");
        ghostFrightenedSpeedLabel.text = gameManager.settings.ghostFrightenedSpeed.ToString("F1");
        powerPelletLabel.text = gameManager.settings.powerPelletDuration.ToString("F1") + "s";
        prisonTimeLabel.text = gameManager.settings.ghostPrisonTime.ToString("F1") + "s";
    }

    public void ToggleSettings()
    {
        if (settingsPanel.activeSelf)
            CloseSettings();
        else
            OpenSettings();
    }

    public void OpenSettings()
    {
        SyncSlidersToSettings();
        settingsPanel.SetActive(true);
        wasGameRunning = gameManager.gameIsRunning;
        gameManager.gameIsRunning = false;
        if (gameManager.siren.isPlaying) gameManager.siren.Pause();
        if (gameManager.powerPelletAudio.isPlaying) gameManager.powerPelletAudio.Pause();
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        if (wasGameRunning)
        {
            gameManager.gameIsRunning = true;
            gameManager.siren.UnPause();
            gameManager.powerPelletAudio.UnPause();
        }
    }

    public void OnPlayerSpeedChanged(float value)
    {
        gameManager.settings.playerSpeed = value;
        playerSpeedLabel.text = value.ToString("F1");
        gameManager.pacman.GetComponent<MovementController>().SetSpeed(value);
    }

    public void OnGhostNormalSpeedChanged(float value)
    {
        gameManager.settings.ghostNormalSpeed = value;
        ghostNormalSpeedLabel.text = value.ToString("F1");
    }

    public void OnGhostFrightenedSpeedChanged(float value)
    {
        gameManager.settings.ghostFrightenedSpeed = value;
        ghostFrightenedSpeedLabel.text = value.ToString("F1");
    }

    public void OnPowerPelletChanged(float value)
    {
        gameManager.settings.powerPelletDuration = value;
        gameManager.powerPelletTimer = value;
        powerPelletLabel.text = value.ToString("F1") + "s";
    }

    public void OnPrisonTimeChanged(float value)
    {
        gameManager.settings.ghostPrisonTime = value;
        prisonTimeLabel.text = value.ToString("F1") + "s";
    }

    public void ResetDefaults()
    {
        gameManager.settings.playerSpeed = 4f;
        gameManager.settings.ghostNormalSpeed = 2f;
        gameManager.settings.ghostFrightenedSpeed = 1f;
        gameManager.settings.powerPelletDuration = 8f;
        gameManager.settings.ghostPrisonTime = 5f;
        gameManager.powerPelletTimer = 8f;
        gameManager.pacman.GetComponent<MovementController>().SetSpeed(4f);
        SyncSlidersToSettings();
    }
}
