using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TelePoint : MonoBehaviour
{
    public bool isEnabled = false;
    // target
    public Transform target;

    public UnityEvent OnTeleportation;

    // on trigger2d enter
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if collision is player
        if (collision.gameObject.CompareTag("Player"))
        {
            // set player position to target position
            collision.gameObject.transform.position = target.position;
            OnTeleportation?.Invoke();
        }
    }

}
