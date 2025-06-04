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

    // Ǯ���� ������
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

    //Ǯ�� ��ȯ
    private void ReturnPool()
    {
        gameObject.SetActive(false);
        if (!pool.Contains(this)) pool.Add(this); //�ߺ� �߰� ����
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

        // ���� �Ÿ� �̻� ���ư��� ����
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

            // �ǰ� ���Ͱ� �־ ReturnPool ó��
            ReturnPool();
        }
        else if (!other.CompareTag("Player"))
        {
            ReturnPool();
        }
    }
}
