using UnityEngine;

public class Flail : MonoBehaviour
{
    FinalCube parent;

    [SerializeField] GameObject target;
    [SerializeField] float angle;

    void Start()
    {
        parent = transform.parent.gameObject.GetComponent<FinalCube>();
    }

    void OnTriggerEnter(Collider other)
    {
        parent.FlailLaunch(other.gameObject);
    }

    private void FixedUpdate()
    {
        transform.RotateAround(target.transform.position, Vector3.up, angle);
    }
}
