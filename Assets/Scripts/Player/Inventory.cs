using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item[] item = new Item[3];
    public RaycastHit ray;
    public enum invenSlot
    {
        main = 0,
        second = 1,
        heal = 2
    }



    public void GetItem()
    {
        


    }


    public void DropItem()
    { 
    }



    public void SwapItem()
    {
       
    }

    private void Update()
    {
        //ī�޶� 
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green);
        if (Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            if (hit.collider.gameObject.CompareTag("Item"))
            {
                // �����ۿ� Ŀ���� ������ �� �� ���� ui ����
                // InteractiveUi.show("fŰ�� ���� �����ϱ�");

                if(Input.GetKeyDown(KeyCode.F))
                {
                    GetItem();
                }


            }
        }
    }



}
