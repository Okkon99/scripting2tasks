using Unity.VisualScripting;
using UnityEngine;

public class Cube4_6 : MonoBehaviour
{
    [SerializeField] private float moveDistance;
    public GameObject orbitTarget;
    Vector3 targetPos;

    [SerializeField] private float distance;


    void FixedUpdate()
    {
        targetPos = orbitTarget.transform.position;
        transform.position = targetPos + transform.forward * distance * Mathf.Sin(Time.time) + (transform.right * distance * Mathf.Cos(Time.time));
    }
}
