using UnityEngine;
using TMPro;

public class TempEnemyHealthBar : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Health health;

    void Update()
    {
        text.text = health.GetCurrentHealth().ToString();
    }
}
