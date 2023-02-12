using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressKeyTrigger : MonoBehaviour
{
    public KeyCode key;

    public void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(key))
            {
                Debug.Log("OnTriggerStay2D");
                // send OnUse message to self gameObject
                gameObject.SendMessage("OnUse", other.transform);
                
            }
        }
    }
}
