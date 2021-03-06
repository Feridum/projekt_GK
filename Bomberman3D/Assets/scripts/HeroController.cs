﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class HeroController : NetworkBehaviour
{

    private float timer;
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
    public GameObject bombPrefab;
    private GameObject currentBomb;
    private IdPlayer[] playersID;
    private int bombCount;
    public int bombLimit = 1;
    private GameObject[] bonuses;
    private GameObject[] bombs;
    public GameObject breakableWallPrefab;
    private GameObject currentWall;
    private GameObject[] breakAbleWalls;
    private GameObject[] unbreakAbleWalls;
    public GameObject[] heros;


    // Use this for initialization
    void Start()
    {

        anim = gameObject.GetComponent<Animator>();
        startingPosition = target = transform.position;


        rotation = transform.rotation;
        timer = Time.time;
    }
    public override void OnStartLocalPlayer()
    {
        SkinnedMeshRenderer[] elements = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer element in elements)
            element.material.SetColor("_Color", Color.blue);
        setStartPosition();
    }

    private void setStartPosition()
    {
        heros = GameObject.FindGameObjectsWithTag("hero");
        if (heros.Length == 2)
        {
            GameObject hero1 = heros[0];
            GameObject hero2 = heros[1];
            if (System.Math.Round(hero1.transform.position.x) == System.Math.Round(hero2.transform.position.x))
            {
                if (System.Math.Round(hero2.transform.position.x) == 18)
                {
                    Vector3 position = new Vector3(-18, this.transform.position.y, -18);
                    hero2.transform.position = position;
                }
                else
                {
                    Vector3 position = new Vector3(18, this.transform.position.y, 18);
                    hero2.transform.position = position;
                }

            }
        }
    }


    // Update is called once per frame
    public void Update()
    {
        if (this.isLocalPlayer)
        {
            heros = GameObject.FindGameObjectsWithTag("hero");
            if (heros.Length == 2)
            // if (true) //TODO chage to line above (only for tests)
            {
                if (!(Input.GetKeyUp(moveDown) || Input.GetKeyUp(moveUp) || Input.GetKeyUp(moveLeft) || Input.GetKeyUp(moveRight)))
                {
                    anim.SetTrigger("Stop");
                }

                if (target == transform.position)
                {
                    t = 0;
                    startingPosition = transform.position;
                    rotation = transform.rotation;
                    if (Input.GetKey(moveLeft))
                    {
                        target = startingPosition;
                        if (canMoveInX(target, -1))
                        {
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
                    tryAddBomb();
                }
            }
        }
        checkCollisionsWithBonus();
        if (this.isLocalPlayer)
        {
            // checkCollisionsWithBonus();
            t += Time.deltaTime / moveSpeed;
            transform.position = Vector3.Lerp(transform.position, target, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed);

        }
    }

    private void checkCollisionsWithBonus()
    {
        BonusScript[] bonuses = GameObject.FindObjectsOfType<BonusScript>();
        foreach (BonusScript bonus in bonuses)
        {
            if (System.Math.Round(bonus.transform.position.x) == System.Math.Round(this.transform.position.x) && System.Math.Round(bonus.transform.position.z) == System.Math.Round(this.transform.position.z))
            {
                int id = this.transform.GetInstanceID();
                ++bombLimit;
                Destroy(bonus.gameObject);
            }
        }
    }

    private void tryAddBomb()
    {
        if (timer + 0.5 <= Time.time)
        {
            bombCount = countPlayerBombs();
            if (bombCount < bombLimit)
            {
                Vector3 roundedPosition = new Vector3(Mathf.RoundToInt(transform.position.x), 0, Mathf.RoundToInt(transform.position.z));
                addBomb(roundedPosition);
            }
        }
    }

    private int countPlayerBombs()
    {
        //playersID = FindObjectsOfType<IdPlayer>();
        //int temp = 0;
        //foreach (IdPlayer player in playersID)
        //{
        //    if (player.ID == this.GetInstanceID())
        //        temp++;
        //}
        //return temp;
        BombControllerBeta[] bom = FindObjectsOfType<BombControllerBeta>();
        int temp = 0;
        foreach (BombControllerBeta bomb in bom)
        {
            if (bomb.playerID == this.GetInstanceID())
                temp++;
        }
        return temp;
    }

    private void addBomb(Vector3 position)
    {
        if (canPlantBomb(position))
        {
            timer = Time.time;
            anim.SetTrigger("PlantBomb");
            CmdBomb(position);
        }
    }

    [Command(channel = 0)]
    void CmdBomb(Vector3 position)
    {
        currentBomb = (GameObject)Instantiate(bombPrefab, position, Quaternion.identity, transform.parent);//todo get all bombs check position and add field
        BombControllerBeta[] bom = FindObjectsOfType<BombControllerBeta>();
        foreach (BombControllerBeta bomb in bom)
        {
            if (System.Math.Round(bomb.transform.position.x) == System.Math.Round(this.transform.position.x) && System.Math.Round(bomb.transform.position.z) == System.Math.Round(this.transform.position.z))
            {
                bomb.playerID = this.GetInstanceID();
            }
        }
        //currentBomb.AddComponent<IdPlayer>().ID = this.GetInstanceID();
        NetworkServer.Spawn(currentBomb);
    }

    private bool canPlantBomb(Vector3 position)
    {
        if (position.x == 18 || position.x == 15 || position.x == 12 || position.x == 9 || position.x == 6 || position.x == 3 || position.x == 0 || position.x == -18 || position.x == -15 || position.x == -12 || position.x == -9 || position.x == -6 || position.x == -3)
        {
            if (position.z == 18 || position.z == 15 || position.z == 12 || position.z == 9 || position.z == 6 || position.z == 3 || position.z == 0 || position.z == -18 || position.z == -15 || position.z == -12 || position.z == -9 || position.z == -6 || position.z == -3)
                return true;
        }
        return false;
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
        bombs = GameObject.FindGameObjectsWithTag("bomb");
        foreach (GameObject bomb in bombs)
        {
            if ((System.Math.Round(bomb.transform.position.x) == System.Math.Round(this.transform.position.x - 3) && System.Math.Round(bomb.transform.position.z) == System.Math.Round(this.transform.position.z) && direction < 0) || (System.Math.Round(bomb.transform.position.x) == System.Math.Round(this.transform.position.x + 3) && System.Math.Round(bomb.transform.position.z) == System.Math.Round(this.transform.position.z) && direction > 0))
            {
                return false;
            }
        }

        breakAbleWalls = GameObject.FindGameObjectsWithTag("breakableWall");
        foreach (GameObject wall in breakAbleWalls)
        {
            if ((System.Math.Round(wall.transform.position.x) == System.Math.Round(this.transform.position.x - 3) && System.Math.Round(wall.transform.position.z) == System.Math.Round(this.transform.position.z) && direction < 0) || (System.Math.Round(wall.transform.position.x) == System.Math.Round(this.transform.position.x + 3) && System.Math.Round(wall.transform.position.z) == System.Math.Round(this.transform.position.z) && direction > 0))
            {
                return false;
            }
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
        bombs = GameObject.FindGameObjectsWithTag("bomb");
        foreach (GameObject bomb in bombs)
        {
            if ((System.Math.Round(bomb.transform.position.z) == System.Math.Round(this.transform.position.z - 3) && System.Math.Round(bomb.transform.position.x) == System.Math.Round(this.transform.position.x) && direction < 0) || (System.Math.Round(bomb.transform.position.z) == System.Math.Round(this.transform.position.z + 3) && System.Math.Round(bomb.transform.position.x) == System.Math.Round(this.transform.position.x) && direction > 0))
            {
                return false;
            }
        }
        breakAbleWalls = GameObject.FindGameObjectsWithTag("breakableWall");
        foreach (GameObject wall in breakAbleWalls)
        {
            if ((System.Math.Round(wall.transform.position.z) == System.Math.Round(this.transform.position.z - 3) && System.Math.Round(wall.transform.position.x) == System.Math.Round(this.transform.position.x) && direction < 0) || (System.Math.Round(wall.transform.position.z) == System.Math.Round(this.transform.position.z + 3) && System.Math.Round(wall.transform.position.x) == System.Math.Round(this.transform.position.x) && direction > 0))
            {
                return false;
            }
        }
        return true;
    }
}
