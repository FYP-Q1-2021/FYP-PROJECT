using System.Collections;
using UnityEngine;

public class DestroyedVersion : MonoBehaviour
{
    [SerializeField] private float despawnTime = 3f;
    [SerializeField] private float despawnSpeed = 1f;
    [SerializeField] private float fadeSpeed = 1f;
    private float timer = 0f;

    private MeshRenderer[] meshRenderers;
    private float fadeAmount = 1f;
    private int childrenCount;

    void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        childrenCount = meshRenderers.Length;
    }

    void Update()
    {
        if (timer < despawnTime)
            timer += despawnSpeed * Time.deltaTime;
        else
            StartCoroutine("Fade");
    }

    IEnumerator Fade()
    {
        fadeAmount -= fadeSpeed * Time.deltaTime;

        for (int i = 0; i < childrenCount; ++i)
        {
            Color color = meshRenderers[i].material.color;
            meshRenderers[i].material.color = new Color(color.r, color.g, color.b, fadeAmount);
        }

        yield return new WaitUntil(() => fadeAmount < 0f);

        Destroy(gameObject);
    }
}
