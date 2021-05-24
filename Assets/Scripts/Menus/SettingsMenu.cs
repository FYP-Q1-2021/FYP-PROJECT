using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resolutionText;
    [SerializeField] private TextMeshProUGUI fullscreenStatus;
    [SerializeField] private TextMeshProUGUI brightnessText;
    private int currentBrightness;

    private Resolution[] resolutions;
    private List<string> options = new List<string>();

    private int numOfResolutions;
    private int currentResolutionIndex;

    void Start()
    {
        resolutions = Screen.resolutions;
        numOfResolutions = resolutions.Length;

        for(int i = 0; i < numOfResolutions; ++i)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                resolutionText.text = option;
                currentResolutionIndex = i;
            }
        }

        if (Screen.fullScreen)
            fullscreenStatus.text = "Fullscreen";
        else
            fullscreenStatus.text = "Windowed";

        currentBrightness = int.Parse(brightnessText.text.Substring(0, brightnessText.text.Length - 1));
    }

    public void DecreaseResolution()
    {
        if (currentResolutionIndex < 1)
            return;

        --currentResolutionIndex;
        Resolution resolution = resolutions[currentResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        resolutionText.text = options[currentResolutionIndex];
    }

    public void IncreaseResolution()
    {
        if (currentResolutionIndex > numOfResolutions - 2)
            return;

        ++currentResolutionIndex;
        Resolution resolution = resolutions[currentResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        resolutionText.text = options[currentResolutionIndex];
    }

    public void SetFullscreen()
    {
        if (Screen.fullScreen)
        {
            Screen.fullScreen = false;
            fullscreenStatus.text = "Windowed";
        }
        else
        {
            Screen.fullScreen = true;
            fullscreenStatus.text = "Fullscreen";
        }
    }

    // Screen.brightness only works on iOS
    public void LowerBrightness()
    {
        if (currentBrightness < 1)
            return;

        --currentBrightness;
        brightnessText.text = currentBrightness.ToString() + "%";
        //Screen.brightness = currentBrightness / 100f;
    }

    public void IncreaseBrightness()
    {
        if (currentBrightness > 99)
            return;

        ++currentBrightness;
        brightnessText.text = currentBrightness.ToString() + "%";
        //Screen.brightness = currentBrightness / 100f;
    }
}
