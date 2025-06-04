using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    [Header("ü�� ����")]
    [SerializeField] private int maxHealth = 30;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[�浹 ����] �浹�� ������Ʈ: {other.name}");
        // "Attack" �±׸� ���� ������Ʈ�� ó��
        if (!other.CompareTag("Attack")) return;

        // BulletSpawn ������Ʈ�� �ִ��� Ȯ�� (TryGetComponent ���)
        if (other.TryGetComponent(out BulletSpawn bullet))
        {
            TakeDamage(bullet.attackPower);
        }

        // �Ѿ� ��Ȱ��ȭ (������Ʈ Ǯ�� �ǵ���)
        other.gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} �ǰ�! ���� ü��: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} ���");
        gameObject.SetActive(false);
    }
}
