﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    float speedx = 1.5f;
    //float speedz = 5.0f;
    float gravity = 80.0f;
    Vector3 dir = new Vector3(0.0f, 0.0f, 0.0f);
    public Rigidbody bcol;
    public Transform bsize;
    bool flowing = true;
    float flowingmode = 0;
    public Transform psize;
    public Rigidbody pcol;
    public Transform gsize; // goal size
    public Transform msize; // ground size
    public float bzone = 0;
    // public float pzone = 0;

    // Use this for initialization
    void Start()
    {
        bcol = GetComponent<Rigidbody>();
        bsize = GetComponent<Transform>();

        GameObject player = GameObject.Find("Player1");
        if (player != null)
        {
            psize = player.transform;
            pcol = player.GetComponent<Rigidbody>();
        }
        GameObject ground = GameObject.Find("Ground");
        if (ground != null)
        {
            msize = ground.transform;
        }
        GameObject goal = GameObject.Find("Goal");
        if (goal != null)
        {
            gsize = goal.transform;
        }
        flowingmode = 0; // 0 start - normal line, 1 curved, 2 ??, 3 ??
        flowing = true;
    }

    void Update()
    {

    }
    

    // Update is called once per frame
    void FixedUpdate ()
    {

        var angleball = transform.position;
        var anglesref = gsize.position;
        Vector3 guple = new Vector3(- gsize.transform.localScale.x/2, 0.0f, gsize.transform.localScale.z / 2);
        Vector3 gupri = new Vector3(gsize.transform.localScale.x / 2, 0.0f, gsize.transform.localScale.z / 2);
        Vector3 gdole = new Vector3(- gsize.transform.localScale.x / 2, 0.0f, - gsize.transform.localScale.z / 2);
        Vector3 gdori = new Vector3(gsize.transform.localScale.x / 2, 0.0f, - gsize.transform.localScale.z / 2);
        /* var angleupri = Vector3.Angle(gupri, angleball);
         var angledori = Vector3.Angle(gdori, angleball);
         var angleuple = Vector3.Angle(guple, angleball);
         var angledole = Vector3.Angle(gdole, angleball);*/
        // Debug.Log(" angleupri: " + angleupri);
        //Debug.Log(" angledori: " + angledori);
        /* Debug.Log(" angleuple: " + angleuple);
         Debug.Log(" angledole: " + angledole);*/
        float angledori = Vector3.Angle(gdori, angleball);
        Vector3 crossdori = Vector3.Cross(gdori, angleball);

        float angleupri = Vector3.Angle(gupri, angleball);
        Vector3 crossupri = Vector3.Cross(gupri, angleball);

        float angleuple = Vector3.Angle(guple, angleball);
        Vector3 crossuple = Vector3.Cross(guple, angleball);
        float angledole = Vector3.Angle(gdole, angleball);
        Vector3 crossdole = Vector3.Cross(gdole, angleball);

        if (crossdori.y < 0) angledori = -angledori;
        if (crossupri.y < 0) angleupri = -angleupri;
        if (crossdori.y < 0) angledole = -angledole;
        if (crossupri.y < 0) angleuple = -angleuple;
        if (((angleupri > 15) && (angleupri <= 180)) && ((angledori < -15) && (angledori >= -180))) //&& (bsize.transform.position.z <= gsize.transform.localScale.z / 2))
        {
            Debug.Log("zone1");
        }
        if ((angleupri <= 15) && (angleupri >= -180) && ((angleuple >= -15) && (angleuple <= 180)))
        {
            Debug.Log("zone2");
        }
        if ((angledole > 15) && (angledole <= 180) && ((angleuple < -15) && (angleuple >= -180)))
        {
            Debug.Log("zone3");
        }
        if ((angledole <= 15) && (angledole >= -180) && ((angledori >= -15) && (angledori <= 180)))
        {
            Debug.Log("zone4");
        }

      
        // ----------------------------------------------------- unescape ground -------------------------------------//

        //right
        if (bsize.position.x + (bsize.localScale.x /2) > msize.localScale.x / 2)
            bsize.transform.position = new Vector3(-(msize.localScale.x / 2) + (bsize.localScale.x/2), bsize.position.y, bsize.position.z);
        //left
        if (bsize.position.x - (bsize.localScale.x /2) < -(msize.localScale.x / 2))
            bsize.transform.position = new Vector3((msize.localScale.x / 2) - (bsize.localScale.x / 2), bsize.position.y, bsize.position.z);
        //down
        if (bsize.position.z - (bsize.localScale.z / 2) < -(msize.localScale.z / 2))
            bsize.transform.position = new Vector3(bsize.position.x, bsize.position.y, (msize.localScale.z / 2) -(bsize.localScale.z / 2));
        //up
        if (bsize.position.z + (bsize.localScale.z / 2) > (msize.localScale.z / 2))
        {
           // Debug.Log("down to up");
            bsize.transform.position = new Vector3(bsize.position.x, bsize.position.y, -(msize.localScale.z / 2) + (bsize.localScale.z / 2));
        }


        if (flowing == true)
        {
            float maxGravDist = msize.localScale.x;
            float maxGravity = gravity;
            float dist;
            Vector3 adjusteddir;
            GameObject players = GameObject.Find("Player1");
            Controls playergravity = players.GetComponent<Controls>();
           // Debug.Log("x : " + bcol.velocity.x + " y : " + bcol.velocity.z);
            switch (playergravity.zone)
            {
                case 1: // right
                    adjusteddir = new Vector3(gsize.position.x + gsize.localScale.x / 2, bsize.position.y, bsize.position.z);
                    break;
                case 2: // up
                    adjusteddir = new Vector3(gsize.position.x, gsize.position.y, gsize.position.z + gsize.localScale.z / 2);
                       //   adjusteddir = new Vector3(gsize.position.x, gsize.position.y, gsize.position.z - gsize.localScale.z / 2);
                    break;
                case 3: // left
                    adjusteddir = new Vector3(gsize.position.x - gsize.localScale.x / 2, bsize.position.y, bsize.position.z);
                    break;
                case 4: // down
                    adjusteddir = new Vector3(gsize.position.x, gsize.position.y, gsize.position.z - gsize.localScale.z / 2);
                        //  adjusteddir = new Vector3(gsize.position.x, gsize.position.y, gsize.position.z + gsize.localScale.z / 2);
                    break;
                default:
                    adjusteddir = new Vector3(gsize.position.x, bsize.position.y, gsize.localScale.z);
                    break;
            }
            // GRAVITY AROUND HERE

            dist = Vector3.Distance(adjusteddir, bsize.position);
            if (dist <= maxGravDist)
            {
                Vector3 v = adjusteddir - bsize.position;
                bcol.AddForce(v.normalized * (1.0f - dist / maxGravDist) * maxGravity);
            }
            if (flowingmode == 1)
            {
              /*  if (bsize.transform.position != new Vector3(20.0f, 0.0f, 0.0f))
                {
                    bsize.transform.position = Vector3.Slerp(bsize.transform.position, new Vector3(19.0f, 0.0f, 0.0f), 0.1f);
                    Debug.Log("bsize " + bsize.transform.position);
                    Debug.Log("bcol velocity " + bcol.velocity);
                }*/
            }
        }

        if (flowing == false )
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //var rot = Quaternion.AngleAxis(psize.rotation.y, Vector3.right);
                //var lDirection = rot * Vector3.forward;
                dir = Quaternion.AngleAxis(psize.transform.eulerAngles.y, Vector3.down) * Vector3.right;
                dir.x *= msize.localScale.x / 2;
                GameObject player = GameObject.Find("Player1");
                Controls playercontrols = player.GetComponent<Controls>();

                if ((playercontrols.zone == 1) || (playercontrols.zone == 3))
                    dir.z *= -1;
                dir.z *= msize.localScale.z / 2;

                bcol.velocity = dir * speedx;

                flowingmode = 1;
                flowing = true;
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log("c");
                // curve ball
                dir = new Vector3(19.0f, 0.0f, 0.0f);
                bcol.AddForce(dir, ForceMode.VelocityChange);
                flowingmode = 1;
                flowing = true;
            }
            else
            {
                // replace ball on player
                bcol.rotation = Quaternion.identity;
                GameObject player = GameObject.Find("Player1");
                Controls playercontrols = player.GetComponent<Controls>();
                switch (playercontrols.zone)
                {
                    case 1: // right
                        bsize.position = new Vector3(psize.position.x + psize.localScale.z/2 + bsize.localScale.x/2 +0.1f, psize.position.y, psize.position.z);
                        break;
                    case 2: // up
                        bsize.position = new Vector3(psize.position.x, psize.position.y, psize.position.z + psize.localScale.z / 2 + bsize.localScale.z / 2 + 0.1f);
                        break;
                    case 3: // left
                        bsize.position = new Vector3(psize.position.x - psize.localScale.z / 2 - bsize.localScale.x / 2 - 0.1f, psize.position.y, psize.position.z);
                        break;
                    case 4: // down
                        bsize.position = new Vector3(psize.position.x, psize.position.y, psize.position.z - +psize.localScale.z / 2 - bsize.localScale.z / 2 - 0.1f);
                        break;
                }
                flowing = false;
            }
        }
    }

    

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("TOUCHEE--------------------------------------------------------" + col.gameObject.name);
        if (col.gameObject.name == "Player1")
        {
            switch (col.gameObject.GetComponent<Controls>().zone)
            {
                case 1:
                    bsize.position = new Vector3(psize.position.x + psize.localScale.x, psize.position.y, psize.position.z);
                    break;
                case 2:
                    bsize.position = new Vector3(psize.position.x, psize.position.y, psize.position.z + psize.localScale.z);
                    break;
                case 3:
                    bsize.position = new Vector3(psize.position.x - psize.localScale.x, psize.position.y, psize.position.z);
                    break;
                case 4:
                    bsize.position = new Vector3(psize.position.x, psize.position.y, psize.position.z - psize.localScale.z);
                    break;
            }
            flowing = false;
        }/*
         else if (col.gameObject.name == "Goal")
         {
             // win
         }*/
    }
}
