using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishingGame : MonoBehaviour
{
    //the hook object and script
    public GameObject fishHook;
    public FishingHook currentHookData;
    
    //where the cast is spawned from
    public Transform FishingRod;

    //speed of cast and return
    public float castForce = 10f;
    public float reelInSpeed = 10f;
    public float lureRadius = 10f;
    
    bool hookOut = false;
    private GameObject activeHook;
    private InputAction clickAction;
    private InputAction rightClickAction;
    private bool isReeling = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clickAction = InputSystem.actions.FindAction("Click");
        rightClickAction = InputSystem.actions.FindAction("RightClick");
    }

    // Update is called once per frame
    void Update()
    {
        if (clickAction != null && clickAction.WasPressedThisFrame())
        {
            if (!hookOut)
            {
                CastHook();
            }
            else if (!isReeling)
            {
                ReelInHook();
            }
        }

        if (rightClickAction != null && rightClickAction.WasPressedThisFrame())
        {
            if (hookOut && !isReeling)
            {
                LureFish();
            }
        }
    }


    public void CastHook()
    {
        hookOut = true;
        activeHook = Instantiate(fishHook, FishingRod.position, FishingRod.rotation);
        if (!activeHook.activeInHierarchy)
        {
            activeHook.SetActive(true);
        }
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0; // Ensure 2D position for 2D calculation

        Vector2 direction = (mouseWorldPos - FishingRod.position).normalized;

        Rigidbody2D rb = activeHook.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(direction * castForce, ForceMode2D.Impulse);
        }
    }

    public void ReelInHook()
    {
        if (hookOut && !isReeling)
        {
            StartCoroutine(ReelInCoroutine());
        }
    }

    private System.Collections.IEnumerator ReelInCoroutine()
    {
        isReeling = true;
        
        if (activeHook != null)
        {
            Rigidbody2D rb = activeHook.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.isKinematic = true;
            }

            while (activeHook != null && Vector3.Distance(activeHook.transform.position, FishingRod.position) > 0.1f)
            {
                activeHook.transform.position = Vector3.MoveTowards(
                    activeHook.transform.position, 
                    FishingRod.position, 
                    reelInSpeed * Time.deltaTime
                );
                yield return null;
            }

            if (activeHook != null)
            {
                activeHook.gameObject.SetActive(false);
                Destroy(activeHook, 3f);
            }
        }

        hookOut = false;
        isReeling = false;
    }

    public void LureFish()
    {
        //circle check for fish in range and set them to lured state
        if (activeHook != null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(activeHook.transform.position, lureRadius);
            foreach (Collider2D col in colliders)
            {
                FishManager fish = col.GetComponent<FishManager>();
                if (fish != null)
                {
                    fish.SetLured(activeHook);
                }
            }
            TriggerHookParticles();
        }
    }
    
    
    
    public void TriggerHookParticles()
    {
        if (activeHook != null)
        {
            FishingHook hookScript = activeHook.GetComponentInChildren<FishingHook>();
            if (hookScript != null)
            {
                hookScript.TriggerParticles();
            }
        }
    }
    
}
