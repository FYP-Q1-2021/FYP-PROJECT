using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private Health health;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = health.GetMaxHealth();
        slider.value = health.GetMaxHealth();

        health.OnDamaged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        health.OnDamaged -= UpdateHealthBar;
    }

    public void UpdateHealthBar()
    {
        slider.value = health.GetCurrentHealth();
    }
}
