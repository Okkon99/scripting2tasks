using UnityEngine;

public class Cube4_12 : MonoBehaviour
{
    public GameObject target;
    [SerializeField] float angle;
    private float distance;

    [SerializeField] float thisShouldBeAStaticValue;

    private void Start()
    {
        distance = (target.transform.position - transform.position).magnitude;
    }

    void FixedUpdate()
    {
        float targetY = target.transform.position.y;
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);


        transform.RotateAround(target.transform.position, Vector3.up, angle);



        thisShouldBeAStaticValue = (target.transform.position - transform.position).magnitude;
    }
}
