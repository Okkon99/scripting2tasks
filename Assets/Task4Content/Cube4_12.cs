using UnityEngine;

public class Cube4_12 : MonoBehaviour
{
    public GameObject target;
    [SerializeField] float angle;

    [SerializeField] float thisShouldBeAStaticValue;

    void Update()
    {
        transform.RotateAround(target.transform.position, Vector3.up, angle);

        float targetY = target.transform.position.y;
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);



        thisShouldBeAStaticValue = (target.transform.position - transform.position).magnitude;
    }
}
