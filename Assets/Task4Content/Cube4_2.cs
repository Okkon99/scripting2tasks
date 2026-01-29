using Unity.VisualScripting;
using UnityEngine;
using Vectors;

public class Cube4_2 : MonoBehaviour
{
    public GameObject StartObject;
    public GameObject EndObject;


    private void Start()
    {
        transform.position = StartObject.transform.position;
    }

    void Update()
    {
        Vector3 startToEnd = (EndObject.transform.position - StartObject.transform.position);

        float t = Unity.Mathematics.math.frac(Time.time);

        transform.position = Vector3.LerpUnclamped(StartObject.transform.position, EndObject.transform.position, easeInOutElastic(t));
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
