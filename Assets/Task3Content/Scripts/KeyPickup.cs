using GameTask3;
using UnityEngine;
using static GameTask3.Player;

public class KeyPickup : MonoBehaviour
{
    [SerializeField] KeyId keyId;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Player>(out var player))
        {
            return;
        }

        player.AddKey(keyId);
        Destroy(gameObject);
    }
}

