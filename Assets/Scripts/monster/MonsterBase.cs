using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    [Header("ü�� ����")]
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;

    [Header("���� ����")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float detectRange = 5f;
    [SerializeField] private LayerMask detectLayer;


    private Animator animator;
    private bool isPlayerDetected = false;
    private Transform playerPos;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        FindPlayer();
    }

    private void Update()
    {

        DitectPlayer();
        animator.SetBool("isDetected", isPlayerDetected);

        if (isPlayerDetected && playerPos != null)
        {
            MoveToPlayer();
        }
    }

    // �� ���� �÷��̾� �±� ����
    private void DitectPlayer()
    {
        isPlayerDetected = false;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectRange, detectLayer);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            Collider collider = hitColliders[i];
            if (collider.CompareTag("Player"))
            {
                isPlayerDetected = true;

                break;
            }
        }
    }


    // ���� ���� �� �÷��̾��� �±� Ȯ�� �� ��ġ Ȯ��
    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerPos = player.transform;
        }
    }

    // �÷��̾� ��ġ ��� ���� ������ �� ����
    private void MoveToPlayer()
    {
        Vector3 targetPos = new Vector3(playerPos.position.x, transform.position.y, playerPos.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        transform.LookAt(targetPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        //  �ǰ� ó��
        if (other.CompareTag("Attack") && other.TryGetComponent(out BulletSpawn bullet))
        {
            TakeDamage(bullet.attackPower);
            other.gameObject.SetActive(false);
        }

        // Ž�� ���� ����
        if (other.CompareTag("Player"))
        {
            isPlayerDetected = true;
            Debug.Log($"[Ž��] �÷��̾� �߰�! �� {gameObject.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Ž�� ���� ��Ż
        if (other.CompareTag("Player"))
        {
            isPlayerDetected = false;
            Debug.Log($"[Ž�� ����] �÷��̾� ����� �� {gameObject.name}");
        }
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
        FindObjectOfType<GameManagerProj>().AddKill();
        gameObject.SetActive(false);
    }
}
