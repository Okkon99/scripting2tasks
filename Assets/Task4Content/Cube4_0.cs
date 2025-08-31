using UnityEngine;
using Vectors;

[RequireComponent(typeof(VectorRenderer))]
public class Cube4_0 : MonoBehaviour
{
    [HideInInspector]
    private VectorRenderer vectors;

    public GameObject target;

    void Start()
    {
        vectors = GetComponent<VectorRenderer>();
    }

    void Update()
    {
        Vector3 end_vector = new Vector3(0.0f, 0.0f, 0.0f); // replace

        using (vectors.Begin())
        {
            vectors.Draw(transform.position, end_vector, Color.orange * 5.0f);
        }
    }
}
