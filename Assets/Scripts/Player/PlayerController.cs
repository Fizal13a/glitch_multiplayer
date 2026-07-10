using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")] 
    private Rigidbody rb;
    private InteractionSystem interactionSystem;
    private Player_Inputs playerInput;

    [Header("Movement")] [SerializeField] private float speed;
    private Vector3 movementAxis;
    
    [Header("Rotation")] [SerializeField] private float rotationSpeed;

    [Header("Jump")] [SerializeField] private float jumpForce;

    [Header("Ground Check")] [SerializeField]
    private Transform groundCheckPoint;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        interactionSystem = GetComponent<InteractionSystem>();
    }

    private void OnEnable()
    {
        playerInput = new Player_Inputs();
        playerInput.Enable();
    }

    private void OnDisable()
    {
        if (playerInput != null) playerInput.Disable();
    }

    private void Start()
    {
        playerInput.Player.Move.performed += OnMove;
        playerInput.Player.Move.canceled += OnMove;
        playerInput.Player.Jump.started += OnJump;
        playerInput.Player.Interact.started += OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
        interactionSystem.Interact();
    }

    #region InputCallbacks

    private void OnJump(InputAction.CallbackContext obj)
    {
        if (!IsGrounded()) return;

        Jump();
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        Vector2 moveAxis = obj.ReadValue<Vector2>();
        movementAxis = new Vector3(moveAxis.x, 0, moveAxis.y);
    }

    #endregion

    private void FixedUpdate()
    {
        Move();
    }

    #region Actions

    private void Move()
    {
        Vector3 targetPosition = rb.position + movementAxis * speed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);
        
        RotateTowardsMovement();
    }

    private void RotateTowardsMovement()
    {
        if (movementAxis.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(movementAxis);

        rb.MoveRotation(Quaternion.Slerp(
            rb.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime));
    }
    
    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }

    #endregion

    #region Getters

    public bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(groundCheckPoint.position, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            return true;
        }

        return false;
    }

    #endregion
}