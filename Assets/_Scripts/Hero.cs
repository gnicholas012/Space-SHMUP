using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class Hero : MonoBehaviour
{
    static public Hero S;



    [Header("Set in Inspector")]

    public float speed = 30;

    public float speedFocus = 10;

    public float rollMult = -45;

    public float pitchMult = 30;

    public float radius = 1f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    public Weapon[] weapons;
    public Weapon[] weaponsF;



    [Header("Set Dynamically")]

    public bool isFocusMode = false;

    public int graze = 0;
    public float camWidth;

    public float camHeight;
    public int weaponCounter = 0;

    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;

    Hitbox A;

/*    [SerializeField]
    public float _shieldLevel;

    private GameObject lastTriggerGo = null;
*/

/*    public float shieldLevel
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
	    }
	}
    }
*/
    // Use this for initialization
    void Start()
    {

	if(S == null)

        {
            S = this;
        }

        else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S");
        }

	A = GameObject.Find("Hitbox").GetComponent<Hitbox>();

	//fireDelegate += TempFire;

        camHeight = Camera.main.orthographicSize;

        camWidth = camHeight * Camera.main.aspect;

	ClearWeapons();
	weapons[0].SetType(WeaponType.blaster);
    }

	
	// Update is called once per frame

    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");

        float yAxis = Input.GetAxis("Vertical");


        if (Input.GetKeyDown("right shift") == true || Input.GetKeyDown("left shift") == true)
        {
            isFocusMode = true;

	    for(int i = 0; i <= weaponCounter; i++)
	    {
		weapons[i].SetActive(false);
		weaponsF[i].SetActive(true);
	    }
        }

        if (Input.GetKeyUp("right shift") == true || Input.GetKeyUp("left shift") == true)
        {
            isFocusMode = false;

	    for(int i = 0; i <= weaponCounter; i++)
	    {
		weaponsF[i].SetActive(false);
		weapons[i].SetActive(true);
	    }
        }



        Vector3 pos = transform.position;


        if(isFocusMode == true)
        {
            pos.x += xAxis * speedFocus * Time.deltaTime;
            pos.y += yAxis * speedFocus * Time.deltaTime;
        }

        else
        {
            pos.x += xAxis * speed * Time.deltaTime;
            pos.y += yAxis * speed * Time.deltaTime;
        }



        if (pos.x > camWidth + radius)

        {
            pos.x = -camWidth - radius + 1;
        }

        if (pos.x < -camWidth - radius)

        {
            pos.x = camWidth + radius - 1;
        }
        if (pos.y > camHeight - radius)
        {
            pos.y = camHeight - radius;
        }
        if (pos.y < -camHeight + radius)
        {
            pos.y = -camHeight + radius;
        }


        transform.position = pos;


        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);


/*	if(Input.GetKeyDown(KeyCode.Space))
	{
	    TempFire();
	}
*/

	if(Input.GetAxis("Jump") == 1 && fireDelegate != null)
	{
	    fireDelegate();
	}

    }

    void OnTriggerEnter(Collider other)
    {
	Transform rootT = other.gameObject.transform.root;
	GameObject go = rootT.gameObject;

	if(go.tag == "Enemy" || go.tag == "ProjectileEnemy")
	{
	    graze += 1;
	}
	else
	{
	    if(go.tag == "PowerUp")
	    {
		AbsorbPowerUp(go);
	    }
	}
    }
    void TempFire()
    {
	GameObject projGO = Instantiate<GameObject>(projectilePrefab);
	projGO.transform.position = transform.position;
	Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
//	rigidB.velocity = Vector3.up * projectileSpeed;

	ProjectileHero proj = projGO.GetComponent<ProjectileHero>();
	proj.type = WeaponType.blaster;
	float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
	rigidB.velocity = Vector3.up * tSpeed;
    }
    public void AbsorbPowerUp(GameObject go)
    {
	PowerUp pu = go.GetComponent<PowerUp>();
	
	switch(pu.type)
	{
	    case WeaponType.shield:
		A.shieldLevel++;
		break;

	    default:
		if(pu.type == weapons[0].type)
		{
		    Weapon w = GetEmptyWeaponSlot();
		    Weapon wf = GetEmptyWeaponSlotF();

		    if(w != null)
		    {
			w.SetType(pu.type);
			wf.SetType(pu.type);
			weaponCounter++;

			if(isFocusMode == true)
			{
			    weapons[weaponCounter].SetActive(false);
			}
			if(isFocusMode == false)
			{
			    weaponsF[weaponCounter].SetActive(false);
			}
		    }
		}
		else
		{
		    ClearWeapons();
		    weapons[0].SetType(pu.type);
		    weaponCounter = 0;
		}
		break;
	}
	pu.AbsorbedBy(this.gameObject);
    }
    Weapon GetEmptyWeaponSlot()
    {
	for(int i = 0; i < weapons.Length; i++)
	{
	    if(weapons[i].type == WeaponType.none)
	    {
		return (weapons[i]);
	    }
	}
	return (null);
    }
    Weapon GetEmptyWeaponSlotF()
    {
	for(int i = 0; i < weaponsF.Length; i++)
	{
	    if(weaponsF[i].type == WeaponType.none)
	    {
		return (weaponsF[i]);
	    }
	}
	return (null);
    }
    void ClearWeapons()
    {
	foreach(Weapon w in weapons)
	{
	    w.SetType(WeaponType.none);
	}
	foreach(Weapon w in weaponsF)
	{
	    w.SetType(WeaponType.none);
	}
    }
}