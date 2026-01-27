using UnityEngine;
using UnityEngine.UIElements;
using Vectors;

[RequireComponent(typeof(VectorRenderer))]
public class Cube4_0 : MonoBehaviour
{
    [HideInInspector]
    private VectorRenderer vectors;

    [SerializeField] private float moveSpeed;

    Vector3 startPos;

    void OnEnable()
    {
        vectors = GetComponent<VectorRenderer>();
    }

    private void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + moveSpeed * Time.deltaTime);

        using (vectors.Begin())
        {
            vectors.Draw(
                transform.position,
                transform.position + new Vector3(0.0f, 0.0f, 1.0f),
                new Color(1.0f, 0.7f, 0.0f),
                0.05f,
                0.2f
            );
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = startPos;
        }

    }
}
