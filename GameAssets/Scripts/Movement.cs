using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Movement variables
    private float horizontal;
    private float speed = 8f;
    private float jumpSpeed = 8f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //Dash variables
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 10f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [SerializeField] private TrailRenderer trail;

    void Update()
    {
        if (isDashing) {

            return;
        }
        
        horizontal = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump") && isGrounded()) {

            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        }

        if (Input.GetButtonUp("Jump") && body.velocity.y > 0f) {

            body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) {

            StartCoroutine(Dash());
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (isDashing) {

            return;
        }
        
        body.velocity = new Vector2(horizontal * speed, body.velocity.y);
    }

    private bool isGrounded(){

        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    private void Flip(){

        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) {

            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash() {

        canDash = false;
        isDashing = true;
        float originalGravity = body.gravityScale;
        body.gravityScale = 0f;
        body.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        trail.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trail.emitting = false;
        body.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
