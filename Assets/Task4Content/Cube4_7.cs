using Unity.VisualScripting;
using UnityEngine;

public class Cube4_7 : MonoBehaviour
{
    [SerializeField] private float moveDistance;
    public GameObject orbitTarget;
    public Vector3 rotation;
    Vector3 targetPos;

    [SerializeField] private float distance;


    void FixedUpdate()
    {

        transform.Rotate(rotation);

        targetPos = orbitTarget.transform.position;
        transform.position = targetPos + transform.forward * distance * Mathf.Sin(Time.time) + (transform.right * distance * Mathf.Cos(Time.time));


        transform.position = targetPos + new Vector3(Mathf.Cos(Time.time), 0.0f,Mathf.Sin(Time.time)) * distance;
        //  transform.position = startPosition + transform.forward * Mathf.Sin(Time.time * 2) * 5;
    }
}
