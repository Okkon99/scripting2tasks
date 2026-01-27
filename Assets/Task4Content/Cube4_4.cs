using UnityEngine;

public class Cube4_4 : MonoBehaviour
{
    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float t = Unity.Mathematics.math.frac(Time.time);

    }
}
