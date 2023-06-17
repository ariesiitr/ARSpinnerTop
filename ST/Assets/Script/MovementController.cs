using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Joystick joystick;
    private Rigidbody rb;
    public float speed = 2f;
    private Vector3 velocityVector = Vector3.zero;
    public float maxVelocity = 4f;
    public float tilt = 10f  ;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //joystick input
        float x_input = joystick.Horizontal;
        float z_input = joystick.Vertical;

        //movement value 
        Vector3 movementHorizontal = transform.right * x_input;
        Vector3 movementVertical = transform.forward * z_input;

        //movement vector combine
        Vector3 movementVector = (movementHorizontal + movementVertical).normalized * speed;
        //Adding tilt 
        transform.rotation = Quaternion.Euler(joystick.Vertical * speed * tilt*0.7f, 0, -1 * joystick.Horizontal * speed * tilt*0.7f);

        Move(movementVector);
    }

    void Move(Vector3 movementVelocityVector)
    {
        velocityVector =  movementVelocityVector; 
    }

    private void FixedUpdate()
    {
        if(velocityVector != Vector3.zero)
        {
            //applying force
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = velocityVector - velocity;
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocity, maxVelocity);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocity, maxVelocity);
            velocityChange.y = 0f;
            rb.AddForce(velocityChange, ForceMode.Acceleration);
        }
       
       
    }

}
