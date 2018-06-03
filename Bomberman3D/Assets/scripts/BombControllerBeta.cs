using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombControllerBeta : MonoBehaviour {
    private Animator anim;
    private float timer;
    public GameObject explosionPrefab;
    private GameObject explosion;
    private bool isExploded = false;
    HeroController hero = new HeroController();
	// Use this for initialization
	void Start () {
        anim = gameObject.GetComponent<Animator>();
        timer = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if (timer+2 <= Time.time && !isExploded) //change to 5
        {
            isExploded = true;
            Vector3 roundedPosition = new Vector3(Mathf.RoundToInt(transform.position.x)-1, 0, Mathf.RoundToInt(transform.position.z));
            explosion = (GameObject)Instantiate(explosionPrefab, roundedPosition, Quaternion.identity);
            Destroy(explosion, 2.0f);
            roundedPosition = new Vector3(Mathf.RoundToInt(transform.position.x), 0, Mathf.RoundToInt(transform.position.z)-3);

               if(hero.canMoveInZ(roundedPosition,-1) )
                    explosion = (GameObject)Instantiate(explosionPrefab, roundedPosition, Quaternion.identity);
            Destroy(explosion, 2.0f);
            Destroy(this.gameObject);


        }

        anim.SetTrigger("rotating");
	}
}
