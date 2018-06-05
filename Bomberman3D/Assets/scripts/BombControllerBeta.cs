using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombControllerBeta : MonoBehaviour
{
    private Animator anim;
    private float timer;
    public GameObject explosionPrefab;
    private GameObject explosion;
    // Use this for initialization
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        timer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer + 5 <= Time.time) //change to 5
        {
            explode();
        }
    }

    public void explode()
    {
        createAllExplosion();
        Destroy(this.gameObject);
    }
    private void explosionUtil(Vector3 position)
    {
        explosion = (GameObject)Instantiate(explosionPrefab, position, Quaternion.identity);
    }

    private void createAllExplosion()
    {
        Vector3 roundedPosition = new Vector3(Mathf.RoundToInt(transform.position.x), 0, Mathf.RoundToInt(transform.position.z));
        explosionUtil(roundedPosition);

        roundedPosition = new Vector3(Mathf.RoundToInt(transform.position.x), 0, Mathf.RoundToInt(transform.position.z) - 3);
        if (canExplodeBelow(roundedPosition))
            explosionUtil(roundedPosition);

        roundedPosition = new Vector3(Mathf.RoundToInt(transform.position.x), 0, Mathf.RoundToInt(transform.position.z) + 3);
        if (canExplodeAbove(roundedPosition))
            explosionUtil(roundedPosition);

        roundedPosition = new Vector3(Mathf.RoundToInt(transform.position.x + 3), 0, Mathf.RoundToInt(transform.position.z));
        if (canExplodeRight(roundedPosition))
            explosionUtil(roundedPosition);

        roundedPosition = new Vector3(Mathf.RoundToInt(transform.position.x - 3), 0, Mathf.RoundToInt(transform.position.z));

        if (canExplodeLeft(roundedPosition))
            explosionUtil(roundedPosition);
    }
    private bool canExplodeBelow(Vector3 target)
    {
        if (((target.x > -19 && target.x < -17) || (target.x > -13 && target.x < -11) || (target.x > -7 && target.x < -5) || (target.x > -1 && target.x < 1) || (target.x > 5 && target.x < 7) || (target.x > 11 && target.x < 13) || (target.x > 17 && target.x < 19)) && ((target.z) >= -19))
        {
            return true;
        }
        return false;
    }

    private bool canExplodeAbove(Vector3 target)
    {
        if (((target.x > -19 && target.x < -17) || (target.x > -13 && target.x < -11) || (target.x > -7 && target.x < -5) || (target.x > -1 && target.x < 1) || (target.x > 5 && target.x < 7) || (target.x > 11 && target.x < 13) || (target.x > 17 && target.x < 19)) && ((target.z) <= 19))
        {
            return true;
        }
        return false;
    }

    private bool canExplodeRight(Vector3 target)
    {
        if (((target.z > -19 && target.z < -17) || (target.z > -13 && target.z < -11) || (target.z > -7 && target.z < -5) || (target.z > -1 && target.z < 1) || (target.z > 5 && target.z < 7) || (target.z > 11 && target.z < 13) || (target.z > 17 && target.z < 19)) && ((target.x) <= 19))
        {
            return true;
        }
        return false;
    }
    private bool canExplodeLeft(Vector3 target)
    {
        if (((target.z > -19 && target.z < -17) || (target.z > -13 && target.z < -11) || (target.z > -7 && target.z < -5) || (target.z > -1 && target.z < 1) || (target.z > 5 && target.z < 7) || (target.z > 11 && target.z < 13) || (target.z > 17 && target.z < 19)) && ((target.x) >= -19))
        {
            return true;
        }
        return false;
    }
}
