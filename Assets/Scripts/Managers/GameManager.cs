using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField]
    private int currLevel = 0;

    public UnityEvent OnLoadingNextLevel;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }

    public void LoadNextLevel()
    {
        currLevel++;
        OnLoadingNextLevel?.Invoke();
    }

    public int GetCurrLevel()
    {
        return currLevel;
    }
    
    public void LoadSpecificLevel(int specificLevel)
    {
        currLevel = specificLevel;
        OnLoadingNextLevel?.Invoke();
    }
}
