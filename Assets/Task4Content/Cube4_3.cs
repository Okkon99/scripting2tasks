using UnityEngine;

public class Cube4_3 : MonoBehaviour
{
    Vector3 startPosition;



    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.position = startPosition + transform.forward * Mathf.Sin(Time.time * 2) * 5;

    }
}
