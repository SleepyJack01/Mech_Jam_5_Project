using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CharacterController controller;
    [SerializeField] Transform modelsTransform;

    [Header("Input Varibles")]
    private Vector2 currentMovementInput;
    private Vector3 moveDirection;
    private float verticalVelocity;
    private Vector3 verticalMove;
    private Vector3 currentModelRotation;

    [Header("Movement Variables")]
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float movementLerpTime = 6f;
    [SerializeField] float rotationSpeed = 15f;

    [Header("Twin Stick Movement Settings")]
    [SerializeField] bool twinStickMovement = false;
    [SerializeField] Vector2 twinStickInput;

    [Header("Gravity Settings")]
    [SerializeField] float gravity = -9.81f;

    [Header("Ground Check Settings")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 1.1f;
    private bool isGrounded
    {
        get
        {
            Ray ray = new Ray(transform.position, -transform.up);
            return Physics.SphereCast(ray, controller.radius, groundDistance, groundMask);
        }
    }
    private void Start() 
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update() 
    {
        ApplyGravity();
        MovementHandler();
        
        if (twinStickMovement)
        {
            TwinStickRotation();
        }
        else
        {
            ModelRotation();
        }
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        verticalMove = new Vector3(0, verticalVelocity, 0);
        controller.Move(verticalMove * Time.deltaTime);
    }

    void MovementHandler()
    {
        if (isGrounded)
        {
            moveDirection = Vector3.Lerp(moveDirection,(transform.forward * currentMovementInput.y + transform.right * currentMovementInput.x), Time.deltaTime * movementLerpTime);
        }

        if (moveDirection.magnitude >= 0.1f)
        {
            controller.Move(moveDirection * movementSpeed * Time.deltaTime);
        }
        else
        {
            controller.Move(Vector3.MoveTowards(moveDirection, Vector3.zero, Time.deltaTime * movementLerpTime) * movementSpeed * Time.deltaTime);
        }
    }

    private void TwinStickRotation()
    {
        Vector3 direction = new Vector3(twinStickInput.x, 0, twinStickInput.y);
        if (direction != Vector3.zero)
        {
            modelsTransform.rotation = Quaternion.Slerp(modelsTransform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
        }
    }

    private void ModelRotation()
    {
        currentModelRotation = new Vector3(moveDirection.x, 0, moveDirection.z);
        if (currentModelRotation != Vector3.zero)
        {
            modelsTransform.rotation = Quaternion.Slerp(modelsTransform.rotation, Quaternion.LookRotation(currentModelRotation), Time.deltaTime * rotationSpeed);
        }
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        twinStickInput = context.ReadValue<Vector2>();
    }
    
}
