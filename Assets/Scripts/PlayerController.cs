using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f; // �̵� �ӵ�
    public float jumpForce = 7.0f; // ���� ��
    public float dashSpeed = 10.0f; // ��� �ӵ�
    public float dashDuration = 0.5f; // ��� ���� �ð�


    private Rigidbody rb;
    private bool isGrounded = true; // ���� ��Ҵ��� ����
    private int jumpCount = 0; // ���� Ƚ��
    private bool isDashing = false; // ��� ������ ����
    private float dashTimer = 0.0f; // ��� Ÿ�̸�

    private Transform mainCameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        mainCameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // �̵� ó��
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 cameraForward = mainCameraTransform.forward;
        Vector3 cameraRight = mainCameraTransform.right;
        cameraForward.y = 0f; // y �� ȸ�� ������ �����մϴ�.
        cameraRight.y = 0f;

        Vector3 movement = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;

        // ��� ���� ��� ��� �ӵ��� �̵�
        if (isDashing)
        {
            movement = transform.forward * dashSpeed;
            dashTimer += Time.deltaTime;

            // ��� ���� �ð��� ������ ��� ����
            if (dashTimer >= dashDuration)
            {
                isDashing = false;
                dashTimer = 0.0f;
            }
        }
        else
        {

            // �Ϲ� �̵�: �̵� ������ �̿��Ͽ� �̵�
            movement *= speed;
        }

        // ȸ�� ó��: �̵� �������� ĳ���͸� ȸ��
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f); // ȸ���� �ε巴�� ����

        }

        // Rigidbody�� �̵� ���� (y ���� ������ �ӵ��� �״�� ����)
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // ��� ó��
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && isGrounded)
        {
            isDashing = true;
        }

        // ���� ó��
        if (isGrounded)
        {
            jumpCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 1 && !isDashing)
        {
            // ���� ���� �����Ͽ� ����
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
        }
    }
 
    void OnCollisionEnter(Collision collision)
    {
        // ���� ������ ���� ������ ���·� ����
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // ������ �������� ���� �Ұ����� ���·� ����
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}