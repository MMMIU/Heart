using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopeRespawnPoint : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement;
    [SerializeField]
    private GameObject bossGO;
    private Boss boss;
    private SpiritControl bossSpiritControl;

    // hook player die event
    private void OnEnable()
    {
        playerMovement.OnPlayerDie.AddListener(RespawnPlayer);
        boss = bossGO.GetComponent<Boss>();
        bossSpiritControl = bossGO.GetComponent<SpiritControl>();
    }

    // unhook player die event
    private void OnDisable()
    {
        playerMovement.OnPlayerDie.RemoveListener(RespawnPlayer);
    }

    // respawn player
    private void RespawnPlayer()
    {
        playerMovement.transform.position = transform.position;
        // disable gameobject
        gameObject.SetActive(false);
        boss.TakeDamage(10086);
        bossSpiritControl.AttachToPlayer();
    }
}
