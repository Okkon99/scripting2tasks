using UnityEngine;
using Vectors;
using UnityEngine.InputSystem;


[RequireComponent(typeof(VectorRenderer))]
public class Cube4_1 : MonoBehaviour
{
    [HideInInspector]
    private VectorRenderer vectors;
    private Vector3 moveInput;

    [SerializeField] float moveSpeed;



    void OnEnable()
    {
        vectors = GetComponent<VectorRenderer>();
    }

    void Update()
    {
        float x = 0f;
        float y = 0f;
        float z = 0f;


        if (Input.GetKey(KeyCode.D))
            x += 1f;
        if (Input.GetKey(KeyCode.A))
            x -= 1f;
        if (Input.GetKey(KeyCode.LeftShift))
            y -= 1f;
        if (Input .GetKey(KeyCode.LeftControl))
            y += 1f;
        if (Input.GetKey(KeyCode.W))
            z += 1f;
        if (Input.GetKey(KeyCode.S))
            z -= 1f;


        moveInput = new Vector3(x, y, z).normalized;


        Vector3 inputDirection = transform.forward * moveInput.z + transform.right * moveInput.x + transform.up * moveInput.y;


        transform.position = new Vector3(
            transform.position.x + inputDirection.x * Time.deltaTime * moveSpeed,
            transform.position.y + inputDirection.y * Time.deltaTime * moveSpeed,
            transform.position.z + inputDirection.z * Time.deltaTime * moveSpeed
            );


        using (vectors.Begin())
        {
            vectors.Draw(
                transform.position,
                transform.position + inputDirection*2f, // * 2f so the line is more visible
                new Color(1.0f, 0.7f, 0.0f),
                0.05f,
                0.2f
            );
        }
    }
}
