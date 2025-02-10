using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor.Experimental.GraphView;
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
    private Vector3 originalGravity;

    [Header("Camera")]
    public Transform cam;
    public CameraController camControl;

    public Vector3 direction;
    float cameraInputY;
    float cameraInputX;

    private void Start()
    {
        // Dynamically find the player object if not already assigned
        if (player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player"); // Ensure your player has the "Player" tag
            if (playerObject != null)
            {
                player = playerObject.transform;
                playerRb = playerObject.GetComponent<Rigidbody>();
                playerAnim = playerObject.GetComponent<Animator>();

                // Assign colliders
                CapsuleCollider[] colliders = playerObject.GetComponents<CapsuleCollider>();
                foreach (var col in colliders)
                {
                    if (col.height > crouchHeight) // Example: determine standing collider by height
                        standingCollider = col;
                    else
                        crouchingCollider = col;
                }
            }
            else
            {
                Debug.LogError("Player object not found! Make sure the Player has the 'Player' tag.");
            }
        }

        // Initialize other fields
        crouchingCollider.enabled = false;
        standingCollider.enabled = true;
        tired = false;

        if (playerAnim == null)
        {
            Debug.LogWarning("Animator component not found on Player.");
        }
        
        normalSpeed = moveSpeed;

        // Hide the cursor
        Cursor.visible = false;

        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); //AD
        float vertical = Input.GetAxisRaw("Vertical"); //WS

        cameraInputY = Input.GetAxis("Mouse Y");
        cameraInputX = Input.GetAxis("Mouse X");

        // Press "Escape" to unlock and show the cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // Press "C" to hide and lock the cursor again
        if (Input.GetKeyDown(KeyCode.C))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

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

            var audioManager = FindObjectOfType<audioManager>();
            var footstepsSound = audioManager.sounds.FirstOrDefault(s => s.name == "footsteps");
            var runSound = audioManager.sounds.FirstOrDefault(s => s.name == "running");

            if (Input.GetKey(KeyCode.LeftShift) && !crouched)
            {
                // Sprinting
                playerRb.velocity = direction * moveSpeed * sprintModifier;
                playerAnim.SetInteger("trigger", 2);

                if (runSound != null && !runSound.source.isPlaying)
                {
                    // Stop footsteps sound if running starts
                    if (footstepsSound != null && footstepsSound.source.isPlaying)
                    {
                        audioManager.Stop("footsteps");
                    }
                    audioManager.Play("running");
                }
            }
            else
            {
                // Walking
                playerAnim.SetInteger("trigger", crouched ? 4 : 1);

                if (footstepsSound != null && !footstepsSound.source.isPlaying)
                {
                    // Stop running sound if walking starts
                    if (runSound != null && runSound.source.isPlaying)
                    {
                        audioManager.Stop("running");
                    }
                    audioManager.Play("footsteps");
                }
            }
        }
        else
        {
            // Player is idle
            playerRb.velocity = Vector3.zero;

            // Stop all movement sounds
            FindObjectOfType<audioManager>().Stop("footsteps");
            FindObjectOfType<audioManager>().Stop("running");

            playerAnim.SetInteger("trigger", crouched ? 3 : 0);
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
        moveSpeed *= crouchModifier;
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
