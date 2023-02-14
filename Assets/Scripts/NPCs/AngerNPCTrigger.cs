using UnityEngine;
using PixelCrushers.DialogueSystem;

public class AngerNPCTrigger : MonoBehaviour
{
    [Tooltip("Typically leave unticked so temporary Dialogue Managers don't unregister your functions.")]
    public bool unregisterOnDisable = false;
    
    public Sprite angrySprite;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        // Make the functions available to Lua: (Replace these lines with your own.)
        Lua.RegisterFunction(nameof(VeryAngry), this, SymbolExtensions.GetMethodInfo(() => VeryAngry()));
    }

    void OnDisable()
    {
        if (unregisterOnDisable)
        {
            // Remove the functions from Lua: (Replace these lines with your own.)
            Lua.UnregisterFunction(nameof(VeryAngry));
        }
    }

    public void VeryAngry()
    {
        spriteRenderer.sprite = angrySprite;
    }
}
