using Unity.VisualScripting;
using UnityEngine;

public class Cube4_6 : MonoBehaviour
{
    [SerializeField] private float moveDistance;
    public GameObject orbitTarget;
    private Vector3 orbitToCube;

    void FixedUpdate()
    {
        Vector3 dir = (orbitTarget.transform.position - transform.position).normalized;
        dir = Vector3.Cross(dir, Vector3.up);

        Vector3 newPos = transform.position + dir * moveDistance;
        Vector3 orbitToNewPos = (newPos - orbitTarget.transform.position).normalized;

        float distance = (orbitTarget.transform.position - transform.position).magnitude;
        transform.position = orbitTarget.transform.position+orbitToNewPos*distance;
    }
}
