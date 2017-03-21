﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {

    public Rigidbody pcol;
    public Rigidbody bcol;
    public Transform psize;
    public Transform gsize;
    public Transform msize;
    public int zone; // 1 = right / 2 = up / 3 = left / 4 = down
    Vector3 rotadjusted = Vector3.zero;

    // Use this for initialization
    void Start ()
    {
        pcol = GetComponent<Rigidbody>();
        psize = GetComponent<Transform>();
        psize.transform.position = new Vector3(1.5f,0.0f,0.0f);
        GameObject goal = GameObject.Find("Goal");
        if (goal != null)
        {
            gsize = goal.transform;
        }
        GameObject ball = GameObject.Find("Ball");
        if (ball != null)
        {
            bcol = ball.GetComponent<Rigidbody>();
        }
        GameObject ground = GameObject.Find("Ground");
        if (ground != null)
        {
            msize = ground.transform;
        }
        zone = 1;
    }

    // Update is called once per frame
    void Update () {
        
    }

    void FixedUpdate()
    {
        float moveV = Input.GetAxis("Vertical");

        // up

        float posx = psize.position.x;
        float posy = psize.position.z;
        float gposx = gsize.localScale.x/2; // scale because pos is 0,0,0
        float gposy = gsize.localScale.z/2; // scale because pos is 0,0,0
        float offsetx = 1.0f;//psize.localScale.x;
        float offsety = psize.localScale.z;
        float maxx = gposx + offsetx;
        float maxy = gposy + offsety;
        float speed = 0.2f;
        Vector3 readjusted = psize.position;
        // Vector3 rot = psize.transform.rotation.eulerAngles;
        if (posx > maxx)
            posx = maxx;
        if (posx < -maxx)
            posx = -maxx;
        if (posy > maxy)
            posy = maxy;
        if (posy < -maxy)
            posy = -maxy;
        if ((posy < maxy) && (posy > -maxy) && ((posx != maxx) && (posx != -maxx)))
        {
            Debug.Log("lost inside2");
        }
        if ((posx < maxx) && (posx > -maxx) && ((posy != maxy) && (posy != -maxy)))
        {
            Debug.Log("lost inside");
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //---------------------------------------- TODO : overflow everywhere instead of - move on lane directly?-------------------------------------
            if (posx == maxx)
            {
                if ((posy + (moveV * speed)) < maxy)
                {
                    readjusted = new Vector3(posx, 0.0f, posy + (moveV * speed)); // move up move up
                    zone = 1;
                }
                else
                {
                    float overflow = (posy + (moveV * speed)) - maxy;
                    // go right
                    readjusted = new Vector3(posx - overflow, 0.0f, maxy);
                    if (rotadjusted.y > -34.0f)
                    {
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, -35.0f, transform.eulerAngles.z);
                        rotadjusted.y = -35.0f;
                    }
                    zone = 2;
                }
            }
            else
            {
                if ((posy == maxy) && (posx != -maxx))
                {
                    if (posy + (moveV * speed) > -maxx)
                    {
                        readjusted = new Vector3(posx - (moveV * speed), 0.0f, posy);
                        zone = 2;
                    }
                    else
                    {
                        // USELESS CAUSE CORNER MAXYMAXX and cba to delete (just in case)
                        readjusted = new Vector3(-maxx, 0.0f, posy - (moveV * speed));
                        zone = 3;
                    }
                }
                else if (posx == -maxx)
                {
                    if ((posy - (moveV * speed)) > -maxy)
                    {
                        //down lane
                        readjusted = new Vector3(posx, 0.0f, posy - (moveV * speed));
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, -180.0f, transform.eulerAngles.z);
                        rotadjusted.y = -180.0f;
                        zone = 3;
                    }
                    else
                    {
                        //left corner
                        readjusted = new Vector3(posx + (moveV * speed), 0.0f, -maxy);
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90.0f, transform.eulerAngles.z);
                        rotadjusted.y = 90.0f;
                        zone = 4;
                    }
                }
                else if (posy == -maxy)
                {
                    if ((posx + (moveV * speed)) < maxx)
                    {
                        //left lane
                        readjusted = new Vector3(posx + (moveV * speed), 0.0f, posy);
                        zone = 4;
                    }
                    else
                    {
                        //up corner
                        readjusted = new Vector3(maxx, 0.0f, posy + (moveV * speed));
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0.0f, transform.eulerAngles.z);
                        rotadjusted.y = 0.0f;
                        zone = 1;
                    }

                }
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (posx == maxx)
            {
                if ((posy + (moveV * speed)) < -maxy)
                {
                    // go down
                    float overflow = (posy + (moveV * speed)) - (-maxy);
                    readjusted = new Vector3(posx + overflow, 0.0f, -maxy);
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90.0f, transform.eulerAngles.z);
                    rotadjusted.y = 90.0f;
                    zone = 4;
                }
                else
                {
                    readjusted = new Vector3(posx, 0.0f, posy + (moveV * speed)); // right to down

                    zone = 1;
                }
            }
            else
            {
                if ((posy == -maxy)/* && (posx != -maxx)*/) // commented double check to do everywhere?
                {
                    if (posx + (moveV * speed) > -maxx)
                    {
                        //  down lane
                        readjusted = new Vector3(posx + (moveV * speed), 0.0f, posy);
                        zone = 4;
                    }
                    else
                    {
                        // down to left
                        readjusted = new Vector3(-maxx, 0.0f, posy - (moveV * speed));
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, -180.0f, transform.eulerAngles.z);
                        rotadjusted.y = -180.0f;
                        zone = 3;

                    }
                }
                else if (posx == -maxx)
                {
                    if ((posy - (moveV * speed)) < maxy)
                    {
                        // left lane
                        readjusted = new Vector3(posx, 0.0f, posy - (moveV * speed));
                        zone = 3;
                    }
                    else
                    {
                        // left to up
                        readjusted = new Vector3(posx - (moveV * speed), 0.0f, maxy);
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, -90.0f, transform.eulerAngles.z);
                        rotadjusted.y = -90.0f;
                        zone = 2;
                    }
                }
                else if (posy == maxy)
                {
                    if ((posx - (moveV * speed)) < maxx)
                    {
                        //up lane
                        readjusted = new Vector3(posx - (moveV * speed), 0.0f, posy);
                        zone = 2;
                    }
                    else
                    {
                        // up to right
                        readjusted = new Vector3(maxx, 0.0f, posy + (moveV * speed));
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0.0f, transform.eulerAngles.z);
                        rotadjusted.y = 0.0f;
                        zone = 1;
                    }

                }
            }
        }
        float angle = 0.0f;
        switch (zone)
        {
            case 1:
                if ((Input.mousePosition.y >= Camera.main.pixelHeight / 2) && (Input.mousePosition.y < Camera.main.pixelHeight) && (Input.mousePosition.y > 0))
                {
                    angle = (Input.mousePosition.y - Camera.main.pixelHeight / 2) / (Camera.main.pixelHeight / 2) * 65.0f;
                }
                else if (Input.mousePosition.y < Camera.main.pixelHeight / 2)
                {
                    angle = (Input.mousePosition.y - Camera.main.pixelHeight / 2) / (Camera.main.pixelHeight / 2) * 65.0f;
                }
                break;
            case 2:
                if ((Input.mousePosition.x >= Camera.main.pixelWidth / 2) && (Input.mousePosition.x < Camera.main.pixelWidth) && (Input.mousePosition.x > 0))
                {
                    angle = (Input.mousePosition.x - Camera.main.pixelWidth / 2) / (Camera.main.pixelWidth / 2) * -65.0f;
                    angle -= 90.0f;
                }
                else if (Input.mousePosition.x < Camera.main.pixelWidth / 2)
                {
                    angle = (Input.mousePosition.x - Camera.main.pixelWidth / 2) / (Camera.main.pixelWidth / 2) * -65.0f;
                    angle -= 90.0f;
                }
                break;
            case 3:
                if ((Input.mousePosition.y >= Camera.main.pixelHeight / 2) && (Input.mousePosition.y < Camera.main.pixelHeight) && (Input.mousePosition.y > 0))
                {
                    angle = (Input.mousePosition.y - Camera.main.pixelHeight / 2) / (Camera.main.pixelHeight / 2) * -65.0f;
                    angle -= 180.0f;
                }
                else if (Input.mousePosition.y < Camera.main.pixelHeight / 2)
                {
                    angle = (Input.mousePosition.y - Camera.main.pixelHeight / 2) / (Camera.main.pixelHeight / 2) * -65.0f;
                    angle -= 180.0f;
                }
                break;
            case 4:
                if ((Input.mousePosition.x >= Camera.main.pixelWidth / 2) && (Input.mousePosition.x < Camera.main.pixelWidth) && (Input.mousePosition.x > 0))
                {
                    angle = (Input.mousePosition.x - Camera.main.pixelWidth / 2) / (Camera.main.pixelWidth / 2) * 65.0f;
                    angle += 90.0f;
                }
                else if (Input.mousePosition.x < Camera.main.pixelWidth / 2)
                {
                    angle = (Input.mousePosition.x - Camera.main.pixelWidth / 2) / (Camera.main.pixelWidth / 2) * 65.0f;
                    angle += 90.0f;
                }
                break;
            default:
                break;
        }
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.down);
        psize.transform.position = readjusted;
    }
}
