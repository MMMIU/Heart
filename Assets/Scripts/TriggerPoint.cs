using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerPoint : MonoBehaviour
{
    [SerializeField]
    private LevelManager levelManager;
    [SerializeField]
    private float triggerEnableDelay = 1f;
    [SerializeField]
    private enum TriggerType { ENABLE, DISABLE }
    [SerializeField]
    private TriggerType triggerType = TriggerType.ENABLE;

    // if touched triggerEnablePoint, enable trigger after 1s
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (triggerType == TriggerType.ENABLE)
            {
                Debug.Log("TriggerPoint: Enable trigger");
                levelManager?.EnableDoors(triggerEnableDelay);
            }
            else if (triggerType == TriggerType.DISABLE)
            {
                Debug.Log("TriggerPoint: Disable trigger");
                levelManager?.DisableDoors(triggerEnableDelay);
            }
        }
    }

}
