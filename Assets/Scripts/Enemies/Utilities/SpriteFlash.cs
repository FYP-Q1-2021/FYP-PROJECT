using System.Collections;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 1f;
    private float lerpTime = 0f;
    private SpriteRenderer spriteRenderer;
    private Material material;
    private Health health;

    private IEnumerator flashCoroutine;
    private bool usingDefaultMaterial = true;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (transform.parent.name == "Devil")
        {
            material = spriteRenderer.material;
            usingDefaultMaterial = false;
        }

        health = GetComponentInParent<Health>();
        health.OnDamaged += OnDamagedEvent;
    }

    void OnDisable()
    {
        if (usingDefaultMaterial)
            spriteRenderer.color = Color.white;
        else
            material.SetColor("_BaseColor", Color.white);
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
            if (usingDefaultMaterial)
            {
                spriteRenderer.color = Color.Lerp(Color.white, flashColor, 1f - lerpTime);
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
            }
            else
            {
                material.SetColor("_BaseColor", Color.Lerp(Color.white, flashColor, 1f - lerpTime));
                material.color = new Color(material.color.r, material.color.g, material.color.b, 1f);
            }

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