using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyBlob : EnemyAI, IBlob
{
    [SerializeField]
    float knockbackForce = 2f;
    [SerializeField]
    bool ableToAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        FindPlayerPos();
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (ableToAttack && CheckForPlayer)
        {
            Attack();
            ableToAttack = false;
            StartCoroutine(SetAbleToAttack());
        }
    }

    // set ableToAttack after time
    IEnumerator SetAbleToAttack()
    {
        yield return new WaitForSeconds(EnemyBlobMovement.GetJumpReloadTimeInSec());
        ableToAttack = true;
    }

    public void Attack()
    {
        ApplyKnockBack();
        // do damage to player
        playerMovement.TakeDamage(1);
    }

    void ApplyKnockBack()
    {
        // apply knockback
        Rigidbody2D playerRb = playerPos.gameObject.GetComponent<Rigidbody2D>();
        Vector2 knockbackDir = (playerPos.transform.position - transform.position);
        knockbackDir.y = 0;

        playerRb.AddForce(knockbackDir.normalized * knockbackForce, ForceMode2D.Impulse);
    }

    public void Death(float deathWaitTime = 0)
    {
        Destroy(gameObject, deathWaitTime);
    }

    public void TakeDamage(float damage)
    {
        currHealth -= damage;

        if (currHealth <= 0)
        {
            Death();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + offset, playerDetectionRadius);
    }
}
