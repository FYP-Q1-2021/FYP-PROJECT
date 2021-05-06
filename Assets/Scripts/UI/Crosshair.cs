using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private float range = 10f;
    [SerializeField] private GameObject normal;
    [SerializeField] private GameObject pointingAtObject;
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        int x = Screen.width / 2;
        int y = Screen.height / 2;

        Ray ray = playerCamera.ScreenPointToRay(new Vector3(x, y));

        if(Physics.Raycast(ray, out RaycastHit hit, range))
        {
            if(hit.collider.gameObject.GetComponent<MeshDestroy>())
            {
                pointingAtObject.SetActive(true);
                normal.SetActive(false);
            }
            else
            {
                normal.SetActive(true);
                pointingAtObject.SetActive(false);
            }
        }
        else
        {
            normal.SetActive(true);
            pointingAtObject.SetActive(false);
        }
    }
}
