using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritControl : MonoBehaviour
{

    // Variables
    public Vector2 offset;
    public Sprite spiritSprite;
    
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        // Get components and player
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }
    
    public void AttachToPlayer()
    {
        // Change sprite to spirit sprite
        spriteRenderer.sprite = spiritSprite;
        // Attach to player
        transform.parent = player.transform;
        // Disable animator
        animator.enabled = false;
        // Disable rigidbody
        rb.isKinematic = true;
        // Disable collider
        coll.enabled = false;
        // Move spirit to player and make it hover over the player
        transform.position = player.transform.position;
        transform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z);
    }
    
}
