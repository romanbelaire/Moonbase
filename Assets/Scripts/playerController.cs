using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    public float speed = 0;
    public float rotateSpeed = 0;
    public float maxSpeed = 5;

    private Rigidbody rb;

    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
        
    }

    private void FixedUpdate()
    {
        //this gets called several times a second
        updateDrag();
        Vector3 movement = new Vector3(movementY, 0.0f, 0.0f);
        

        movement = transform.rotation * movement;
        rb.AddForce(movement * speed);
        //speedLimit(); //do I even need this??
        rotateBody();
    }

    private void rotateBody()
    {
        //this function rotates the player based on horizontal input, then applies that rotation to the current velocity too (so it turns like a car)
        transform.Rotate(0, rotateSpeed * movementX, 0);//rotate around vertical axis
        Vector3 velocity = rb.velocity;
        Quaternion r = new Quaternion(0, rotateSpeed * movementX, 0, 0);
        velocity = Quaternion.Euler(0, transform.rotation.y, 0) * velocity;//change direction of velocity vector to the direction our rigidBody is facing
        rb.velocity = velocity;
    }

    private void updateDrag()
    {
        //this function updates the drag coefficient of the rigidbody so that while there is input, there is no drag.
        //this prevents gliding from happening without input
        if (Keyboard.current[Key.W].isPressed || Keyboard.current[Key.S].isPressed)
        {
            rb.drag = 1;
        }
        else
        {
            rb.drag = 20;
        }
    }

    private void speedLimit()
    {
        //checks and applies max movement speed limit
        float magnitude = rb.velocity.magnitude;
        float speedDiff = magnitude - maxSpeed;
        if (speedDiff > 0)
        {
            Vector3 counterMovement = rb.velocity.normalized * -1;
            rb.AddForce(counterMovement * speedDiff);
        } 
    }

    private void reduceDrift()
    {
        //this function performs some vector math to reduce drift
        //unity has no vector component functions, but we can get this by applying opposite rotation to our movement vector (giving vector aligned with x axis) and seeing what the Z component is.
        //next, we can reduce or remove drift on the Z axis by creating a proportional but opposite vector, rotating it back to the rigidbody's rotation, and applying.
    }
}
