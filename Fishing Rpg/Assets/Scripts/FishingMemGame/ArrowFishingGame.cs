using System.Collections;
using UnityEngine;

public class ArrowFishingGame : MonoBehaviour
{
    public GameObject upArrow;
    public GameObject downArrow;
    public GameObject leftArrow;
    public GameObject rightArrow;
    public Sprite arrowFlashSprite;
    public GameObject fishingRod;


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
    public int MaxPatternLength = 6;
    public int MinPatternLength = 4;
    public float flashTime = 0.5f;
    


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
        startArrowGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void startArrowGame()
    {
        arrowPattern = GeneratePattern();
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
    }


}
