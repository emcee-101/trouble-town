using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 4f;

    public float groundCheckSize = 0.4f;
    public LayerMask groundMask;
    public bool captureMouse = true;

    CharacterController cc;
    Collider coll;
    Vector3 velocity;
    bool isGrounded;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        coll = GetComponent<Collider>();

        if (captureMouse) Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        #region ground check
        // lowest point of the collider
        Vector3 low = new Vector3(coll.bounds.center.x, coll.bounds.min.y, coll.bounds.center.z);
        // ground check, true if the ground is within groundCheckSize units
        isGrounded = Physics.CheckSphere(low, groundCheckSize, groundMask);
        // ground check logic
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        #endregion
        #region Cursor lock
        // Activate when Escape is pressed
        if (Input.GetButtonDown("Cancel") && Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetButtonDown("Cancel") && Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        #endregion

        // Player Input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Input multiplied by orientation = input relative to player orientation
        Vector3 move = transform.right * x + transform.forward * z;

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            // needed vlecoity to jump to height h is always = Sqrt(h * -2f *gravity)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime); //falling
        cc.Move(move * speed * Time.deltaTime); //walking
    }

    // Drawing sphere in editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if(coll != null)
        {
            Vector3 low = new Vector3(coll.bounds.center.x, coll.bounds.min.y, coll.bounds.center.z);
            Gizmos.DrawSphere(low, groundCheckSize);
        }
    }
}