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
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private float jumpSpeed;

    private bool isJumping;
    private Vector3 verticalVelocity;
    private bool inFreefall;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }



    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        //get the forward-backward, side-to-side, and jump movement input
        float lrInput = Input.GetAxis(leftRightInputName);
        float fbInput = Input.GetAxis(forwardBackwardInputName);
        bool jumpInput = Input.GetKey(jumpKey);
        
        // Create a unit vector in the resulting requested direction of movement.
        Vector3 horizontalDirection = Vector3.ClampMagnitude(transform.forward * fbInput + transform.right * lrInput, 1.0f);

        /*
                // If a jump was requested, start a jump if we are not already in a jump.
                // The isGrounded check is to prevent jumping in midair after falling off something. (In that case, you are not jumping, but also are not grounded.) 
                if ( jumpInput && !isJumping && charController.isGrounded )
                {
                    StartCoroutine(JumpAnimation());
                }
                // If not jumping, need to apply gravity or you apparently encounter issues with character not actually being grounded when you would expect it to be.
                if (!isJumping)
                {
                    verticalVelocity = Physics.gravity;
                }
        */

        if (charController.isGrounded)
        {
            isJumping = false;
            inFreefall = false;
            verticalVelocity = Vector3.zero;
            if (jumpInput)
            {
                verticalVelocity += Vector3.up * jumpSpeed;
                isJumping = true;
            }
        }
        else
        {
            if (!inFreefall)
            {
                inFreefall = true;
            }
            // Adjust the vertical speed based on gravity.
            verticalVelocity += Physics.gravity * Time.deltaTime;
        }
//        print(verticalVelocity);
        // Move the character based on the inputs
        Vector3 moveDisplacement = ((horizontalDirection * movementSpeed) + verticalVelocity) * Time.deltaTime;
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

}


