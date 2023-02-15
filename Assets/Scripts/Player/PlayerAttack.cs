using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // hurt enemy ontrigger2d
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Hurt enemy");
            // check if has Blob component
            if (collision.gameObject.GetComponent<IBlob>() != null)
            {
                // hurt boss
                collision.gameObject.GetComponent<IBlob>().TakeDamage(1);
            }
            // check if has Boss component
            else if (collision.gameObject.GetComponent<Boss>() != null)
            {
                // hurt boss
                collision.gameObject.GetComponent<Boss>().TakeDamage(1);
            }
        }
    }
}
