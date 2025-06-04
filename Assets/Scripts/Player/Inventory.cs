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
        //카메라 
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green);
        if (Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            if (hit.collider.gameObject.CompareTag("Item"))
            {
                // 아이템에 커서를 가져다 댈 때 습득 ui 띄우기
                // InteractiveUi.show("f키를 눌러 습득하기");

                if(Input.GetKeyDown(KeyCode.F))
                {
                    GetItem();
                }


            }
        }
    }



}
