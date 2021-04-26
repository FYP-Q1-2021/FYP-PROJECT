using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private Transform player;

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player = Camera.main.GetComponent<Transform>();
    }

    void LateUpdate()
    {
        transform.LookAt(player);
    }
}
