using UnityEngine;

public class Billboarding : MonoBehaviour
{
    private Transform player;

    void Start()
    {
        player = Camera.main.GetComponent<Transform>();
    }

    void LateUpdate()
    {
        transform.LookAt(player);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}
