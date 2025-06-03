using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;
    public float itemCoolTime;
    public float reLoading;
    public float bulletNum;



    // ������ ����(�Ǽ� > ������)
    public virtual void Equip() { }

    //������ ����(������ > �Ǽ�)
    public virtual void UnEquip() { }

  


    //źâ ������
    public virtual void ItemUi() { }

    // ��� ������ �̿� / ����� : ����, ���� : ȸ��
    public virtual void Use() { }

    // �ѱ���� ����o, �ٰŸ��� ����x
    public virtual void Reload() { }

}
