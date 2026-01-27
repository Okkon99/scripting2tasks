using UnityEngine;
using Vectors;

[RequireComponent(typeof(VectorRenderer))]
public class Cube4_8 : MonoBehaviour
{
    [HideInInspector]
    private VectorRenderer vectors;

    [SerializeField] private GameObject target;
    [SerializeField] private bool changeVersion;    // I first made version 1 (!changeVersion), then i realized
                                                    // i had forgotten that it needed to be normalized.
                                                    // However, i liked how v1 worked, so i decided to keep both.
    void Start()
    {
        vectors = GetComponent<VectorRenderer>();
    }

    void Update()
    {
        Vector3 endVector;

        if (!changeVersion)
        {
            Vector3 targetPos = target.transform.position;
            endVector = targetPos;
        }
        else
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;  // dir has always been a math equation i struggle
            Vector3 targetPos = target.transform.position;                              // to picture in my head.
            endVector = transform.position + dir;                                       // "my target minus my position is my direction"?
        }                                                                               // Even now that confuses me.

        using (vectors.Begin())
        {
            vectors.Draw(transform.position, endVector, new Color(1.0f, 0.7f, 0.0f) * 5.0f);
        }
    }
}
