using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private string leftRightInputName;
    [SerializeField] private string forwardBackwardInputName;
    [SerializeField] private float movementSpeed;

    private CharacterController charController;

    //[SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;
    //[SerializeField] private KeyCode sprintKey;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float jumpSpeedMultiplier;

    //private bool isJumping;
    //private bool isRunning;
    //private bool isFalling;
    private Vector3 verticalVelocity;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        //isJumping = false;
        //isFalling = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        // Get the forward-backward, side-to-side, and jump movement inputs.
        float lrInput = Input.GetAxis(leftRightInputName);
        float fbInput = Input.GetAxis(forwardBackwardInputName);
        bool jumpInput = Input.GetKey(jumpKey);
        
        // Determine the horizontal velocity based on the requested horizontal direction of movement.
        // Do this by creating a unit vector in the requested direction, then multiplying it by the movement speed.
        Vector3 horizontalVelocity = Vector3.ClampMagnitude(transform.forward * fbInput + transform.right * lrInput, 1.0f) * movementSpeed;

        if (charController.isGrounded)
        {
            // If the character is on the ground, its vertical velocity should be zero.
            verticalVelocity = Vector3.zero;
            if (jumpInput)
            {
                // A jump was requested, so give the player a vertical velocity upward with speed jumpSpeed.
                verticalVelocity = Vector3.up * jumpSpeed;
            }
        }
        else
        {
            horizontalVelocity *= jumpSpeedMultiplier;
        }

        // Apply gravity: adjust the vertical velocity by the gravitational acceleration multiplied by the time elapsed.
        // If the character is on the ground, it will simply stay on the ground as it will collide with the ground.
        // (Apparently, this is actually necessary, because due to quirks with the CharacterController, the character sometimes ends up slightly above the ground and isGrounded is false.)
        verticalVelocity += Physics.gravity * gravityMultiplier * Time.deltaTime;

        // Move the character based on the inputs.
        // Its displacement in this frame will be its current velocity vector (the sum of the horizontal and vertical velocities) multiplied by the elapsed time.
        Vector3 moveDisplacement = (horizontalVelocity + verticalVelocity) * Time.deltaTime;
        charController.Move(moveDisplacement);
    }

}


