using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController chControl;
    public GameObject cameraObj;
    public Transform groundCheck;
    public float groundDistanse = 0.4f;
    public LayerMask groundMask;
    public float mouseSensivityX = 300f;
    public float mouseSensivityY = 400f;
    public float walkSpeed = 10f;
    public float sprintSpeed = 20f;
    public float gravity = -9.8f;
    float verticalRotation = 0f;
    float speed;
    Vector3 velocity = new Vector3();
    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensivityX;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivityY;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation,-90f,90f);
        cameraObj.transform.localRotation = Quaternion.Euler(verticalRotation,0f,0f);
        transform.eulerAngles += new Vector3(0f,mouseX,0f);

        Vector3 dest = Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistanse, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -0.2f;
        }
        velocity.y += gravity * Time.deltaTime;
        chControl.Move(dest * Time.deltaTime * speed + velocity * Time.deltaTime);
    }

}
