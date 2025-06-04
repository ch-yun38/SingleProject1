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



    // 아이템 장착(맨손 > 아이템)
    public virtual void Equip() { }

    //아이템 해제(이이템 > 맨손)
    public virtual void UnEquip() { }

  


    //탄창 유아이
    public virtual void ItemUi() { }

    // 모든 아이템 이용 / 무기류 : 공격, 힐팩 : 회복
    public virtual void Use() { }

    // 총기류는 구현o, 근거리는 구현x
    public virtual void Reload() { }

}
