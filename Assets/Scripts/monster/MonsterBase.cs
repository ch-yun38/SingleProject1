using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    [Header("체력 설정")]
    [SerializeField] private int maxHealth = 30;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[충돌 감지] 충돌한 오브젝트: {other.name}");
        // "Attack" 태그를 가진 오브젝트만 처리
        if (!other.CompareTag("Attack")) return;

        // BulletSpawn 컴포넌트가 있는지 확인 (TryGetComponent 사용)
        if (other.TryGetComponent(out BulletSpawn bullet))
        {
            TakeDamage(bullet.attackPower);
        }

        // 총알 비활성화 (오브젝트 풀로 되돌림)
        other.gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} 피격! 남은 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} 사망");
        gameObject.SetActive(false);
    }
}
