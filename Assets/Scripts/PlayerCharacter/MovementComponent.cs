using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementComponent : MonoBehaviour
{
    #region Serialized Member Variables
    [Header("Movement Variables")]
    [SerializeField]
    private float walkSpeed = 5;
    [SerializeField]
    private float runSpeed = 10;
    [SerializeField]
    private float jumpForce = 5;
    [SerializeField]
    private float aimSensitivity = 5;
    [SerializeField]
    private int maxJumps = 1;
    private int jumpCount;
    private bool isGrounded;

    [Header("Ground Checking")]
    [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    float groundedAngle = 45;
    [SerializeField]
    float raycastDist = 0.1f;
    [SerializeField]
    float coyoteTime = 0.1f;
    float coyoteTimer = 0;
    [SerializeField]
    float jumpTime = 0.1f;
    float jumpTimer = 0;

    [Header("Camera")]
    [SerializeField]
    private float cameraMin = 20;
    [SerializeField]
    private float cameraMax = 70;
    #endregion

    #region Movement Variables
    private Vector2 inputVector = Vector2.zero;
    private Vector3 moveDirection = Vector3.zero;
    private Vector2 lookInput = Vector3.zero;
    #endregion

    #region Component Reference Variables
    private PlayerController playerController;
    private Animator animator;
    private Rigidbody rigidbody;
    private Collider collider;
    public GameObject followTarget;
    #endregion

    public readonly int movementXHash = Animator.StringToHash("MovementX");
    public readonly int movementYHash = Animator.StringToHash("MovementY");
    public readonly int isRunningHash = Animator.StringToHash("isRunning");
    public readonly int isJumpingHash = Animator.StringToHash("isJumping");
    public readonly int verticalAimHash = Animator.StringToHash("verticalAim");


    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    private void Start()
    {
        if (!GameManager.instance.cursorActive)
            AppEvents.SetMouseCursorVisible(false);
    }

    private void FixedUpdate()
    {
        if (jumpTimer < jumpTime)
            jumpTimer += Time.fixedDeltaTime;

        isGrounded = false;

        RaycastHit[] hitInfo = Physics.SphereCastAll(new Ray(collider.bounds.center + Vector3.down * collider.bounds.extents.y, Vector3.down), collider.bounds.extents.x * 0.5f, raycastDist, layerMask);
        foreach (RaycastHit hit in hitInfo)
        {
            if (!hit.transform || hit.transform.gameObject == gameObject) continue;

            if (Vector3.Angle(hit.normal, Vector3.up) < groundedAngle)
            {
                isGrounded = true;
                break;
            }
            else
            {
                continue;
            }
        }
        if (!isGrounded)
        {
            coyoteTimer += Time.fixedDeltaTime;
            if (coyoteTimer < coyoteTime)
                isGrounded = true;
        }
        else if (jumpTimer > jumpTime)
        {
            coyoteTimer = 0;
            jumpCount = 0;
            if (playerController.isInAir)
            {
                animator.SetBool(isJumpingHash, false);
            }
        }
        
        playerController.isInAir = !isGrounded;
    }

    private void Update()
    {
        //#region Camera rotation
        //followTarget.transform.rotation *= Quaternion.AngleAxis(lookInput.x * aimSensitivity, Vector3.up);
        //followTarget.transform.rotation *= Quaternion.AngleAxis(lookInput.y * aimSensitivity, Vector3.left);
        //
        //transform.rotation = Quaternion.Euler(0, followTarget.transform.rotation.eulerAngles.y, 0);
        //
        //Vector3 angles = followTarget.transform.localEulerAngles;
        //if (angles.x > cameraMax && angles.x < 180)
        //    angles.x = cameraMax;
        //if (angles.x > 270 && angles.x < 360 + cameraMin)
        //    angles.x = 360 + cameraMin;
        //followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
        followTarget.transform.localEulerAngles = new Vector3(0, 0, 0);

        // Firing angles
        float min = -70;
        float max = 70;
        float range = max - min;
        float offsetToZero = 0 - min;
        float aimAngle = followTarget.transform.localEulerAngles.x;
        aimAngle = (aimAngle > 180) ? aimAngle - 360 : aimAngle;
        float verticalAim = (aimAngle + offsetToZero) / (range);
        animator.SetFloat(verticalAimHash, verticalAim);
        //#endregion


        if (playerController.isInAir) return;
        if (!(inputVector.magnitude > 0)) moveDirection = Vector3.zero;
        
        moveDirection = transform.forward * inputVector.y + transform.right * inputVector.x;
        float currentSpeed = playerController.isRunning ? runSpeed : walkSpeed;
        
        Vector3 movementDirection = moveDirection * currentSpeed * Time.deltaTime;
        transform.position += movementDirection;
    }

    public void OnMovement(InputValue value)
    {
        inputVector = value.Get<Vector2>();
        animator.SetFloat(movementXHash, inputVector.x);
        animator.SetFloat(movementYHash, inputVector.y);
    }

    public void OnRun(InputValue value)
    {
        playerController.isRunning = value.isPressed;
        animator.SetBool(isRunningHash, playerController.isRunning);
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (!isGrounded) return;
            if (playerController.isInAir) return;

            if (jumpCount < maxJumps && jumpTimer >= jumpTime)
            {
                ++jumpCount;
                jumpTimer = 0;
                animator.SetBool(isJumpingHash, true);
                rigidbody.AddForce((transform.up + moveDirection) * jumpForce, ForceMode.Impulse);
            }
        }
    }

    public void OnAim(InputValue value)
    {
        playerController.isAiming = value.isPressed;
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
        // If we aim up, down, adjust animations to have a mask that will let us properly animate aim.
    }
}
