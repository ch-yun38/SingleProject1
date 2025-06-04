using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    [Header("체력 설정")]
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;

    [Header("추적 설정")]
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

    // 빛 쏴서 플레이어 태그 대조
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


    // 게임 시작 시 플레이어의 태그 확인 및 위치 확인
    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerPos = player.transform;
        }
    }

    // 플레이어 위치 기반 몬스터 움직임 및 응시
    private void MoveToPlayer()
    {
        Vector3 targetPos = new Vector3(playerPos.position.x, transform.position.y, playerPos.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        transform.LookAt(targetPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        //  피격 처리
        if (other.CompareTag("Attack") && other.TryGetComponent(out BulletSpawn bullet))
        {
            TakeDamage(bullet.attackPower);
            other.gameObject.SetActive(false);
        }

        // 탐지 범위 진입
        if (other.CompareTag("Player"))
        {
            isPlayerDetected = true;
            Debug.Log($"[탐지] 플레이어 발견! → {gameObject.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 탐지 범위 이탈
        if (other.CompareTag("Player"))
        {
            isPlayerDetected = false;
            Debug.Log($"[탐지 해제] 플레이어 사라짐 → {gameObject.name}");
        }
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
        FindObjectOfType<GameManagerProj>().AddKill();
        gameObject.SetActive(false);
    }
}
