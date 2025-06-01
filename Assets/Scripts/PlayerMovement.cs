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
        // �Է� ����
        float xInput = Input.GetAxisRaw("Horizontal");
        float zInput = Input.GetAxisRaw("Vertical");

        // ���� ����
        Vector3 dir = new Vector3(xInput, 0, zInput);


        // �밢�� ���� �ӵ� ����
        if (dir.sqrMagnitude > 1)
        {
            // Vector3.sqrMagnitude : �б� ����, ���� ������ ���� ���� �����´�. (�밢�� ���̴� ��Ʈ ����)
            // ���� ���� (����ȭ), ������ �밢���� �� �ӵ� ���� ������ ����
            dir = dir.normalized;
        }

        // Rigidbody.velocity���� Vector3 ���� �Է� ����
        // MoveToward(����ӵ�, ��ǥ�ӵ�(����*�ְ�ӵ�), �� ������ ���� �ִ� ���ӰŸ�)
        // x,z�� ���� �δ� ���� y�� ������ ����
        // MoveTowards�� ������ Vector3. Vector3�� �ϳ��� ���� ���� float ���̹Ƿ�  Mathf.�� float ������ ��ȯ��Ų��. (�Է°�, ��°� ��� flaot)

        // ���� �ӵ� ����
        Vector3 move = rigid.velocity;
        Vector3 vec = dir * speed;

        // ���� �ÿ� �̵� ���� �̵��ӵ��� �ٸ����Ͽ� ���� �Ҷ� �̵��ӵ��� �������� ���� ����
        // curAccel�� ������ Ȱ��ȭ �Ǹ� ���� ���ӵ���, ������ ��Ȱ��ȭ �Ǹ� �̵� ���ӵ��� �̵��ϵ��� ���׿����� �̿�
        curAccel = isJumped ? jumpAccel : moveAccel;
        move.x = Mathf.MoveTowards(move.x, vec.x, curAccel * Time.deltaTime);
        move.z = Mathf.MoveTowards(move.z, vec.z, curAccel * Time.deltaTime);

        //�����ӵ� �ݿ�       
        rigid.velocity = move;

    }

    public void SetJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumped)
        {
            // ����
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isJumped = true;
            if (isJumped == true)
            {
                // �����ϰ� �̵����� �� �������� ���� ����
                jumpAccel = 0.1f * jumpAccel;

                // �����ϸ鼭 �������� �� �̵��ӵ��� �������� ���� ����
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
        // ���콺 ���� ���� �󸶳� �����̴��� ��ȯ�ȴ�.
        // +��, -�Ʒ� ������,  ī�޶� ���� ���콺�� ���ε�� 
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
