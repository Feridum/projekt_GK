using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    private Animator anim;
    private float t;
    private Vector3 startingPosition;
    private Vector3 target;
    private Quaternion rotation;
    public float moveSpeed = 1;
    public float rotationSpeed = 0.1f;
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode plantBomb = KeyCode.Space;


    // Use this for initialization
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        startingPosition = target = transform.position;
        rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == transform.position)
        {
            t = 0;
            startingPosition = transform.position;
            rotation = transform.rotation;
            if (Input.GetKey(moveLeft))
            {
                target = startingPosition;
                if (canMoveInX(target, -1)) {
                    target.x += -3;
                }
                rotation = Quaternion.Euler(0, 90, 0);
                anim.SetTrigger("Move");
            }
            else if (Input.GetKey(moveRight))
            {
                target = startingPosition;
                if (canMoveInX(target, 1))
                {
                    target.x += 3;
                }
                rotation = Quaternion.Euler(0, -90, 0);
                anim.SetTrigger("Move");
            }
            else if (Input.GetKey(moveUp))
            {
                target = startingPosition;
                if (canMoveInZ(target, 1))
                {
                    target.z += 3;
                }
                rotation = Quaternion.Euler(0, 180, 0);
                anim.SetTrigger("Move");
            }
            else if (Input.GetKey(moveDown))
            {
                target = startingPosition;
                if (canMoveInZ(target, -1))
                {
                    target.z += -3;
                }
                rotation = Quaternion.Euler(0, 0, 0);
                anim.SetTrigger("Move");
            }
        }
        if (Input.GetKey(plantBomb))
        {
            anim.SetTrigger("PlantBomb");
        }
        if (Input.GetKeyUp(moveDown) || Input.GetKeyUp(moveUp) || Input.GetKeyUp(moveLeft) || Input.GetKeyUp(moveRight))
        {
            anim.SetTrigger("Stop");
        }
        t += Time.deltaTime / moveSpeed;
        transform.position = Vector3.Lerp(transform.position, target, t);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed);


    }

    private bool canMoveInX(Vector3 target, int direction)
    {
        if ((target.x > -19 && target.x < -17) || (target.x > -13 && target.x < -11) || (target.x > -7 && target.x < -5) || (target.x > -1 && target.x < 1) || (target.x > 5 && target.x < 7) || (target.x > 11 && target.x < 13) || (target.x > 17 && target.x < 19))
        {
            if ((target.z > -16 && target.z < -14) || (target.z > -10 && target.z < -8) || (target.z > -4 && target.z < -2) || (target.z > 2 && target.z < 4) || (target.z > 8 && target.z < 10) || (target.z > 14 && target.z < 16))
            {
                return false;
            }
        }
        if (((target.x - 3) <= -19 && direction == -1) || ((target.x + 3) >= 19 && direction == 1))
        {
            return false;
        }
        return true;
    }

    private bool canMoveInZ(Vector3 target, int direction)
    {
        if ((target.z > -19 && target.z < -17) || (target.z > -13 && target.z < -11) || (target.z > -7 && target.z < -5) || (target.z > -1 && target.z < 1) || (target.z > 5 && target.z < 7) || (target.z > 11 && target.z < 13) || (target.z > 17 && target.z < 19))
        {
            if ((target.x > -16 && target.x < -14) || (target.x > -10 && target.x < -8) || (target.x > -4 && target.x < -2) || (target.x > 2 && target.x < 4) || (target.x > 8 && target.x < 10) || (target.x > 14 && target.x < 16))
            {
                return false;
            }
        }
        if (((target.z - 3) <= -19 && direction == -1) || ((target.z + 3) >= 19 && direction == 1))
        {
            return false;
        }
        return true;
    }
}
