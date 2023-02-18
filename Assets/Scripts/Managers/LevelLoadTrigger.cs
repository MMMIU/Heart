using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelLoadTrigger : MonoBehaviour
{
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
    }
}
