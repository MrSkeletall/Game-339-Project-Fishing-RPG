using UnityEngine;

public class FishManager : MonoBehaviour
{
    public FishObj fishData;
    private GameObject hookObject;

    public enum FishState
    {
        Idle,
        Lured,
        Hooked,
        Running
    }
    
    public FishState currentState = FishState.Idle;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckBounds();
        
        switch (currentState)
        {
            case FishState.Idle:
                UpdateIdleState();
                break;
            case FishState.Lured:
                UpdateLuredState();
                break;
        }
    }

    private void CheckBounds()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        bool moved = false;

        if (viewportPos.x < 0)
        {
            viewportPos.x = 1;
            moved = true;
        }
        else if (viewportPos.x > 1)
        {
            viewportPos.x = 0;
            moved = true;
        }

        if (viewportPos.y < 0)
        {
            viewportPos.y = 1;
            moved = true;
        }
        else if (viewportPos.y > 1)
        {
            viewportPos.y = 0;
            moved = true;
        }

        if (moved)
        {
            Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewportPos);
            worldPos.z = 0; // Maintain 2D Z position
            transform.position = worldPos;
        }
    }

    private void UpdateIdleState()
    {
        if (fishData != null && rb != null)
        {
            // Simple random movement for testing idle state and bounds
            rb.linearVelocity = transform.up * fishData.speed;
        }
    }

    public void SetLured(GameObject hook)
    {
        hookObject = hook;
        currentState = FishState.Lured;
    }

    private void UpdateLuredState()
    {
        if (hookObject != null && rb != null)
        {
            Vector2 direction = (hookObject.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * fishData.speed;
            
            // Rotate to face the hook
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // If close enough to hook, maybe transition to Hooked? 
            // For now just keep following.
        }
        else
        {
            currentState = FishState.Idle;
        }
    }

    private void EnterHookedState()
    {
        
    }

    private void ExitHookedState()
    {
        
    }

    private void UpdateHookedState()
    {
        
    }
    
}
