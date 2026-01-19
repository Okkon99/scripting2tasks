using UnityEngine;
using Vectors;

[RequireComponent(typeof(VectorRenderer))]
public class Cube4_8 : MonoBehaviour
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
            vectors.Draw(transform.position, end_vector, new Color(1.0f, 0.7f, 0.0f) * 5.0f);
        }
    }
}
