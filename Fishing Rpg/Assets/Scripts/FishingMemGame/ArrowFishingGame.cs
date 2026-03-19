using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowFishingGame : MonoBehaviour
{
    public GameObject upArrow;
    public GameObject downArrow;
    public GameObject leftArrow;
    public GameObject rightArrow;
    public Sprite arrowFlashSprite;
    public Sprite arrowDefaultSprite;
    
    
    //visual
    public GameObject fishingRod;
    public GameObject fishingHook;
    public FishingLine fishingLine;
    public GameObject fishSprite;

    private enum ArrowState
    {
        Up,
        Down,
        Left,
        Right,
        
    }
    
    //stores pattern for arrows
    private ArrowState[] arrowPattern;
    private int currentPatternIndex = 0;
    private ArrowState currentArrowState;
    public int MaxPatternLength = 8;
    public int MinPatternLength = 5;
    public float flashTime = 0.5f;

    private bool isCollectingInput = false;
    private ArrowState[] playerPattern;
    private int playerPatternIndex = 0;
    
    
    //input actions
    private InputAction clickAction;
    private InputAction rightClickAction;
    private InputAction moveAction;
    private InputAction jumpAction;


    private ArrowState[] GeneratePattern()
    {
        int currentPatternLength = GetRandomPatternLength();
        ArrowState[] pattern = new ArrowState[currentPatternLength];
        for (int i = 0; i < currentPatternLength; i++)
        {
            pattern[i] = (ArrowState)Random.Range(0, 4);
        }
        Debug.Log("Pattern generated: " + string.Join(", ", pattern));
        return pattern;
        
    }

    private int GetRandomPatternLength()
    {
        return Random.Range(MinPatternLength, MaxPatternLength);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clickAction = InputSystem.actions.FindAction("Click");
        rightClickAction = InputSystem.actions.FindAction("RightClick");
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        fishingRod = GameObject.Find("Fishing Rod");
        fishingHook = GameObject.Find("Hook");
        fishingLine = fishingHook.GetComponent<FishingLine>();
        
        fishingLine.EnableLine(true);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpAction.WasPressedThisFrame())
        {
            startArrowGame();
        }

        if (isCollectingInput)
        {
            Vector2 moveInput = moveAction.ReadValue<Vector2>();
            if (moveInput != Vector2.zero && moveAction.WasPressedThisFrame())
            {
                ArrowState? input = getPlayerInputToArrowPattern(moveInput);
                if (input.HasValue)
                {
                    playerPattern[playerPatternIndex] = input.Value;
                    playerPatternIndex++;

                    // Flash the arrow to give feedback
                    GameObject arrow = getArrowFromState(input.Value);
                    StartCoroutine(FlashSingleArrow(arrow));

                    // Check if pattern is complete
                    if (playerPatternIndex >= arrowPattern.Length)
                    {
                        isCollectingInput = false;
                        bool isCorrect = ComparePatterns();
                        CatchFish();
                        Debug.Log("Pattern " + (isCorrect ? "CORRECT!" : "INCORRECT!"));
                    }
                }
            }
        }
    }

    

    void startArrowGame()
    {
        arrowPattern = GeneratePattern();
        HookEnabled(true);
        currentPatternIndex = 0;
        currentArrowState = arrowPattern[currentPatternIndex];
        StartCoroutine(FlashArrowPattern(arrowPattern));
        
    }

    private void swapArrowSprite()
    {
        GameObject arrow = getArrowFromState(currentArrowState);
        arrow.GetComponent<SpriteRenderer>().sprite = arrowFlashSprite;
    }
    
    private void resetArrowSprite()
    {
        GameObject arrow = getArrowFromState(currentArrowState);
        arrow.GetComponent<SpriteRenderer>().sprite = arrow.GetComponent<SpriteRenderer>().sprite;
    }

    private GameObject getArrowFromState(ArrowState state)
    {
        switch (state)
        {
            case ArrowState.Up:
                return upArrow;
            case ArrowState.Down:
                return downArrow;
            case ArrowState.Left:
                return leftArrow;
            case ArrowState.Right:
                return rightArrow;
            default:
                return null;
        }

    }

    private IEnumerator FlashArrowPattern(ArrowState[] pattern)
    {
        foreach (ArrowState state in pattern)
        {
            GameObject arrow = getArrowFromState(state);
            SpriteRenderer spriteRenderer = arrow.GetComponent<SpriteRenderer>();

            // Store original sprite
            Sprite originalSprite = spriteRenderer.sprite;

            // Swap to flash sprite
            spriteRenderer.sprite = arrowFlashSprite;

            // Wait for flashTime duration
            yield return new WaitForSeconds(flashTime);

            // Restore original sprite
            spriteRenderer.sprite = originalSprite;
        }

        // After showing pattern, prompt player to enter it
        PromptPlayerPattern(pattern);
    }

    private IEnumerator FlashSingleArrow(GameObject arrow)
    {
        SpriteRenderer spriteRenderer = arrow.GetComponent<SpriteRenderer>();
        Sprite originalSprite = spriteRenderer.sprite;

        spriteRenderer.sprite = arrowFlashSprite;
        yield return new WaitForSeconds(flashTime);
        spriteRenderer.sprite = originalSprite;
    }

    private void ResetArrowSprites()
    {
        GameObject[] arrows = { upArrow, downArrow, leftArrow, rightArrow };
        foreach (GameObject arrow in arrows)
        {
            arrow.GetComponent<SpriteRenderer>().sprite = arrowDefaultSprite;
        }
    }

    private void PromptPlayerPattern(ArrowState[] currentPattern)
    {
        ResetArrowSprites();
        playerPattern = new ArrowState[currentPattern.Length];
        playerPatternIndex = 0;
        isCollectingInput = true;
    }

    private ArrowState? getPlayerInputToArrowPattern(Vector2 moveInput)
    {
        // Ensure only one direction is pressed
        if (moveInput.x == -1 && moveInput.y == 0)
        {
            return ArrowState.Left;
        }
        if (moveInput.x == 1 && moveInput.y == 0)
        {
            return ArrowState.Right;
        }
        if (moveInput.x == 0 && moveInput.y == -1)
        {
            return ArrowState.Down;
        }
        if (moveInput.x == 0 && moveInput.y == 1)
        {
            return ArrowState.Up;
        }

        return null;
    }

    private bool ComparePatterns()
    {
        for (int i = 0; i < arrowPattern.Length; i++)
        {
            if (playerPattern[i] != arrowPattern[i])
            {
                return false;
            }
        }
        return true;
    }


    public void CatchFish()
    {
        
        HookEnabled(false);
        ThrowVisualFish();
        
        
    }

    private void ThrowVisualFish()
    {
        GameObject fish = Instantiate(fishSprite, fishingHook.transform.position, Quaternion.identity);
        Rigidbody2D rbFish = fish.GetComponent<Rigidbody2D>();
        rbFish.AddForce(fishingRod.transform.up * 1000f, ForceMode2D.Impulse);
        
    }
    
    private void HookEnabled(bool state)
    {
        fishingHook.SetActive(state);
        fishingLine.EnableLine(state);
    }

}
