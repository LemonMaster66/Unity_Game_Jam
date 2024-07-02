using UnityEngine;
using VInspector;

public class Billboard : MonoBehaviour
{
    [RangeResettable(0,1)]
    public float Speed = 1;
    public Vector3 Offset;

    private Transform cam;

    void Awake()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        Vector3 direction = cam.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(Offset);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Speed);
    }
}

