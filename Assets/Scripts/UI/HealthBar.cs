using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private Health health;

    void Start()
    {
        if (!health)
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        slider = GetComponent<Slider>();
        slider.maxValue = health.GetMaxHealth();
        slider.value = health.GetMaxHealth();

        health.OnDamaged += UpdateHealthBar;
        health.OnHeal += UpdateHealthBar;
    }

    void OnDestroy()
    {
        health.OnDamaged -= UpdateHealthBar;
        health.OnHeal -= UpdateHealthBar;
    }

    public void UpdateHealthBar()
    {
        slider.value = health.GetCurrentHealth();
    }
}
