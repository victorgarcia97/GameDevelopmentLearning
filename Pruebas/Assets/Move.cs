using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    public float horizantalMove;
    public float verticalMove;
    private Vector3 playerInput;

    public CharacterController player;

    public float speed;
    private Vector3 movePlayer;
    public float gravity = 9.8f;
    public float fallSpeed;
    public float jumpForce;
    private bool isSprint;
        
    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;

    
    void Start()
    {
        player = GetComponent<CharacterController>();
        isSprint = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        horizantalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");
        
        
        playerInput = new Vector3(horizantalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        camDirection();

        movePlayer = playerInput.x * camRight + playerInput.z * camForward;
        movePlayer = movePlayer * speed;

        player.transform.LookAt(player.transform.position + movePlayer);

        setGravity();

        playerSkills();


        player.Move(movePlayer * Time.deltaTime);
        
    }

    void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    void setGravity()
    {
        if(player.isGrounded)
        {
            fallSpeed = -gravity * Time.deltaTime;
            movePlayer.y = fallSpeed;
        }
        else
        {
            fallSpeed -= gravity * Time.deltaTime;
            movePlayer.y = fallSpeed;
        }
    }

    void playerSkills()
    {
        if(player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallSpeed = jumpForce;
            movePlayer.y = fallSpeed;
        }
    }
}
