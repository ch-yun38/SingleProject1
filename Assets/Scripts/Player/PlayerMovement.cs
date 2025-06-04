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
        // Rigidbody.velocity���� Vector3 ���� �Է� ����
        // MoveToward(����ӵ�, ��ǥ�ӵ�(����*�ְ�ӵ�), �� ������ ���� �ִ� ���ӰŸ�)
        // x,z�� ���� �δ� ���� y�� ������ ����
        // MoveTowards�� ������ Vector3. Vector3�� �ϳ��� ���� ���� float ���̹Ƿ�  Mathf.�� float ������ ��ȯ��Ų��. (�Է°�, ��°� ��� flaot)

        // ���� �ӵ� ����
        Vector3 move = rigid.velocity;
        Vector3 vec = dir * speed;

        if (isAiming)
        {
            vec *= 0.5f;
        }


        // ���� �ÿ� �̵� ���� �̵��ӵ��� �ٸ����Ͽ� ���� �Ҷ� �̵��ӵ��� �������� ���� ����
        // curAccel�� ������ Ȱ��ȭ �Ǹ� ���� ���ӵ���, ������ ��Ȱ��ȭ �Ǹ� �̵� ���ӵ��� �̵��ϵ��� ���׿����� �̿�
        curAccel = isJumped ? jumpAccel : moveAccel;
        move.x = Mathf.MoveTowards(move.x, vec.x, curAccel * Time.deltaTime);
        move.z = Mathf.MoveTowards(move.z, vec.z, curAccel * Time.deltaTime);

        //�����ӵ� �ݿ�       
        rigid.velocity = move;
    }


    public void Jump()
    {
        // 2�� ���� ��������� �� ���� ����
        if (isJumped == true) { return; }


        // ����
        rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        isJumped = true;
        
        // �����ϰ� �̵����� �� �������� ���� ����
        jumpAccel = 0.1f * jumpAccel;

        // �����ϸ鼭 �������� �� �̵��ӵ��� �������� ���� ����
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
        // ��ǥ*���� = ���콺 ȸ�� ������ ����
        Vector2 mouseDir = mousePose * mouseSensitivity;

        // ���� ���� Ȯ��
        // ���δ� 360ȸ�� ����/ �������� �ִ� �ּ��� �� ������ ������ �־�� ��

        currentRotation.x += mouseDir.x;
        currentRotation.y = Mathf.Clamp(currentRotation.y + mouseDir.y, minPitch, maxPitch);

        // ĳ���� �¿츸 �����̰� ����
        transform.rotation = Quaternion.Euler(0, currentRotation.x, 0);
      
        // ���� ����� ���� ȸ������ ����
        // �Ȱ��� ���콺 y���� 3�� ȸ���� x���� ����Ű�Ƿ� ���콺 ���� �޴� currentRotation.y�� x��,
        // �� ���� ���� ī�޶� ��ü��  ��ǥ���� �ְ� aim��ǥ�� ���� x���� ȸ���� ���� ���ϸ� �����ϰ�
        // �������� ������ ����
        Vector3 currentEuler = aim.localEulerAngles;
        aim.localEulerAngles = new Vector3(currentRotation.y, currentEuler.y, currentEuler.z);

        //rotDir�� ȸ���� �÷��̾��� ���� ������ ��Ÿ���� ����, ¡�� ���� �ʿ��Ҷ� ����
        Vector3 rotateDirVector = transform.forward;
        rotateDirVector.y = 0;
        return rotateDirVector.normalized;

    }


    // ���ӻ��°� �ƴ� ��� ĳ���͸� ȸ���ϰ�
    public void AvatarRotation(Vector3 direction)
    {
        
        // ���Ⱚ 0�̸� ������
        if (direction == Vector3.zero) return;

        // direction �������� ȸ���ñ� dk��Ÿ�� ȸ��������
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // �ε巯�� �������� ���� ����(����, ��ǥ, �ӵ�)
        avatar.rotation = Quaternion.Lerp(avatar.rotation, targetRotation, rotSpeed*Time.deltaTime);

    }

    // ���Ӹ��
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
