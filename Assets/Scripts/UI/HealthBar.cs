using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    private Health playerHP;

    void Start()
    {
        playerHP = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        slider = GetComponent<Slider>();
        slider.maxValue = playerHP.GetMaxHealth();
        slider.value = playerHP.GetMaxHealth();
    }

    public void UpdateHealthBar()
    {
        slider.value = playerHP.GetCurrentHealth();
    }
}
