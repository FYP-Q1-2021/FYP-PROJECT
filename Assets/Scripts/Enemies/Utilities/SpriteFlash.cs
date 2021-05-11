using System.Collections;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 1f;
    private float lerpTime = 0f;
    private SpriteRenderer spriteRenderer;
    private Health health;

    private IEnumerator flashCoroutine;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        health = GetComponentInParent<Health>();
        health.OnDamaged += OnDamagedEvent;
    }

    void OnDestroy()
    {
        health.OnDamaged -= OnDamagedEvent;
    }

    private void Flash()
    {
        if (flashCoroutine != null)
        {
            lerpTime = 0f;
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = DoFlash();
        StartCoroutine(flashCoroutine);
    }

    private IEnumerator DoFlash()
    {
        while (lerpTime < flashDuration)
        {
            spriteRenderer.color = Color.Lerp(Color.white, flashColor, 1f - lerpTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);

            lerpTime += Time.deltaTime / flashDuration;
            yield return null;
        }

        lerpTime = 0f;
    }

    private void OnDamagedEvent()
    {
        Flash();
    }
}