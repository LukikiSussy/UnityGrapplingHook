using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 8f;
    public float jumpPower = 16f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    public GrapplingGun grappleSc;

    void Update() {

        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && canJump()) {

            rb.velocity = new Vector2(rb.velocity.x, jumpPower);

        }

    }

    private void FixedUpdate() {

        if (!grappleSc.grapplingRopeSc.lineRenderer.enabled && canJump()) {
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

    private bool canJump() {

        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

    }
}
