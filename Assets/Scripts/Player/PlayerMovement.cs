using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{

    [Header("Reference")]
    [SerializeField] private Transform avatar;
    [SerializeField] private Transform aim;

    [Header("Mouse Config")]
    [SerializeField][Range(-90, 0)] private float minPitch;
    [SerializeField][Range(0, 90)] private float maxPitch;
    [SerializeField][Range(0, 5)] private float mouseSensitivity;


    private Rigidbody rigid;
    private PlayerStatus playerStatus;
    public float speed { get; private set; } = 5f;
    public float moveAccel { get; private set; } = 15f;
    public float jumpAccel { get; private set; } = 15f;
    private float jumpPower = 5;
    private float curAccel;
    private bool isJumped = false;
    private bool isAiming = false;
    private float rotSpeed = 1f;
    public Camera aimCamera;
    public Camera idleCamera;
    public Vector3 rotDir { get; private set; }
    private Vector2 currentRotation;
    private Animator animator;

    public void SetMove(Vector3 dir)
    {
        // Rigidbody.velocity에는 Vector3 값만 입력 가능
        // MoveToward(현재속도, 목표속도(방향*최고속도), 한 프레임 동안 최대 가속거리)
        // x,z를 따로 두는 것은 y값 보존을 위함
        // MoveTowards의 형식은 Vector3. Vector3의 하나의 원소 값은 float 값이므로  Mathf.로 float 값으로 반환시킨다. (입력값, 출력값 모두 flaot)

        // 현재 속도 저장
        Vector3 move = rigid.velocity;
        Vector3 vec = dir * speed;

        if (isAiming)
        {
            vec *= 0.5f;
        }


        // 점프 시와 이동 시의 이동속도를 다르게하여 점프 할때 이동속도가 빨라지는 것을 방지
        // curAccel은 점프가 활성화 되면 점프 가속도로, 점프가 비활성화 되면 이동 가속도로 이동하도록 삼항연산자 이용
        curAccel = isJumped ? jumpAccel : moveAccel;
        move.x = Mathf.MoveTowards(move.x, vec.x, curAccel * Time.deltaTime);
        move.z = Mathf.MoveTowards(move.z, vec.z, curAccel * Time.deltaTime);

        //최종속도 반영       
        rigid.velocity = move;
    }


    public void Jump()
    {
        // 2단 점프 막기용으로 맨 위에 정의
        if (isJumped == true) { return; }


        // 점프
        rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        isJumped = true;
        
        // 점프하고 이동했을 때 빨라지는 것을 방지
        jumpAccel = 0.1f * jumpAccel;

        // 가속하면서 점프했을 때 이동속도가 빨라지는 것을 방지
        Vector3 jumpVel = rigid.velocity;
        jumpVel.x *= 0.65f;
        jumpVel.z *= 0.65f;
        rigid.velocity = jumpVel;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Field"))
        {
            isJumped = false;
            jumpAccel = moveAccel;
        }
    }



    public Vector3 AimRotation(Vector2 mousePose) 
    {
        // 좌표*감도 = 마우스 회전 정도를 구함
        Vector2 mouseDir = mousePose * mouseSensitivity;

        // 현재 각도 확인
        // 가로는 360회전 가능/ 세로축은 최대 최소의 고개 젖히기 제한이 있어야 함

        currentRotation.x += mouseDir.x;
        currentRotation.y = Mathf.Clamp(currentRotation.y + mouseDir.y, minPitch, maxPitch);

        // 캐릭터 좌우만 움직이게 조절
        transform.rotation = Quaternion.Euler(0, currentRotation.x, 0);
      
        // 에임 모드의 로컬 회전값을 저장
        // 똑같이 마우스 y값이 3차 회전의 x값을 가리키므로 마우스 값을 받는 currentRotation.y는 x에,
        // 그 다음 에임 카메라 자체의  좌표값을 넣고 aim좌표를 수정 x값은 회전에 따라 상하를 조절하고
        // 나머지는 기존값 보존
        Vector3 currentEuler = aim.localEulerAngles;
        aim.localEulerAngles = new Vector3(currentRotation.y, currentEuler.y, currentEuler.z);

        //rotDir은 회전시 플레이어의 진행 방향을 나타내는 벡터, 징행 방향 필요할때 쓰기
        Vector3 rotateDirVector = transform.forward;
        rotateDirVector.y = 0;
        return rotateDirVector.normalized;

    }


    // 에임상태가 아닌 경우 캐릭터만 회전하게
    public void AvatarRotation(Vector3 direction)
    {
        
        // 방향값 0이면 가만히
        if (direction == Vector3.zero) return;

        // direction 방형으로 회전시기 dk바타의 회전방향을
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // 부드러운 움직임을 위한 보간(현재, 목표, 속도)
        avatar.rotation = Quaternion.Lerp(avatar.rotation, targetRotation, rotSpeed*Time.deltaTime);

    }

    // 에임모드
    public void OnAimMode(bool aiming)
    {
        isAiming = aiming;
        aimCamera.gameObject.SetActive(aiming);
        idleCamera.gameObject.SetActive(!aiming);

        animator.SetBool("isAiming", aiming);
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        playerStatus = GetComponent<PlayerStatus>();

    }

    void Start()
    {
        Vector2 currentRotation = new Vector2()
        {
            x = transform.rotation.eulerAngles.x,
            y = transform.rotation.eulerAngles.y
        };

        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!isAiming)
        {
          
            Vector3 aniVel = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);
            float aniSpeed = aniVel.magnitude;

            animator.SetFloat("speed", aniSpeed);
            animator.SetBool("isAiming", isAiming);
        }
    }
}
