using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressKeyTrigger : MonoBehaviour
{
    public string buttonName = "Interact";

    public void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (Input.GetButton(buttonName))
            {
                // send OnUse message to self gameObject
                gameObject.SendMessage("OnUse", other.transform);
                
            }
        }
    }
}
