using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
    private HeroController HeroController;
    public int bombPositionX;
    public int bombPositionY;
    public GameObject explosionPrefab;
	// Use this for initialization
	void Start () {
        HeroController = FindObjectOfType<HeroController>();
    }
	
	// Update is called once per frame
	void Update () {

    }
    public void explode()
    {
        Instantiate(explosionPrefab, new Vector3(bombPositionX, bombPositionY, 0), Quaternion.identity);
    }
}
