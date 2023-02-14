using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryDialogueTrigger : MonoBehaviour
{
    public GameObject secondaryDialogueTrigger;
    
    public void EnableSecondaryTriggerAfter(float time)
    {
        StartCoroutine(EnableSecondaryTriggerHelper(time));
    }

    IEnumerator EnableSecondaryTriggerHelper(float time)
    {
        yield return new WaitForSeconds(time);
        secondaryDialogueTrigger.SetActive(true);
    }

}
