using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField, OptionalField]
    ParticleSystem hitEffect;
    [SerializeField, OptionalField]
    ParticleSystem deathEffect;
    [SerializeField]
    protected float maxHealth;
    [SerializeField]
    LayerMask playerLayer;

    [Header("Player detection")]
    [SerializeField]
    protected float playerDetectionRadius;
    [SerializeField]
    protected Vector3 offset;

    [SerializeField]
    protected float currHealth;
    protected Transform playerPos;
    protected PlayerMovement playerMovement;

    private void Start()
    {
        FindPlayerPos();
    }

    protected void FindPlayerPos()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        playerMovement = playerPos.GetComponent<PlayerMovement>();
    }

    protected bool CheckForPlayer => Physics2D.OverlapCircle(transform.position + offset, playerDetectionRadius, playerLayer);
}
