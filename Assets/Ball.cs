using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    float speedx = 15f;
    float speedz = 5f;
    float gravity = 0.5f;
    Vector3 dir = new Vector3(0.0f, 0.0f, 0.0f);
    public Rigidbody bcol;
    public Transform bsize;
    bool flowing = true;
    float flowingmode = 0;
    public Transform psize;
    public Rigidbody pcol;
    public Transform gsize; // goal size
    public Transform msize; // ground size

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
        dir = new Vector3(-speedx, 0.0f, 0.0f);
       // bcol.AddForce(dir, ForceMode.VelocityChange);
        flowingmode = 0; // 0 start - normal line, 1 curved, 2 ??, 3 ??
        flowing = true;
    }

    void Update()
    {

    }
    

    // Update is called once per frame
    void FixedUpdate ()
    {



        /* float bosx = bsize.transform.position.x;
         float bosz = bsize.transform.position.z;
         float posx = psize.transform.position.x;
         float posz = psize.transform.position.z;
         float psizex = psize.localScale.x ;
         float psizey = psize.localScale.z / 2;
         float msizex = msize.localScale.x / 2;
         float msizey = msize.localScale.z / 2;
         float gsizex = gsize.localScale.x / 2;
         float gsizey = gsize.localScale.z / 2;*/
        //Debug.Log("lossy : " + msize.worldToLocalMatrix);
        // ----------------------------------------------------- unescape ground -------------------------------------//
        if (bsize.position.x + (bsize.localScale.x /2) > msize.localScale.x*10 / 2)
            bsize.transform.position = new Vector3(-(msize.localScale.x *10 / 2) + (bsize.localScale.x/2), bsize.position.y, bsize.position.z);
        if (bsize.position.x - (bsize.localScale.x /2) < -(msize.localScale.x*10 / 2))
            bsize.transform.position = new Vector3((msize.localScale.x *10/ 2) - (bsize.localScale.x / 2), bsize.position.y, bsize.position.z);
        
        if (bsize.position.z - (bsize.localScale.z / 2) < -(msize.localScale.z *10 / 2))
            bsize.transform.position = new Vector3(bsize.position.x, bsize.position.y, (msize.localScale.z * 10 / 2) -(bsize.localScale.z / 2));
        if (bsize.position.z + (bsize.localScale.z / 2) > (msize.localScale.z *10 / 2))
            bsize.transform.position = new Vector3(bsize.position.x, bsize.position.y, -(msize.localScale.z * 10 / 2) + (bsize.localScale.z / 2));


        if (flowing == true)
        {
            float maxGravDist = 40.0f;
            float maxGravity = 35.0f;
            float dist;
            Vector3 adjusteddir;
            GameObject players = GameObject.Find("Player1");
            Controls playergravity = players.GetComponent<Controls>();
            switch (playergravity.zone)
            {
                case 1: // right
                    adjusteddir = new Vector3(gsize.position.x + gsize.localScale.x / 2, psize.position.y, psize.position.z);
                    break;
                case 2: // up
                    adjusteddir = new Vector3(psize.position.x, psize.position.y, gsize.position.z + gsize.localScale.z / 2);
                    break;
                case 3: // left
                    adjusteddir = new Vector3(gsize.position.x - gsize.localScale.x / 2, psize.position.y, psize.position.z);
                    break;
                case 4: // down
                    adjusteddir = new Vector3(psize.position.x, psize.position.y, gsize.position.z - gsize.localScale.z / 2);
                    break;
                default:
                    adjusteddir = new Vector3(gsize.position.x, psize.position.y, gsize.localScale.z);
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
            Debug.Log("flowing");
        }

        if (flowing == false )
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("space");
                var rot = Quaternion.AngleAxis(psize.rotation.y, Vector3.right);
                var lDirection = rot * Vector3.forward;
               // var wDirection = transform.TransformDirection(lDirection);



                // line ball
                GameObject player = GameObject.Find("Player1");
                Controls playercontrols = player.GetComponent<Controls>();
                switch (playercontrols.zone)
                {
                    case 1:
                        dir = new Vector3(speedx, 0.0f, 0.0f);
                        break;
                    case 2:
                        dir = new Vector3(0.0f, 0.0f, speedz);
                        break;
                    case 3:
                        dir = new Vector3(-speedx, 0.0f, 0.0f);
                        break;
                    case 4:
                        dir = new Vector3(0.0f, 0.0f, -speedz);
                        break;
                }
                bcol.velocity = Vector3.zero;
                bcol.AddForce(lDirection, ForceMode.VelocityChange);
                flowingmode = 0;
                flowing = true;
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log("c");
                // curve ball
                dir = new Vector3(19.0f, 0.0f, 0.0f);
                //bsize.transform.position = Vector3.Slerp(bsize.transform.position, new Vector3 (19.0f, 0.0f, 0.0f), 0.1f);
                bcol.AddForce(dir, ForceMode.VelocityChange);
                flowingmode = 1;
                flowing = true;
            }
            else
            {
                Debug.Log("sticky");
                // replace ball on player
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
            Debug.Log("TOUCHEE--------------------------------------------------------");
        }/*
         else if (col.gameObject.name == "Goal")
         {
             // win
         }*/
    }
}
