using UnityEngine;

public class SubEjectionPoint : MonoBehaviour
{
    [SerializeField] private Transform mainEjectionPoint;

    [SerializeField]
    enum Direction
    { 
        LEFT,
        RIGHT
    }

    [SerializeField] private Direction direction;

    void Update()
    {
        switch (direction)
        {
            case Direction.LEFT:
                transform.forward = Quaternion.Euler(0, -1, 0) * mainEjectionPoint.forward;
                break;
            case Direction.RIGHT:
                transform.forward = Quaternion.Euler(0, 1, 0) * mainEjectionPoint.forward;
                break;
        }
    }
}
