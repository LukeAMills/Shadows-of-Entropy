using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    //Handling
    public float rotationSpeed = 450f;
    public float walkspeed = 5f;
    public float runspeed = 8f;
    private float acceleration = 5f;

    //System 
    private Quaternion targetRotation;
    private Vector3 currentVelocityMod;

    //Components
    public Gun gun;
    private CharacterController controller;
    private Camera cam;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        ControlMouse();
        //ControlWASD();

        if (Input.GetButtonDown("Shoot"))   //single press
        {
            gun.Shoot();
        }
        else if (Input.GetButton("Shoot"))  //hold down
        {
            gun.ShootContinuous();
        }
 
    }

    void ControlMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.transform.position.y - transform.position.y));
        targetRotation = Quaternion.LookRotation(mousePos - new Vector3(transform.position.x, 0, transform.position.z));
        transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        currentVelocityMod = Vector3.MoveTowards(currentVelocityMod, input, acceleration * Time.deltaTime);
        Vector3 motion = currentVelocityMod;
        //Vector3 motion = input;
        motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? .7f : 1;    //so walking on angles isnt 1.4x as fast as any normal direction
        motion *= (Input.GetButton("Run")) ? runspeed : walkspeed;
        motion += Vector3.up * -8;
        controller.Move(motion * Time.deltaTime);
    }

    void ControlWASD()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (input != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(input);
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
        }

        currentVelocityMod = Vector3.MoveTowards(currentVelocityMod, input, acceleration*Time.deltaTime);
        Vector3 motion = currentVelocityMod;
        motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? .7f : 1;    //so walking on angles isnt 1.4x as fast as any normal direction
        motion *= (Input.GetButton("Run")) ? runspeed : walkspeed;
        motion += Vector3.up * -8;
        controller.Move(motion * Time.deltaTime);
    }
}
