using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Info")]
    public Transform player;
    public Rigidbody playerRb;
    public Animator playerAnim;
    public CapsuleCollider standingCollider;
    public CapsuleCollider crouchingCollider;
    private float originalCapsuleHeight;

    [Header("Bools")]
    public bool crouched;
    public bool tired;

    [Header("Stats")]
    public float moveSpeed;
    public float normalSpeed;
    public float rotateSpeed;
    public float jumpForce;
    public float gravityModifier;
    public float sprintModifier;
    public float crouchHeight;
    public float crouchModifier;

    [Header("Camera")]
    public Transform cam;
    public CameraController camControl;

    public Vector3 direction;
    float cameraInputY;
    float cameraInputX;

    private void Start()
    {
        crouchingCollider.enabled = false;
        standingCollider.enabled = true;
        tired = false;
        playerAnim = GetComponent<Animator>();
        Physics.gravity *= gravityModifier;
        normalSpeed = moveSpeed;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); //AD
        float vertical = Input.GetAxisRaw("Vertical"); //WS

        cameraInputY = Input.GetAxis("Mouse Y");
        cameraInputX = Input.GetAxis("Mouse X");

        if (!tired)
        {
            Rotation(vertical, horizontal);
            Movement(vertical, horizontal);
        }
    }

    private void LateUpdate()
    {
        camControl.OrbitingCamera(cameraInputX, cameraInputY);
        camControl.FollowTarget();
        camControl.CameraCollisions();
    }

    void Movement(float vertical, float horizontal)
    {
        direction = (cam.forward * vertical) + (cam.right * horizontal);
        direction.Normalize();
        direction.y = 0;

        if (direction.magnitude >= 0.1f)
        {
            playerRb.velocity = direction * moveSpeed;

            if (crouched)
            {
                playerAnim.SetInteger("trigger", 4); // crouch walking
                if (Input.GetKeyUp(KeyCode.LeftControl))
                {
                    playerAnim.SetInteger("trigger", 0);
                }
            }
            else if (!crouched)
            {
                playerAnim.SetInteger("trigger", 1); // normal walking
            }

            if (Input.GetKey(KeyCode.LeftShift) && !crouched)
            {
                playerRb.velocity = direction * moveSpeed * sprintModifier; ;
                playerAnim.SetInteger("trigger", 2);

                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    playerAnim.SetInteger("trigger", 0);
                }
            }

        }
        else if (direction.magnitude == 0)
        {
            playerRb.velocity = Vector3.zero;

            if (crouched)
            {
                playerAnim.SetInteger("trigger", 3); // crouch idle
            }
            else
            {
                playerAnim.SetInteger("trigger", 0); // normal idle
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouched = true;
            Crouch();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            crouched = false;
            Stand();
        }
    }

    void Crouch()
    {
        standingCollider.enabled = false;
        crouchingCollider.enabled = true;
        moveSpeed = 2f;
    }

    void Stand()
    {
        standingCollider.enabled = true;
        crouchingCollider.enabled = false;
        moveSpeed = normalSpeed;
    }

    void Rotation(float vertical, float horizontal)
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = (cam.forward * vertical) + (cam.right * horizontal);
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }
}
