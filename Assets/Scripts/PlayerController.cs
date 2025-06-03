using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerStatus status;



    public Vector3 GetInputDirection()
    {
        // �Է� ����
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        return new Vector3(x, 0, z);
    }


    public Vector3 GetMoveDirection()
    {
       
        // ���� ����
        Vector3 input = GetInputDirection();

        Vector3 dir = (transform.right * input.x) + (transform.forward * input.z);

        Debug.DrawRay(movement.transform.position + Vector3.up * 1.5f, dir * 2f, Color.red); // �� ������

        return dir.normalized;
    }

    /*public Vector3 GetCameraDirection()
    {
        Camera aimCamera;
        Camera idleCamera;

        // IdleMode���� ĳ���͸� ȸ���� �� �ֵ��� ���Ⱚ �ޱ�
        Vector3 camForward = idleCamera.transform.forward;
        Vector3 camRight = idleCamera.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 rotDir = (camForward * dir.z + camRight * dir.x).normalized;
    }
   */

    public Vector2 GetMousePos()
    {
        // ���콺 ���� ���� �󸶳� �����̴��� ��ȯ�ȴ�.
        // +��, -�Ʒ� ������,  ī�޶� ���� ���콺�� ���ε�� 
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        return new Vector2(mouseX,mouseY);
    }



    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        status = GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        Vector3 inputDir = GetMoveDirection(); // ī�޶� ���� ���� ����
        movement.SetMove(inputDir);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.Jump();
        }

        //  ���콺 ������ ���Ӹ��� ���� 

        Vector2 mousePose = GetMousePos();
        movement.AimRotation(mousePose);

        bool isAiming = Input.GetKey(KeyCode.Mouse1);
        movement.OnAimMode(isAiming);
        if (!isAiming)
        {
            movement.AvatarRotation(movement.rotDir);
        }

    }
}
