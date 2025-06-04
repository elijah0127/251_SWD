using JetBrains.Annotations;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;
using UnityEngine.UIElements;

public class Playermove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    public float fallFaster;
    public float shortJumper;
    public int maxjumpCount = 1;
    public int jumpCount;
    public Transform groundCheck;
    Rigidbody2D rigid;
    SpriteRenderer SpriteRenderer;
    Animator animator;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //����
        if (Input.GetKeyDown(KeyCode.UpArrow) && jumpCount > 0)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpCount--;
            animator.SetBool("isJumping", true);
        }

        //������ȯ �ִϸ��̼�
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            SpriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        //�̵� �ִϸ��̼�
        if (Input.GetButton("Horizontal"))
            animator.SetBool("isRunning", true);
        else
            animator.SetBool("isRunning", false);

        //���� Ȯ��
        RaycastHit2D rayHit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.5f, LayerMask.GetMask("Platform"));

   
        if (rayHit.collider != null && rayHit.distance <= 0.3f)
        {
            if (rigid.velocity.y < 0)
                rigid.velocity = new Vector2(rigid.velocity.x, 0); //���� ����

            jumpCount = maxjumpCount;
            animator.SetBool("isJumping", false);
        }
    }

    void FixedUpdate()
    {
        //�¿��̵�, �ִ�
        float h = Input.GetAxisRaw("Horizontal");
        rigid.velocity = new Vector2(h * maxSpeed, rigid.velocity.y);

        //���� �ӵ� ���� & ª�� ����
        if (rigid.velocity.y < 0)
        {
            rigid.gravityScale = fallFaster;
        }
        else if (rigid.velocity.y > 0 && jumpCount != maxjumpCount){
            rigid.gravityScale = shortJumper;
        } 
    }
}
