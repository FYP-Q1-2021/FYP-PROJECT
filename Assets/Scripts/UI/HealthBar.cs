using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private Health playerHP;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = playerHP.GetMaxHealth();
        slider.value = playerHP.GetMaxHealth();
    }

    public void UpdateHealthBar()
    {
        slider.value = playerHP.GetCurrentHealth();
    }
}
