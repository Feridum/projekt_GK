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

    private float timer;
    // Use this for initialization
    void Start()
    {
        HeroController = FindObjectOfType<HeroController>();
        timer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer + 0.5 <= Time.time)
        {
            Vector3 position = new Vector3(Mathf.RoundToInt(this.transform.position.x), 2, Mathf.RoundToInt(this.transform.position.z));
            CmdBonus(position);
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
            }
        }

        //TOTO change tag to bonus

        bonuses = GameObject.FindGameObjectsWithTag("breakableWall");
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