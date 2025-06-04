using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerStatus status;



    public Vector3 GetInputDirection()
    {
        // 입력 설정
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        return new Vector3(x, 0, z);
    }


    public Vector3 GetMoveDirection()
    {
       
        // 방향 설정
        Vector3 input = GetInputDirection();

        Vector3 dir = (transform.right * input.x) + (transform.forward * input.z);

        Debug.DrawRay(movement.transform.position + Vector3.up * 1.5f, dir * 2f, Color.red); // ← 디버깅용

        return dir.normalized;
    }

    /*public Vector3 GetCameraDirection()
    {
        Camera aimCamera;
        Camera idleCamera;

        // IdleMode에서 캐릭터만 회전할 수 있도록 방향값 받기
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
        // 마우스 가로 세로 얼마나 움직이는지 반환된다.
        // +위, -아레 움직임,  카메라 기준 마우스를 위로들면 
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
        Vector3 inputDir = GetMoveDirection(); // 카메라 기준 방향 포함
        movement.SetMove(inputDir);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.Jump();
        }

        //  마우스 누르면 에임모드로 들어가서 

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
