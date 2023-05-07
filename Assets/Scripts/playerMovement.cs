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



    private float horizontal;

    private bool canJumpNow;
    private bool couldHaveJumped;

    private bool spaceHeldNow;
    private bool spaceHeldBefore;
    private bool spaceHeld = true;

    private bool JumpCoodown = true;

    void Update() {

        Jump();

        horizontal = Input.GetAxisRaw("Horizontal");

        if (SpaceHeldDown() && CanJump() && spaceHeld && JumpCoodown) {

            spaceHeld = false;
            JumpCoodown = false;
            StartCoroutine(JumpCooldownTimer());

            rb.velocity = new Vector2(rb.velocity.x, jumpPower);

        }

    }

    IEnumerator hasJumped() {

        bool grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        bool spacePressed = Input.GetKey("space");
        yield return new WaitForSeconds(canJumpAfterSeconds);

        if (grounded) {

            couldHaveJumped = true;

        }
        else {

            couldHaveJumped = false;

        }

        if (spacePressed) {

            spaceHeldBefore = true;

        }
        else {

            spaceHeldBefore = false;

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

    private void Jump() {

        canJumpNow = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        spaceHeldNow = Input.GetKey("space");
        if (Input.GetKeyDown("space")) {
            spaceHeld = true;
        }
        StartCoroutine(hasJumped());

    }

    private bool CanJump() {
        if (canJumpNow || couldHaveJumped) {
            return true;
        }
        return false;
    }

    private bool SpaceHeldDown() {
        if (spaceHeldNow || spaceHeldBefore) {
            return true;
        }
        return false;
    }

    IEnumerator JumpCooldownTimer() {
        yield return new WaitForSeconds(canJumpAfterSeconds + 0.1f);
        JumpCoodown = true;
    }
}
