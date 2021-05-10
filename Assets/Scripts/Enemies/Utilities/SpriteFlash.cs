using System.Collections;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    [SerializeField] private Color flashColor;
    [SerializeField] private float flashDuration = 1f;
    private Material mat;
    private Health health;

    private IEnumerator flashCoroutine;

    void Awake()
    {
        mat = GetComponent<SpriteRenderer>().material;
        health = GetComponentInParent<Health>();
        health.OnDamaged += OnDamagedEvent;
    }

    void Start()
    {
        mat.SetColor("_FlashColor", flashColor);
    }

    void OnDestroy()
    {
        health.OnDamaged -= OnDamagedEvent;
    }

    private void Flash()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = DoFlash();
        StartCoroutine(flashCoroutine);
    }

    private IEnumerator DoFlash()
    {
        float lerpTime = 0;

        while (lerpTime < flashDuration)
        {
            lerpTime += Time.deltaTime;
            float perc = lerpTime / flashDuration;

            SetFlashAmount(1f - perc);
            yield return null;
        }
        SetFlashAmount(0);
    }

    private void SetFlashAmount(float flashAmount)
    {
        mat.SetFloat("_FlashAmount", flashAmount);
    }

    private void OnDamagedEvent()
    {
        Flash();
    }
}