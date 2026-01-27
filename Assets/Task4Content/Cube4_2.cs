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
        float t = Unity.Mathematics.math.frac(Time.time);

        transform.position = Vector3.Lerp(StartObject.transform.position, EndObject.transform.position, t);
    }
}
