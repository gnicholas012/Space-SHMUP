using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class BoundsCheck : MonoBehaviour
{

    [Header("Set in Inspector")]

    public float radius = 1f;

    public bool keepOnScreen = true;



    [Header("Set Dynamically")]

    public bool isOnScreen = true;

    public float camWidth;

    public float camHeight;

    public Vector3 pos;



//    [HideInInspector]

    public bool offRight = false;

    public bool offLeft = false;

    public bool offUp = false;

    public bool offDown = false;


    void Awake()
    {
        camHeight = Camera.main.orthographicSize;

        camWidth = camHeight * Camera.main.aspect;

    }


	// Update is called once per frame

    void LateUpdate()
    {
	offRight = false;

	offLeft = false;

	offUp = false;

	offDown = false;


        pos = transform.position;

        isOnScreen = true;


        if (pos.x > camWidth + radius)

        {
            pos.x = camWidth + radius;
            isOnScreen = false;
            offRight = true;
        }

        if (pos.x < -camWidth - radius)

        {
            pos.x = -camWidth - radius;
            isOnScreen = false;
            offLeft = true;
        }
        if (pos.y > camHeight - radius)
        {
            pos.y = camHeight - radius;
            isOnScreen = false;
            offUp = true;
        }
        if (pos.y < -camHeight + radius)
        {
            pos.y = -camHeight + radius;
            isOnScreen = false;
            offDown = true;
        }

        if (keepOnScreen == true && isOnScreen == false)
        {
            transform.position = pos;
            isOnScreen = true;
            offRight = false;
            offLeft = false;
            offUp = false;
            offDown = false;
        }
    }


    private void OnDrawGizmos()
    {
        if(!Application.isPlaying)
        {
            return;
        }
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, .1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }

}
