using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private string leftRightInputName;
    [SerializeField] private string forwardBackwardInputName;
    [SerializeField] private float movementSpeed;

    private CharacterController charController;

    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private AnimationCurve fallFallOff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private float jumpSpeed;

    private bool isJumping;
    private bool isFalling;
    private Vector3 verticalVelocity;
    // Serialize this so we can see it in the Unity Inspector.
    [SerializeField] private bool usePhysics;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        isJumping = false;
        isFalling = false;
        usePhysics = true;
    }



    // Update is called once per frame
    void Update()
    {
        // If the fire button has been pressed, toggle between jump/fall modes (so they are easy to compare to help decide which is preferred).
        if (Input.GetButtonDown("Fire1"))
        {
            usePhysics = !usePhysics;
        }
        if (usePhysics)
        {
            // Jump/fall based on real-world physics.
            PlayerMovePhysics();
        }
        else
        {
            // Jump/fall based on animation curves.
            PlayerMoveAnimation();
        }

    }

    private void PlayerMovePhysics()
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

        // Apply gravity: adjust the vertical velocity by the gravitational acceleration multiplied by the time elapsed.
        // If the character is on the ground, it will simply stay on the ground as it will collide with the ground.
        // (Apparently, this is actually necessary, because due to quirks with the CharacterController, the character sometimes ends up slightly above the ground and isGrounded is false.)
        verticalVelocity += Physics.gravity * Time.deltaTime;

        // Move the character based on the inputs.
        // Its displacement in this frame will be its current velocity vector (the sum of the horizontal and vertical velocities) multiplied by the elapsed time.
        Vector3 moveDisplacement = (horizontalVelocity + verticalVelocity) * Time.deltaTime;
        charController.Move(moveDisplacement);
    }

    private void PlayerMoveAnimation()
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
            // If a jump was requested, start a jump if we are not already in a jump.
            if (jumpInput && !isJumping)
            {
                StartCoroutine(JumpAnimation());
            }
        }
        else if (!isJumping && !isFalling)
        {
            // We're not grounded, we are not in a jump, and we are not currently falling, so we must have just started to fall. Start the falling animation.
            StartCoroutine(FallAnimation());
        }
        // If not jumping, need to apply gravity or you apparently encounter issues with character not actually being grounded when you would expect it to be.
        if (!isJumping)
        {
            //verticalVelocity = Physics.gravity * Time.deltaTime;
        }

        // Move the character based on the inputs.
        // Its displacement in this frame will be its current velocity vector (the sum of the horizontal and vertical velocities) multiplied by the elapsed time.
        Vector3 moveDisplacement = (horizontalVelocity + verticalVelocity) * Time.deltaTime;
        charController.Move(moveDisplacement);
    }

    //controls jump
    private IEnumerator JumpAnimation()
    {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;

        isJumping = true;
        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            // Set the velocity vector for this frame of the jump
            verticalVelocity = Vector3.up * jumpForce * jumpMultiplier;
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        charController.slopeLimit = 45.0f;
        isJumping = false;
    }

    //controls fall
    private IEnumerator FallAnimation()
    {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;

        isFalling = true;
        do
        {
            float fallForce = fallFallOff.Evaluate(timeInAir);
            // Set the velocity vector for this frame of the fall
            verticalVelocity = Vector3.up * fallForce * jumpMultiplier;
            print(fallForce + " " + verticalVelocity);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!charController.isGrounded);

        charController.slopeLimit = 45.0f;
        isFalling = false;
    }
}


