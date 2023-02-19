using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TelePoint : MonoBehaviour
{
    public bool isEnabled = false;
    // target
    public Transform target;
    public float delay = 1f;

    public UnityEvent OnTeleportation;


    // on trigger2d enter
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if collision is player
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(TeleportPlayer(collision));
        }
    }

    IEnumerator TeleportPlayer(Collider2D go)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Teleporting player..");

        // set player position to target position
        go.gameObject.transform.position = target.position;
        OnTeleportation?.Invoke();
    }
}
