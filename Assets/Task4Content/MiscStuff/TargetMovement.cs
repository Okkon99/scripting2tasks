using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    Vector3 start_position;

    void Start()
    {
        start_position = transform.position;
    }

    void LateUpdate()
    {
        transform.position = start_position + new Vector3(Mathf.Cos(Time.time), Mathf.Sin(Time.time), 0.0f);
    }
}
