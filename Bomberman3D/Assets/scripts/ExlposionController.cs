using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExlposionController : NetworkBehaviour
{
    private GameObject[] heros;
    private GameObject[] bombs;
    private BonusScript[] bonuses;
    public int bombPositionX;
    public int bombPositionY;
    public GameObject bonusPrefab;
    private GameObject currentBonus;
    private GameObject[] breakableWalls;
    private bool exploded = false;

    private float timer;
    // Use this for initialization
    void Start()
    {
        timer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        System.Random rnd = new System.Random();
        if (timer + 0.5 <= Time.time)
        {
            // timer = Time.time;
            Destroy(gameObject);
        }

        if (!exploded)
        {
            exploded = true;
            bonuses = GameObject.FindObjectsOfType<BonusScript>();
            foreach (BonusScript bonus in bonuses)
            {
                if (currentBonus == null)
                {
                    if (System.Math.Round(bonus.transform.position.x) == System.Math.Round(this.transform.position.x) && System.Math.Round(bonus.transform.position.z) == System.Math.Round(this.transform.position.z))
                    {
                        Destroy(bonus.gameObject);
                        continue;
                    }
                }
                if (bonus != null && currentBonus != null)
                    if (bonus.GetInstanceID() != currentBonus.GetInstanceID())
                    {
                        if (System.Math.Round(bonus.transform.position.x) == System.Math.Round(this.transform.position.x) && System.Math.Round(bonus.transform.position.z) == System.Math.Round(this.transform.position.z))
                        {
                            Destroy(bonus.gameObject);
                        }
                    }
            }
        }
        breakableWalls = GameObject.FindGameObjectsWithTag("breakableWall");
        foreach (GameObject wall in breakableWalls)
        {
            if (System.Math.Round(wall.transform.position.x) == System.Math.Round(this.transform.position.x) && System.Math.Round(wall.transform.position.z) == System.Math.Round(this.transform.position.z))
            {
                breakableWallController[] wallBonus = FindObjectsOfType<breakableWallController>();
                foreach (breakableWallController bonus in wallBonus)
                {
                    if (System.Math.Round(bonus.transform.position.x) == System.Math.Round(wall.transform.position.x) && System.Math.Round(bonus.transform.position.z) == System.Math.Round(wall.transform.position.z))
                    {
                        if (bonus.isBonus)
                        {

                            Vector3 position = new Vector3(Mathf.RoundToInt(this.transform.position.x), 0, Mathf.RoundToInt(this.transform.position.z));
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


    }
    [Command]
    void CmdBonus(Vector3 position)
    {
        currentBonus = Instantiate(bonusPrefab, position, Quaternion.identity);
        NetworkServer.Spawn(currentBonus);
    }
}