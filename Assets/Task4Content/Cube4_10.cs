using UnityEngine;

public class Cube4_10 : MonoBehaviour
{
    [SerializeField] float rotateSpeed;

    void Update()
    {
        transform.Rotate(new Vector3(0.0f, 0.0f, rotateSpeed * Time.deltaTime * 180.0f));
    }
}
