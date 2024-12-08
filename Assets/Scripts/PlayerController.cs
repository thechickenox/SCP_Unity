using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float runSpeed = 7f; // Velocidad al correr
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;
    public float initialScaleX = 0.66412f;
    private Vector2 movementInput;
    private bool isRunning = false;
    private Rigidbody2D rb;
    private Animator animator;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Entradas del control de Xbox (también funciona para teclado)
        float moveX = Input.GetAxis("Horizontal"); // Left Stick X
        float moveY = Input.GetAxis("Vertical");   // Left Stick Y
        movementInput = new Vector2(moveX, moveY).normalized;

        // Detectar si el jugador está corriendo (Shift o RT en control)
        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Fire3"); // Fire3 se asigna al botón "B"

        // Establecer velocidad y animación
        float currentSpeed = isRunning ? runSpeed : moveSpeed;
        animator.SetFloat("Speed", currentSpeed);

        // Ajustar escala según la dirección del movimiento horizontal
        if (moveX > 0)
        {
            transform.localScale = new Vector3(-initialScaleX, transform.localScale.y, transform.localScale.z);
        }
        else if (moveX < 0)
        {
            transform.localScale = new Vector3(initialScaleX, transform.localScale.y, transform.localScale.z);
        }

        // Actualizar animaciones
        if (movementInput != Vector2.zero)
        {
            animator.SetFloat("Horizontal", moveX);
            animator.SetFloat("Vertical", moveY);
            animator.SetFloat("Movement", 1);
        }
        else
        {
            animator.SetFloat("Movement", 0);
        }
    }

    private void FixedUpdate()
    {
        if (movementInput != Vector2.zero)
        {
            TryMove(movementInput * (isRunning ? runSpeed : moveSpeed) * Time.fixedDeltaTime);
        }
    }

    private bool TryMove(Vector2 direction)
    {
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            direction.magnitude + collisionOffset
        );

        if (count == 0)
        {
            rb.MovePosition(rb.position + direction);
            return true;
        }
        else
        {
            return false;
        }
    }
}