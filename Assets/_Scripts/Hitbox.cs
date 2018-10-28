using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class Hitbox : MonoBehaviour
{

    static public Hitbox H;

    public GameObject Player;
    public float startX;

    public float startY;

    public float startZ;
    public float gameRestartDelay = 2f;

    [SerializeField]
    public float _shieldLevel = 1;
    private GameObject lastTriggerGo = null;

    public float shieldLevel
    {
	get
	{
	    return _shieldLevel;
	}
	set
	{
	    _shieldLevel = Mathf.Min(value, 4);

	    if(value < 0)
	    {
		Destroy(this.gameObject);
		Destroy(Player.gameObject);

		Main.S.DelayRestart(gameRestartDelay);
	    }
	}
    }

    void Awake()
    {

	if(H == null)

        {
            H = this;
        }

    }


    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPos = Player.transform.position;
        newPos.x -= startX;
        newPos.y -= startY;
        newPos.z -= startZ;
        transform.position = newPos;

    }

    void OnTriggerEnter(Collider other)
    {
	Transform rootT = other.gameObject.transform.root;
	GameObject go = rootT.gameObject;
	print("Triggered: " + go.gameObject.name);

	if(go == lastTriggerGo)
	{
	    return;
	}

	lastTriggerGo = go;

	if(go.tag == "Enemy" || go.tag == "ProjectileEnemy")
	{
	    shieldLevel--;
//	    Destroy(go);
	}
	else
	{
	    print("Triggered by non-enemy: " + go.name);
	}
    }
}
