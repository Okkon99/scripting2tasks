using UnityEngine;
using Vectors;

[RequireComponent(typeof(VectorRenderer))]
public class Cube3_1 : MonoBehaviour
{
    [HideInInspector]
    private VectorRenderer vectors;
    public Vector3 direction;

    void OnEnable()
    {
        vectors = GetComponent<VectorRenderer>();
    }

    void Update()
    {
        using (vectors.Begin())
        {
            vectors.Draw(
                transform.position,
                transform.position + direction,
                new Color(1.0f, 0.7f, 0.0f),
                0.05f,
                0.2f
            );
        }
    }
}
