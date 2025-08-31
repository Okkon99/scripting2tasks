using UnityEngine;
using Vectors;

[RequireComponent(typeof(VectorRenderer))]
public class Cube4_3 : MonoBehaviour
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
        using (vectors.Begin())
        {
            vectors.Draw(transform.position, transform.position + transform.forward * 2.0f, Color.orange * 5.0f);
        }
    }
}
