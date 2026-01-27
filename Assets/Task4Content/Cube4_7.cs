using UnityEngine;

public class Cube4_7 : MonoBehaviour
{
    public GameObject orbitTarget;

    public Vector3 rotation;

    void Update()
    {
        float distance = (orbitTarget.transform.position - transform.position).magnitude;

        transform.position = orbitTarget.transform.position;
        transform.Rotate(0f, 1f, 0f);
        transform.position += transform.forward * distance;
    }
}
