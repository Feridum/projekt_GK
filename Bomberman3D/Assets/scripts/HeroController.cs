using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    private Animator anim;
    private Vector3 prevLocation;
    private Vector3 moveDirecion;
    public float moveSpeed;
    public float rotationSpeed;
    public KeyCode moveUp;
    public KeyCode moveDown;
    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode plantBomb;


    // Use this for initialization
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        moveDirecion = Vector3.zero;
        prevLocation = transform.position;
        if (Input.GetKey(moveLeft))
        {
            moveDirecion.x = -1;
            anim.SetTrigger("Move");
        }
        if (Input.GetKey(moveRight))
        {
            moveDirecion.x = 1;
            anim.SetTrigger("Move");
        }
        if (Input.GetKey(moveUp))
        {
            moveDirecion.z = 1;
            anim.SetTrigger("Move");
        }
        if (Input.GetKey(moveDown))
        {
            moveDirecion.z = -1;
            anim.SetTrigger("Move");
        }
        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetTrigger("PlantBomb");
        }
        if (Input.GetKeyUp(moveDown) || Input.GetKeyUp(moveUp) || Input.GetKeyUp(moveLeft) || Input.GetKeyUp(moveRight))
        {
            anim.SetTrigger("Stop");
        }
        if (moveDirecion != Vector3.zero)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + moveDirecion.normalized, Time.fixedDeltaTime * moveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position - prevLocation), Time.fixedDeltaTime * rotationSpeed);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
       
    }
}
