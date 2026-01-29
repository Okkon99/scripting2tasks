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
        Vector3 currentPos = transform.position;

        currentPos.z = startPosition.z + easeInOutElastic(t) * 5;
        transform.position = currentPos;
    }

    float easeInOutElastic(float x)
    {
        float c5 = (2 * Mathf.PI) / 4.5f;


        if (x == 0)
        {
            return 0;
        }
        else if (x == 1)
        {
            return 1;
        }
        else if (x < 0.5f)
        {
            return -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2;
        }
        else
        {
            return (Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2 + 1;
        }
    }
}
