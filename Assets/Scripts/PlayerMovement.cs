using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;
    [SerializeField] private float movementSpeed;

    private CharacterController charController;

    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;

    private bool isJumping;
    private Vector3 verticalMove;

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
        //get the movement input
        float horizInput = Input.GetAxis(horizontalInputName);
        float vertInput = Input.GetAxis(verticalInputName);

        //apply movement
        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        //charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed);

        //get jump input
        JumpInput();
        if (!isJumping)
        {
            verticalMove = Physics.gravity;
        }
        charController.Move(((Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed) + verticalMove) * Time.deltaTime);
    }

    private void JumpInput()
    {
        //checks for the jump key being pressed
        // The isGrounded check is to prevent jumping in midair after falling off something. (In that case, you are not jumping, but also are not grounded.) 
        if (Input.GetKey(jumpKey) && !isJumping && charController.isGrounded)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    //controls jump
    private IEnumerator JumpEvent()
    {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;

        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            verticalMove = Vector3.up * jumpForce * jumpMultiplier;
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        charController.slopeLimit = 45.0f;
        isJumping = false;
    }
}


