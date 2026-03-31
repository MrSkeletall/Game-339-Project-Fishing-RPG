using UnityEngine;

public class FishFightManager : MonoBehaviour
{
    [Header("Fish Fighters")]
    public FishFighter fish1;
    public FishFighter fish2;

    [Header("Combat Settings")]
    public bool autoStartCombat = true;
    public float combatStartDelay = 1f;

    private bool combatStarted = false;

    void Start()
    {
        if (autoStartCombat)
        {
            Invoke(nameof(StartCombat), combatStartDelay);
        }
    }

    public void StartCombat()
    {
        if (combatStarted)
        {
            Debug.LogWarning("Combat has already started!");
            return;
        }

        if (fish1 == null || fish2 == null)
        {
            Debug.LogError("Cannot start combat - both fish must be assigned!");
            return;
        }

        fish1.SetEnemy(fish2);
        fish2.SetEnemy(fish1);

        combatStarted = true;
        Debug.Log($"Combat started between {fish1.fishData.fishName} and {fish2.fishData.fishName}");
    }

    void Update()
    {
        if (combatStarted)
        {
            CheckCombatEnd();
        }
    }

    void CheckCombatEnd()
    {
        if (fish1 == null || fish2 == null)
            return;

        if (fish1.fishData.health <= 0)
        {
            OnCombatEnd(fish2, fish1);
        }
        else if (fish2.fishData.health <= 0)
        {
            OnCombatEnd(fish1, fish2);
        }
    }

    void OnCombatEnd(FishFighter winner, FishFighter loser)
    {
        combatStarted = false;
        Debug.Log($"Combat ended! {winner.fishData.fishName} defeated {loser.fishData.fishName}");
    }

    public void ResetCombat()
    {
        combatStarted = false;

        if (fish1 != null)
        {
            fish1.fishData.health = fish1.fishData.maxHealth;
            fish1.gameObject.SetActive(true);
        }

        if (fish2 != null)
        {
            fish2.fishData.health = fish2.fishData.maxHealth;
            fish2.gameObject.SetActive(true);
        }
    }
}
