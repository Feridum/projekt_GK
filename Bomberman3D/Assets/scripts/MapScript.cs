using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MapScript : NetworkBehaviour {

	// Use this for initialization
    public GameObject breakableWallPrefab;
    private GameObject currentWall;
    private GameObject[] breakAbleWalls;
    private GameObject[] unbreakAbleWalls;
    void Start () {
        generateMap();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void generateMap()
    {
        System.Random rnd = new System.Random();
        int iterator = 0;
        int counter;
        bool createdWall;
        for (int i = -18; i <= 18; i += 3)
        {
            if (i == -15 || i == -9 || i == -3 || i == 3 || i == 9 || i == 15)
                counter = 4;
            else
                counter = 8;

            breakAbleWalls = GameObject.FindGameObjectsWithTag("breakableWall");

            while (iterator < counter)
            {
                createdWall = createWall(i);
                if (createdWall)
                    ++iterator;
            }
            iterator = 0;
        }

        breakableWallController[] walls = FindObjectsOfType<breakableWallController>();
        foreach (breakableWallController wall in walls)
        {
            int value = rnd.Next(1, 4);
            if (value >= 1)
            {
                wall.isBonus = true;
            }
        }

    }

    private bool createWall(int randValueX)
    {
        int randValueZ;
        Vector3 position;
        System.Random rnd = new System.Random();
        randValueZ = rnd.Next(-6, 7);
        randValueZ *= 3;
        breakAbleWalls = GameObject.FindGameObjectsWithTag("breakableWall");
        if ((randValueX == -18 && randValueZ == -18) || (randValueX == -18 && randValueZ == -15) || (randValueX == -15 && randValueZ == -18) || (randValueX == 18 && randValueZ == 15) || (randValueX == 15 && randValueZ == 18) || (randValueX == 18 && randValueZ == 18))
            return false;
        foreach (GameObject wall in breakAbleWalls)
        {
            if (System.Math.Round(wall.transform.position.x) == randValueX && System.Math.Round(wall.transform.position.z) == randValueZ)
            {
                return false;
            }
        }
        unbreakAbleWalls = GameObject.FindGameObjectsWithTag("unbreakableWall");
        foreach (GameObject wall in unbreakAbleWalls)
        {
            if (System.Math.Round(wall.transform.position.x) == randValueX && System.Math.Round(wall.transform.position.z) == randValueZ)
            {
                return false;
            }
        }

        position = new Vector3(Mathf.RoundToInt(randValueX), 1, Mathf.RoundToInt(randValueZ));
        CmdWall(position);

        return true;
    }


    [Command]
    void CmdWall(Vector3 position)
    {
        if (NetworkServer.active)
        {
            currentWall = (GameObject)Instantiate(breakableWallPrefab, position, Quaternion.identity, transform.parent);
            NetworkServer.Spawn(currentWall);
        }
    }
}
