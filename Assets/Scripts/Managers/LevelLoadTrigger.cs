using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoadTrigger : MonoBehaviour
{
    [SerializeField]
    private int specificLevel = -1;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (specificLevel == -1)
            {
                GameManager.instance.LoadNextLevel();
            }
            else
            {
                GameManager.instance.LoadSpecificLevel(specificLevel);
            }
        }
    }
}
