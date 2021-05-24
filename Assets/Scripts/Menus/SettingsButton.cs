using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] private Button videoSetting;
    [SerializeField] private Button audioSetting;
    [SerializeField] private Button extraSetting;

    [SerializeField] private GameObject videoContent;
    [SerializeField] private GameObject audioContent;
    [SerializeField] private GameObject extraContent;

    private Image videoImage;
    [SerializeField] private Sprite emptyBackground;
    [SerializeField] private Sprite originalBackground;

    void Start()
    {
        Button btn = audioSetting.GetComponent<Button>();
        btn.onClick.AddListener(DisableDefaultBackground);

        btn = extraSetting.GetComponent<Button>();
        btn.onClick.AddListener(DisableDefaultBackground);

        videoImage = videoSetting.GetComponent<Image>();
    }

    void OnDisable()
    {
        videoImage.sprite = originalBackground;
        videoContent.SetActive(true);
        audioContent.SetActive(false);
        extraContent.SetActive(false);
    }

    void DisableDefaultBackground()
    {
        videoImage.sprite = emptyBackground;
    }
}
