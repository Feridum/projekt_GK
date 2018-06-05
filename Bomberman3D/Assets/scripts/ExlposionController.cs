using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExlposionController : MonoBehaviour {
    private HeroController HeroController;
    private GameObject[] heros;
    private GameObject[] bombs;
    public int bombPositionX;
    public int bombPositionY;

    private float timer;
    // Use this for initialization
    void Start () {
        HeroController = FindObjectOfType<HeroController>();
        timer = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if (timer + 0.5 <= Time.time)
        {
            // timer = Time.time;
            Destroy(gameObject);
        }
            //TODO check position of breakable wall
            heros = GameObject.FindGameObjectsWithTag("hero");
            foreach (GameObject hero in heros)
            {
                if (System.Math.Round(hero.transform.position.x) == this.transform.position.x && System.Math.Round(hero.transform.position.z) == this.transform.position.z)
                {
                    Destroy(hero);
                    Debug.Log("The end");
                }
            }
        bombs = GameObject.FindGameObjectsWithTag("bomb");
        foreach (GameObject bomb in bombs)
        {
            if (System.Math.Round(bomb.transform.position.x) == this.transform.position.x && System.Math.Round(bomb.transform.position.z) == this.transform.position.z)
            {
                BombControllerBeta other = (BombControllerBeta)bomb.GetComponent(typeof(BombControllerBeta));
                other.explode();
                Debug.Log("The end");
            }
        }
    }
}

