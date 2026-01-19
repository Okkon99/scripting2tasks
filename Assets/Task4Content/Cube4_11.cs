using UnityEngine;
using Vectors;

[RequireComponent(typeof(VectorRenderer))]
public class Cube4_11 : MonoBehaviour
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
            vectors.Draw(transform.position, transform.position + transform.forward * 2.0f, new Color(1.0f, 0.7f, 0.0f) * 5.0f);
        }
    }
}
