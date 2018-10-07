using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public GameObject Player;
    public float startX;
    public float startY;
    public float startZ;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPos = Player.transform.position;
        newPos.x -= startX;
        newPos.y -= startY;
        newPos.z -= startZ;
        transform.position = newPos;
	}
}
