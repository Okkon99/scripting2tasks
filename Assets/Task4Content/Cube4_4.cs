using UnityEngine;

public class Cube4_4 : MonoBehaviour
{
    Vector3 start_position;

    void Start()
    {
        start_position = transform.position;
    }

    void Update()
    {
        float t = Unity.Mathematics.math.frac(Time.time);

    }
}
