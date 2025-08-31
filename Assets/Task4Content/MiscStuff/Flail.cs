using UnityEngine;

public class Flail : MonoBehaviour
{
    FinalCube parent;

    void Start()
    {
        parent = transform.parent.gameObject.GetComponent<FinalCube>();
    }

    void OnTriggerEnter(Collider other)
    {
        parent.FlailLaunch(other.gameObject);
    }
}
