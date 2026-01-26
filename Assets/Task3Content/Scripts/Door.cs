using GameTask3;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using static GameTask3.Player;


public class Door : MonoBehaviour
{
    [SerializeField] KeyId requiredKey;
    [SerializeField] float animationSpeed;

    [SerializeField] BoxCollider col;
    [SerializeField] BoxCollider trigger;


    enum DoorState
    {
        Closed,
        Opening,
        Open,
        Closing
    }



    DoorState state;
    Vector3 pos;
    Vector3 openPos;
    Vector3 closedPos;

    void Start()
    {
        closedPos = transform.position;
        openPos = closedPos + Vector3.down * transform.localScale.y - new Vector3(0f, 0.1f, 0f);
        state = DoorState.Closed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Player>(out Player player))
        {
            return;
        }

        if (!player.HasKey(requiredKey))
        {
            return;
        }

        if (state == DoorState.Closing || state == DoorState.Closed)
        {
            state = DoorState.Opening;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Player>(out Player player))
        {
            return;
        }

        if (state == DoorState.Opening || state == DoorState.Open)
        {
            state = DoorState.Closing;
        }
    }



    private void Update()
    {
        switch (state)
        {
            case DoorState.Opening: transform.position = Vector3.MoveTowards(transform.position, openPos, animationSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, openPos) < 0.01f)
                {
                    state = DoorState.Open;
                }
                break;

            case DoorState.Closing: transform.position = Vector3.MoveTowards(transform.position, closedPos, animationSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, closedPos) < 0.01f)
                {
                    state =DoorState.Closed;
                }
                break;
        }
    }

    public bool CanOpen(Player player)
    {
        return player.HasKey(requiredKey);
    }
}
