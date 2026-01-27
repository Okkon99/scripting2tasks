using Unity.VisualScripting;
using UnityEngine;

public class Cube4_9 : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    float currentRotation;

    void Update()
    {
        currentRotation += rotateSpeed * Time.deltaTime * 180f;
        transform.eulerAngles = new Vector3(0, 0, currentRotation);
    }
}
