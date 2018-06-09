using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExlposionController : NetworkBehaviour
{
    private HeroController HeroController;
    private GameObject[] heros;
    private GameObject[] bombs;
    private GameObject[] bonuses;
    public int bombPositionX;
    public int bombPositionY;
    public GameObject bonusPrefab;
    private GameObject currentBonus;
    private GameObject[] breakableWalls;

    private float timer;
    // Use this for initialization
    void Start()
    {
        HeroController = FindObjectOfType<HeroController>();
        timer = Time.time;
        currentBonus = Instantiate(bonusPrefab, new Vector3(0,0,0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer + 0.5 <= Time.time)
        {
            // timer = Time.time;
            Destroy(gameObject);
        }

        breakableWalls = GameObject.FindGameObjectsWithTag("breakableWall");
        foreach (GameObject wall in breakableWalls)
        {
            if (System.Math.Round(wall.transform.position.x) == System.Math.Round(this.transform.position.x) && System.Math.Round(wall.transform.position.z) == System.Math.Round(this.transform.position.z))
            {
                breakableWallController[] wallBonus = FindObjectsOfType<breakableWallController>();
                foreach(breakableWallController bonus in wallBonus)
                {
                    if (bonus.isBonus)
                    {
                        if (System.Math.Round(bonus.transform.position.x) == System.Math.Round(wall.transform.position.x) && System.Math.Round(bonus.transform.position.z) == System.Math.Round(wall.transform.position.z))
                        {
                            Vector3 position = new Vector3(Mathf.RoundToInt(this.transform.position.x), 1, Mathf.RoundToInt(this.transform.position.z));
                            CmdBonus(position);
                        }
                    }
                } 
                Destroy(wall);
            }
        }

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
            }
        }

        bonuses = GameObject.FindGameObjectsWithTag("bonus");
        foreach (GameObject bonus in bonuses)
        {
            if (bonus != null && currentBonus != null)
                if (bonus.GetInstanceID() != currentBonus.GetInstanceID())
                {
                    if (System.Math.Round(bonus.transform.position.x) == System.Math.Round(this.transform.position.x) && System.Math.Round(bonus.transform.position.z) == System.Math.Round(this.transform.position.z))
                    {
                        Destroy(bonus);
                    }
                }
        }
    }
    [Command]
    void CmdBonus(Vector3 position)
    {
        currentBonus = Instantiate(bonusPrefab, position, Quaternion.identity);
        NetworkServer.Spawn(currentBonus);
    }
}