using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelLoadTrigger : MonoBehaviour
{
    [SerializeField]
    private int specificLevel = -1;

    public UnityEvent onLoadingLevel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LoadLevel();
        }
    }

    public void LoadLevel()
    {
        onLoadingLevel.Invoke();
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
