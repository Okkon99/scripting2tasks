using UnityEngine;

public class Sphere : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0.0f, 1.0f * Time.deltaTime * 180.0f, 0.0f));
    }
}
