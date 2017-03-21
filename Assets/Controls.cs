using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {

    public Rigidbody pcol;
    public Rigidbody bcol;
    public Transform psize;
    public Transform gsize;
    public int zone; // 1 = right / 2 = up / 3 = left / 4 = down

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
        float speed = 0.5f;
        float rotationspeed = 1.0f;
        Vector3 readjusted = psize.position;
        Vector3 rot = psize.transform.rotation.eulerAngles;
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
                    zone = 2;
                }
            }
            else
            {
                if ((posy == maxy) && (posx != -maxx))
                {
                    if (posx + (moveV * speed) > -maxx)
                    {
                        //  right lane
                        readjusted = new Vector3(posx - (moveV * speed), 0.0f, posy);
                        zone = 2;
                    }
                    else
                    {
                        // down corner
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
                        zone = 3;
                    }
                    else
                    {
                        //left corner
                        readjusted = new Vector3(posx + (moveV * speed), 0.0f, -maxy);
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
                        zone = 1;
                    }

                }
            }
        }
        

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rot = new Vector3(0, -rotationspeed, 0);
            psize.transform.Rotate(rot);

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rot = new Vector3(0, rotationspeed, 0);
            psize.transform.Rotate(rot);
        }
        if ((zone == 1) && (rot.y > -75) && (rot.y < 75))
        {
          //  Debug.Log("zone : " + zone + " rot : " + rot + " rotp : " + psize.rotation.y);
            rot = new Vector3(0.0f, 0.0f, 0.0f);
          //  Debug.Log("zone : " + zone + " rot : " + rot + " rotp : " + psize.rotation.y);
            psize.transform.Rotate(rot);
        }
        if ((zone == 2) && (rot.y > -35) && (rot.y < -145))
        {
          //  Debug.Log("zone : " + zone + " rot : " + rot + " rotp : " + psize.rotation.y);
            rot = new Vector3(0.0f, -90f, 0.0f);
           // Debug.Log("zone : " + zone + " rot : " + rot + " rotp : " + psize.rotation.y);
            psize.transform.Rotate(rot);
        }
        if ((zone == 3) && (rot.y > 130) && (rot.y < 245))
        {
        //    Debug.Log("zone : " + zone + " rot : " + rot + " rotp : " + psize.rotation.y);
            rot = new Vector3(0.0f, -180f, 0.0f);
           // Debug.Log("zone : " + zone + " rot : " + rot + " rotp : " + psize.rotation.y);
            psize.transform.Rotate(rot);
        }
        if ((zone == 4) && (rot.y < 145) && (rot.y > 30))
        {
            rot = new Vector3(0.0f, 90f, 0.0f);
          //  Debug.Log("zone : " + zone + " rot : " + rot);
            psize.transform.Rotate(rot);
        }
        /* else
         {
             rot = Vector3.zero;
         }*/
        psize.transform.position = readjusted;
    }
}
