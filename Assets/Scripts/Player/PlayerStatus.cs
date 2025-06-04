using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    // 체력 관련 필드
    [Header("Health")]
    public int CurrentHealth = 100;
    public int MaxHealth = 100;
    public bool IsDead { get; private set; } = false;
    public bool IsHit { get; private set; } = false;

    //무적 상태 관련
    [Header("Invincibility")]
    [SerializeField] private float untouchableTime = 2f;
    private bool isUntouchable = false;
    private float timeSinceLastHit = 0f;

    private Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        tag = "Player";
    }

    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    void Update()
    {
        timeSinceLastHit += Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead || IsHit) return;

        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            PlayerDeath();
            return;
        }

        animator?.SetTrigger("HitTrigger");
        StartCoroutine(HitLock());
    }

    private void PlayerDeath()
    {
        if (IsDead) return;
        IsDead = true;

        animator?.SetTrigger("DeathTrigger");

        var moveScript = GetComponent<PlayerMovement>();
        if (moveScript != null) moveScript.enabled = false;

        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
        Debug.Log($"체력 회복: +{amount}, 현재 체력: {CurrentHealth}/{MaxHealth}");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Monster") && timeSinceLastHit >= untouchableTime)
        {
            TakeDamage(1);
            timeSinceLastHit = 0f;
        }
    }

    private IEnumerator HitLock(float duration = 0.4f)
    {
        IsHit = true;
        yield return new WaitForSeconds(duration);
        IsHit = false;
    }

    private IEnumerator UntouchableTime()
    {
        isUntouchable = true;
        yield return new WaitForSeconds(untouchableTime);
        isUntouchable = false;
    }

    public int GetCurrentHealth() => CurrentHealth;
    public int GetMaxHealth() => MaxHealth;

    public bool IsUntouchable
    {
        get => isUntouchable;
        set
        {
            if (value && !isUntouchable)
                StartCoroutine(UntouchableTime());
        }
    }
}
