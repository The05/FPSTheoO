using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public GameObject[] Blocks;
    int blockIndex = 0;
    public float gridSize = 1.0f;
    CharacterController characterController;
    private Camera headCamera;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>(); 
        headCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        for(int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString())){

            }
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }


        RaycastHit hit;
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
            
        if(Physics.Raycast(headCamera.transform.position, headCamera.transform.forward, out hit, 100, layerMask)){
            if (Input.GetMouseButtonDown(1))
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    Instantiate<GameObject>(Blocks[blockIndex], hit.point + 0.5f * hit.normal, Quaternion.LookRotation(hit.normal, Vector3.up));
                }
                else
                {
                Instantiate<GameObject>(Blocks[blockIndex], hit.transform.position + hit.normal, Quaternion.LookRotation(hit.normal, Vector3.up));
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (hit.transform.gameObject.tag == "Block")
                {
                Destroy(hit.transform.gameObject);
                }
            }
            
        }
    }
}

