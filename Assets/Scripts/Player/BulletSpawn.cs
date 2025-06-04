using System.Collections.Generic;
using UnityEngine;

public class BulletSpawn : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public int attackPower = 1;

    private Vector3 bulletDir;
    private Vector3 bulletPos;

    private static List<BulletSpawn> pool = new List<BulletSpawn>();

    [SerializeField] private float maxDistance = 10f;

    // 풀에서 꺼내기
    public static BulletSpawn Spawn(BulletSpawn prefab, Vector3 pos, Quaternion rot)
    {
        BulletSpawn instance;

        if (pool.Count > 0)
        {
            instance = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
        }
        else
        {
            instance = Instantiate(prefab);
        }

        instance.transform.position = pos;
        instance.transform.rotation = rot;
        instance.gameObject.SetActive(true);

        return instance;
    }

    //풀에 반환
    private void ReturnPool()
    {
        gameObject.SetActive(false);
        if (!pool.Contains(this)) pool.Add(this); //중복 추가 방지
    }

    public void BulletStartDirection(Vector3 dir)
    {
        bulletDir = dir.normalized;
    }

    private void OnEnable()
    {
        bulletPos = transform.position;
    }

    private void Update()
    {
        transform.position += bulletDir * bulletSpeed * Time.deltaTime;
        transform.Rotate(0f, 0f, 360f * Time.deltaTime);

        // 일정 거리 이상 날아가면 제거
        if (Vector3.Distance(bulletPos, transform.position) > maxDistance)
        {
            ReturnPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            MonsterBase monster = other.GetComponent<MonsterBase>();
            if (monster != null)
            {
                monster.TakeDamage(attackPower);
            }

            // 피격 몬스터가 있어도 ReturnPool 처리
            ReturnPool();
        }
        else if (!other.CompareTag("Player"))
        {
            ReturnPool();
        }
    }
}
