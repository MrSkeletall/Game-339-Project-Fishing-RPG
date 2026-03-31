using UnityEngine;
using Pathfinding;

public class FishFighter : MonoBehaviour
{

    public FishObj fishData;
    public Rigidbody2D rb;
    private CombatState combatState;
    private FishFighter enemyFish;

    [Header("Combat Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 1.0f;
    public float retreatDistance = 5f;
    public float dodgeChance = 0.3f;

    [Header("A* Pathfinding")]
    public Seeker seeker;
    public float nextWaypointDistance = 0.5f;
    public float repathRate = 0.5f;

    // Internal state
    private float currentHealth;
    private float lastAttackTime;
    private float lastRepathTime;
    private Path currentPath;
    private int currentWaypoint = 0;
    private Vector2 targetPosition;
    private float stateTimer;
    private float decisionTimer;
    private float decisionInterval = 0.5f;
    private bool isAttackActive = false;

    private enum CombatState
    {
        Idle,
        Attacking,
        Moving,
        Running,
        Dead
    }

    void Start()
    {
        fishData.setFishSize(gameObject.transform);
        currentHealth = fishData.maxHealth;
        combatState = CombatState.Idle;
        lastAttackTime = -attackCooldown;

        if (seeker == null)
            seeker = GetComponent<Seeker>();

        FindEnemyFish();
    }

    void Update()
    {
        if (combatState == CombatState.Dead) return;

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        decisionTimer += Time.deltaTime;
        if (decisionTimer >= decisionInterval)
        {
            decisionTimer = 0;
            MakeDecision();
        }

        ExecuteState();
        stateTimer -= Time.deltaTime;
    }

    void MakeDecision()
    {
        if (enemyFish == null || enemyFish.combatState == CombatState.Dead)
        {
            combatState = CombatState.Idle;
            return;
        }

        float distanceToEnemy = Vector2.Distance(transform.position, enemyFish.transform.position);
        float healthPercent = currentHealth / fishData.maxHealth;
        float randomFactor = Random.Range(0f, 1f);

        bool shouldRun = healthPercent < 0.3f && randomFactor > (fishData.luck / 100f);

        if (shouldRun)
        {
            combatState = CombatState.Running;
            ChooseRetreatPosition();
            RequestPath(targetPosition);
            stateTimer = Random.Range(2f, 4f);
        }
        else if (distanceToEnemy <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            combatState = CombatState.Attacking;
            isAttackActive = true;
            targetPosition = enemyFish.transform.position;
            RequestPath(targetPosition);
            stateTimer = 0.5f;
        }
        else if (randomFactor < 0.3f)
        {
            combatState = CombatState.Moving;
            ChooseMovementPosition();
            RequestPath(targetPosition);
            stateTimer = Random.Range(1f, 3f);
        }
        else
        {
            combatState = CombatState.Attacking;
            isAttackActive = false;
            targetPosition = enemyFish.transform.position;
            RequestPath(targetPosition);
        }
    }

    void ExecuteState()
    {
        switch (combatState)
        {
            case CombatState.Idle:
                rb.linearVelocity = Vector2.zero;
                break;

            case CombatState.Attacking:
                FollowPath(fishData.speed);
                break;

            case CombatState.Moving:
                FollowPath(fishData.speed * 0.7f);
                if (Vector2.Distance(transform.position, targetPosition) < 1f || stateTimer <= 0)
                    MakeDecision();
                break;

            case CombatState.Running:
                FollowPath(fishData.speed * 1.3f);
                if (Vector2.Distance(transform.position, targetPosition) < 1f || stateTimer <= 0)
                    MakeDecision();
                break;
        }
    }

    void RequestPath(Vector2 target)
    {
        if (seeker != null && !seeker.IsDone() && Time.time < lastRepathTime + repathRate)
            return;

        lastRepathTime = Time.time;

        if (seeker != null)
            seeker.StartPath(transform.position, target, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            currentPath = p;
            currentWaypoint = 0;
        }
    }

    void FollowPath(float speed)
    {
        if (currentPath == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (currentWaypoint >= currentPath.vectorPath.Count)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = ((Vector2)currentPath.vectorPath[currentWaypoint] - (Vector2)transform.position).normalized;

        float randomVariance = Random.Range(-0.2f, 0.2f);
        Vector2 randomOffset = new Vector2(randomVariance, randomVariance);
        direction = (direction + randomOffset).normalized;

        rb.linearVelocity = direction * speed;
        FaceTarget(currentPath.vectorPath[currentWaypoint]);

        float distance = Vector2.Distance(transform.position, currentPath.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (combatState == CombatState.Attacking && Time.time > lastRepathTime + repathRate * 0.5f)
        {
            RequestPath(enemyFish.transform.position);
        }
    }

    void ChooseMovementPosition()
    {
        if (enemyFish == null) return;

        Vector2 enemyPos = enemyFish.transform.position;
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float distance = Random.Range(attackRange * 0.8f, attackRange * 2f);

        targetPosition = enemyPos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
        
    }

    void ChooseRetreatPosition()
    {
        if (enemyFish == null) return;

        Vector2 directionAway = ((Vector2)transform.position - (Vector2)enemyFish.transform.position).normalized;
        targetPosition = (Vector2)transform.position + directionAway * retreatDistance;
        //targetPosition = ClampToArena(targetPosition);
    }

    
    void FaceTarget(Vector2 target)
    {
        Vector2 direction = target - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAttackActive || combatState != CombatState.Attacking)
            return;

        FishFighter otherFish = collision.gameObject.GetComponent<FishFighter>();

        if (otherFish != null && otherFish == enemyFish)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                PerformAttack();
                lastAttackTime = Time.time;
                isAttackActive = false;
            }
        }
    }

    void PerformAttack()
    {
        if (enemyFish == null) return;

        float baseDamage = fishData.strength;
        float damageVariance = Random.Range(0.8f, 1.2f);
        float criticalHit = Random.Range(0f, 1f) < (fishData.luck / 100f) ? 1.5f : 1.0f;
        float finalDamage = baseDamage * damageVariance * criticalHit;

        bool dodged = Random.Range(0f, 1f) < (enemyFish.fishData.luck / 100f) * dodgeChance;

        if (!dodged)
        {
            enemyFish.TakeDamage(finalDamage);
        }

        Vector2 knockbackDir = ((Vector2)enemyFish.transform.position - (Vector2)transform.position).normalized;
        enemyFish.rb.AddForce(knockbackDir * 5f, ForceMode2D.Impulse);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        fishData.health = currentHealth;
    }

    void Die()
    {
        combatState = CombatState.Dead;
        rb.linearVelocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    void FindEnemyFish()
    {
        FishFighter[] allFish = FindObjectsOfType<FishFighter>();
        foreach (FishFighter fish in allFish)
        {
            if (fish != this)
            {
                enemyFish = fish;
                break;
            }
        }
    }

    public void SetEnemy(FishFighter enemy)
    {
        enemyFish = enemy;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        //Gizmos.DrawWireCube((arenaMin + arenaMax) / 2, arenaMax - arenaMin);
    }
}
