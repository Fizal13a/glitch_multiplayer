using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")] private Rigidbody rb;
    private InteractionSystem interactionSystem;
    private PickupSystem pickupSystem;
    private Player_Inputs playerInput;
    [SerializeField] private PlayerUIManager playerUIManager;

    [Header("Movement")] [SerializeField] private float speed;
    private Vector3 movementAxis;

    [Header("Rotation")] [SerializeField] private float rotationSpeed;

    [Header("Jump")] [SerializeField] private float jumpForce;

    [Header("Ground Check")] [SerializeField]
    private Transform groundCheckPoint;

    [Header("Interaction")] private float interactStartTime;
    [SerializeField] private float maxThrowHeldTime;
    private float interactPressedTime;
    private bool isInteractHeld;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        interactionSystem = GetComponent<InteractionSystem>();
        pickupSystem = GetComponent<PickupSystem>();
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
        playerInput.Player.Interact.started += OnInteractStarted;
        playerInput.Player.Interact.canceled += OnInteractCanceled;
    }

    #region InputCallbacks

    private void OnInteractStarted(InputAction.CallbackContext ctx)
    {
        interactPressedTime = Time.time;
        isInteractHeld = true;
    }

    private void OnInteractCanceled(InputAction.CallbackContext ctx)
    {
        if (!isInteractHeld) return;

        isInteractHeld = false;

        float heldTime = Time.time - interactPressedTime;
        playerUIManager.ResetInteractCharge();

        // Holding item = drop
        if (pickupSystem.IsHoldingItem)
        {
            pickupSystem.DropHeldItem();
            return;
        }

        IInteractable interactable =
            interactionSystem.CurrentInteractable;

        if (interactable == null)
            return;

        // Instant interaction
        if (interactable.HoldDuration <= 0f)
        {
            interactable.Interact();
        }
    }

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

    private void Update()
    {
        CheckInteraction();
    }

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
        if (movementAxis.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(movementAxis);

        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }

    private void CheckInteraction()
    {
        if (!isInteractHeld)
            return;

        float heldTime = Time.time - interactPressedTime;

        // THROW
        if (pickupSystem.IsHoldingItem)
        {
            float charge = Mathf.Clamp01(
                heldTime / maxThrowHeldTime
            );

            playerUIManager.SetInteractCharge(charge);

            if (heldTime >= maxThrowHeldTime)
            {
                isInteractHeld = false;

                playerUIManager.ResetInteractCharge();
                pickupSystem.ThrowHeldItem();
            }

            return;
        }

        // INTERACTABLE HOLD
        IInteractable interactable =
            interactionSystem.CurrentInteractable;

        if (interactable == null)
            return;

        if (interactable.HoldDuration <= 0f)
            return;

        float interactCharge = Mathf.Clamp01(
            heldTime / interactable.HoldDuration
        );

        playerUIManager.SetInteractCharge(interactCharge);

        if (heldTime >= interactable.HoldDuration)
        {
            isInteractHeld = false;

            playerUIManager.ResetInteractCharge();
            interactable.Interact();
        }
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