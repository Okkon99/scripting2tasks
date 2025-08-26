using UnityEngine;

public class OrbitSphere : MonoBehaviour
{
    Vector3 start_position;

    void Start()
    {
        start_position = transform.position;
    }

    void Update()
    {
        transform.position = start_position + new Vector3(0.0f, 0.0f, Mathf.Sin(Time.time));
    }
}
