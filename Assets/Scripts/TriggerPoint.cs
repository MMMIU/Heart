using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject doors;
    [SerializeField]
    private bool triggerEnabled = false;
    [SerializeField]
    private float triggerEnableDelay = 1f;
    [SerializeField]
    private enum TriggerType { ENABLE, DISABLE }
    [SerializeField]
    private TriggerType triggerType = TriggerType.ENABLE;

    public void OnEnableTrigger()
    {
        triggerEnabled = true;
    }
    
    // if touched triggerEnablePoint, enable trigger after 1s
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerEnabled && other.CompareTag("Player"))
        {
            if (triggerType == TriggerType.ENABLE)
            {
                Debug.Log("TriggerPoint: Enable trigger");
                EnableDoors(triggerEnableDelay);
            }
            else if (triggerType == TriggerType.DISABLE)
            {
                Debug.Log("TriggerPoint: Disable trigger");
                DisableDoors(triggerEnableDelay);
            }
        }
    }


    public void DisableDoors()
    {
        doors.SetActive(false);
    }

    public void DisableDoors(float delay)
    {
        if (delay < Mathf.Epsilon)
        {
            DisableDoors();
            return;
        }
        StartCoroutine(DisableDoorsHelper(delay));
    }

    private IEnumerator DisableDoorsHelper(float delay)
    {
        yield return new WaitForSeconds(delay);
        DisableDoors();
    }

    public void EnableDoors()
    {
        doors.SetActive(true);
    }

    public void EnableDoors(float delay)
    {
        if (delay < Mathf.Epsilon)
        {
            EnableDoors();
            return;
        }
        StartCoroutine(EnableDoorsHelper(delay));
    }

    private IEnumerator EnableDoorsHelper(float delay)
    {
        yield return new WaitForSeconds(delay);
        EnableDoors();
    }

    
}
