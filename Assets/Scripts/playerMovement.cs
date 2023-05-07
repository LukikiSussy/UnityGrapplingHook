using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [Header("Grappling Gun Script Ref:")]
    [SerializeField] private GrapplingGun grappleSc;

    [Header("Physics Ref:")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Header("Movement Settings:")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpPower = 16f;

    [Header("Jump Settings:")]
    [SerializeField] private float canJumpAfterSeconds = 0.1f;

    float groundedTimer;
    float spacePressedTimer;

    bool spaceIsBeingPressed = true;

    private float horizontal;

    void Start() {
        groundedTimer = canJumpAfterSeconds;
        spacePressedTimer = canJumpAfterSeconds;
    }

    void Update() {

        horizontal = Input.GetAxisRaw("Horizontal");
        UpdateTimers();

        if (CanJump()) {

            groundedTimer = 0;
            spacePressedTimer = 0;
            spaceIsBeingPressed = false;

            rb.velocity = new Vector2(rb.velocity.x, jumpPower);

        }

    }

    private void FixedUpdate() {

        if (!grappleSc.grapplingRopeSc.lineRenderer.enabled && CanJump()) {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        else if (grappleSc.grapplingRopeSc.lineRenderer.enabled) {
            if (Mathf.Abs(rb.velocity.x) < speed/2) {
                rb.velocity = new Vector2(rb.velocity.x + horizontal * speed/60, rb.velocity.y);
            }
        }
        else if (rb.velocity.x < speed + 1 && horizontal > 0 || rb.velocity.x > -speed - 1 && horizontal < 0) {
            rb.velocity = new Vector2(rb.velocity.x + horizontal * speed/15, rb.velocity.y);
        }

    }

    private bool CanJump() {
        if (groundedTimer > 0 && spacePressedTimer > 0 && spaceIsBeingPressed) {
            return true;
        }
        return false;
    }

    private void UpdateTimers() {

        groundedTimer -= Time.deltaTime;
        spacePressedTimer -= Time.deltaTime;

        if (Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer)) {
            groundedTimer = canJumpAfterSeconds;
        }
        if (Input.GetKey("space")) {
            spacePressedTimer = canJumpAfterSeconds;
        }
        if (Input.GetKeyDown("space")) {
            spaceIsBeingPressed = true;
        }
    }
}
