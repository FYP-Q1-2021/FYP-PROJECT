using UnityEngine;

[ExecuteInEditMode]
public class ObjectForwardTest : MonoBehaviour
{
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 5;
        Debug.DrawRay(transform.position, forward, Color.green);
    }
}
