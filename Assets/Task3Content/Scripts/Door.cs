using GameTask3;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] float animationSpeed;

    [SerializeField] BoxCollider col;
    [SerializeField] BoxCollider trigger;

    bool isOpening;
    bool isClosing;
    Vector3 pos;

    Vector3 openPos;
    Vector3 closedPos;

    void Start()
    {
        closedPos = transform.position;
        openPos = closedPos + Vector3.down * transform.localScale.y - new Vector3(0f, 0.1f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Player>(out Player player))
        {
            return;
        }

        if (transform.position == closedPos)
        {
            isClosing = false;       
        }
        else
        {

        }

        if (player.hasKey)
        {
            isOpening = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Player>(out Player player))
        {
            return;
        }

        isOpening = false;
        isClosing = true;
    }



    private void Update()
    {
        if (isOpening)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPos, animationSpeed * Time.deltaTime);
        }

        if (isClosing)
        {
            transform.position = Vector3.MoveTowards(transform.position, closedPos, animationSpeed * Time.deltaTime);
        }
    }
}
