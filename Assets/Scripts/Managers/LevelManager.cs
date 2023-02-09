using Cinemachine;
using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    [Header("Game Manager: No need to reference here")] 
    [SerializeField]
    private GameManager gameManager;

    [Header("Level Settings")]
    [SerializeField]
    private int levelNumber = -1;

    [Header("Boss Settings")]
    [SerializeField]
    private bool hasBoss = false;
    [SerializeField]
    private Transform bossSpawnPoint;
    [SerializeField]
    private GameObject bossPrefab;

    private GameObject bossInstance;

    [Header("Triggers Settings")]
    [SerializeField]
    private GameObject doors;

    private bool LevelCompletenessCheck()
    {
        // check game manager
        if (gameManager == null)
        {
            Debug.LogError("Game manager is null");
            return false;
        }

        // check level
        if (levelNumber < 0)
        {
            Debug.LogError("Level number is invalid");
            return false;
        }

        // check trigger
        if (doors == null)
        {
            Debug.LogError("Trigger is null");
            return false;
        }

        return true;
    }

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (!LevelCompletenessCheck())
        {
            // Quit editor
            Debug.LogError("Level compromised! Quiting...");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
        }
    }

    private void OnEnable()
    {
        gameManager.OnLoadingNextLevel.AddListener(OnLoadingNextLevel);
    }

    private void OnDisable()
    {
        gameManager.OnLoadingNextLevel.RemoveListener(OnLoadingNextLevel);
    }

    private void OnLoadingNextLevel()
    {
        if (gameManager.GetCurrLevel() == levelNumber)
        {
            Debug.Log("Level " + levelNumber + " is enabled.");
            if (hasBoss && bossPrefab != null)
            {
                InitBoss();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void InitBoss()
    {
        if (bossPrefab == null || bossSpawnPoint == null)
        {
            Debug.Log("No boss to spawn");
            return;
        }
        bossInstance = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation, transform);
        bossInstance.name = "Boss " + levelNumber;
    }

    public void TriggerBoss()
    {
        if (bossInstance == null)
        {
            Debug.LogError("Boss instance is null");
            return;
        }
        Boss boss = bossInstance.GetComponent<Boss>();

        boss.SetBossTriggered(true);
        boss.OnBossDefeated.AddListener(() =>
        {
            Debug.Log("LevelManager knows Boss defeated! Enabling camera trigger!");
        });
    }

    public CinemachineVirtualCamera getLevelCamera()
    {
        return GetComponentInChildren<CinemachineVirtualCamera>();
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
