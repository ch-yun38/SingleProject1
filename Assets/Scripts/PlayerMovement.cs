using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{

    [Header("Reference")]
    [SerializeField] private Transform avatar;
    [SerializeField] private Transform aim;

    [Header("Mouse Config")]
    [SerializeReference][Range(-90, 0)] private float minPitch;
    [SerializeField][Range(0, 90)] private float maxPitch;
    [SerializeField][Range(0, 5)] private float mouseSensitivity;




    private Rigidbody rigid;
    public float speed { get; private set; } = 10f;
    public float moveAccel { get; private set; } = 30f;
    public float jumpAccel { get; private set; } = 30f;
    private float jumpPower = 5;
    private float curAccel;
    private bool isJumped = false;
    private bool isAiming = false;

    
   

    public void SetMove()
    {
        // 입력 설정
        float xInput = Input.GetAxisRaw("Horizontal");
        float zInput = Input.GetAxisRaw("Vertical");

        // 방향 설정
        Vector3 dir = new Vector3(xInput, 0, zInput);


        // 대각선 방향 속도 조절
        if (dir.sqrMagnitude > 1)
        {
            // Vector3.sqrMagnitude : 읽기 전용, 벡터 길이의 제곱 값을 가져온다. (대각선 길이는 루트 있음)
            // 방향 설정 (정규화), 방향이 대각선일 때 속도 과다 방지를 위함
            dir = dir.normalized;
        }

        // Rigidbody.velocity에는 Vector3 값만 입력 가능
        // MoveToward(현재속도, 목표속도(방향*최고속도), 한 프레임 동안 최대 가속거리)
        // x,z를 따로 두는 것은 y값 보존을 위함
        // MoveTowards의 형식은 Vector3. Vector3의 하나의 원소 값은 float 값이므로  Mathf.로 float 값으로 반환시킨다. (입력값, 출력값 모두 flaot)

        // 현재 속도 저장
        Vector3 move = rigid.velocity;
        Vector3 vec = dir * speed;

        // 점프 시와 이동 시의 이동속도를 다르게하여 점프 할때 이동속도가 빨라지는 것을 방지
        // curAccel은 점프가 활성화 되면 점프 가속도로, 점프가 비활성화 되면 이동 가속도로 이동하도록 삼항연산자 이용
        curAccel = isJumped ? jumpAccel : moveAccel;
        move.x = Mathf.MoveTowards(move.x, vec.x, curAccel * Time.deltaTime);
        move.z = Mathf.MoveTowards(move.z, vec.z, curAccel * Time.deltaTime);

        //최종속도 반영       
        rigid.velocity = move;

    }

    public void SetJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumped)
        {
            // 점프
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isJumped = true;
            if (isJumped == true)
            {
                // 점프하고 이동했을 때 빨라지는 것을 방지
                jumpAccel = 0.1f * jumpAccel;

                // 가속하면서 점프했을 때 이동속도가 빨라지는 것을 방지
                Vector3 jumpVel = rigid.velocity;
                jumpVel.x *= 0.65f;
                jumpVel.z *= 0.65f;
                rigid.velocity = jumpVel;
            }

        }


    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Field"))
        {
            isJumped = false;
            jumpAccel = moveAccel;
        }
    }



    public void aimRotation() 
    {
        // 마우스 가로 세로 얼마나 움직이는지 반환된다.
        // +위, -아레 움직임,  카메라 기준 마우스를 위로들면 
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y")* mouseSensitivity;  
        //Vector3 mouseDir = 
    }



    public void bodyRotation()
    {

    }



    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        SetMove();
        SetJump();
    }
}
