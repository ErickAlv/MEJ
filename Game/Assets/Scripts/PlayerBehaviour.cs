using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody))]

public class PlayerBehaviour : MonoBehaviour, ITR {
    #region variables
    public MoveSettings moveSettings;
    public InputSettings inputSettings;
    private Rigidbody playerRigidbody;
    private Vector3 velocity;
    private Quaternion targetRotation, targetRotationY;
    private float forwardInput, sidewaysInput, turnInput, jumpInput, turnY;
    public GameObject plat;
    public Transform spawn;
    public static Text playerStats;
    private TimeReverse trscript;
    #endregion

    [System.Serializable]
    public class MoveSettings
    {
        public float runVelocity = 12;
        public float rotateVelocity = 500;
        public float jumpVelocity = 8;
        public float distanceToGround = 1.3f;
        public LayerMask ground;
    }

    [System.Serializable]
    public class InputSettings
    {
        public string FORWARD_AXIS = "Vertical";
        public string SIDEWAYS_AXIS = "Horizontal";
        public string TURN_AXIS = "Mouse X";
        public string JUMP_AXIS = "Jump";
        public string TURN_AXIS_Y = "Mouse Y";
    }

    private class MyStatus : TRObject
    {
        public Vector3 myPosition;
        public Quaternion myRotation;
    }

    #region ITR implementation
    public void SaveTRObject()
    {
        MyStatus status = new MyStatus();
        status.myPosition = transform.position;
        status.myRotation = transform.rotation;
        trscript.PushTRObject(status);
        playerRigidbody.isKinematic = false;
    }
    public void LoadTRObject(TRObject trobject)
    {
        MyStatus newStatus = (MyStatus)trobject;
        transform.position = newStatus.myPosition;
        transform.rotation = newStatus.myRotation;
        playerRigidbody.isKinematic = true;
    }
    #endregion

    //set all the start values
    void Awake()
    {
        GameData.Instance.Lives += 5;
        velocity = Vector3.zero;
        forwardInput = sidewaysInput = turnInput = jumpInput = 0;
        targetRotation = transform.rotation;
        targetRotationY = transform.GetChild(0).rotation;
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    public void Start()
    {
        trscript = GetComponent<TimeReverse>();
        playerStats = GameObject.Find("PlayerStats").GetComponent<Text>();
        UpdateStats();
        Spawn();
    }

    //call GetInput() and Turn()
    void Update()
    {
        if (GameData.Instance.Paused && gameObject.GetComponent<TimeReverse>() != null)
            return;
        GetInput();
        Turn();
        UpdateStats();
        Place();
    }

    //call Run() ans Jump()
    void FixedUpdate()
    {
        if (GameData.Instance.Paused && gameObject.GetComponent<TimeReverse>() != null)
            return;
        if (!playerRigidbody.isKinematic)
        {
            Run();
            Jump();
        }
    }
    
    
  


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "DeathZone")
        {
            velocity = Vector3.zero;
            Spawn();
        }else if(other.tag == "CheckPoint")//para prevenir la colision hay que usar Layer Collision Mark
        {
            spawn = other.transform;
        }
    }

    void OnDeath()
    {
        Spawn();
    }

    //checke the collision with the enemy
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // Diese Enemy ist eigentlich eine Referenz auf den Script "Enemy"
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            Collider col = other.gameObject.GetComponent<Collider>();
            Collider mycol = this.gameObject.GetComponent<Collider>();
            if (enemy.invincible)
            {
                GameData.Instance.Lives -= 1;
                OnDeath();
            }
            //untere Kante des Players - obere Kante bomb > 0 ==> Player jumped on the bomb
            //extends id die Hoehe von Miite bis zum Rand
            //Eine Colllision tritt erts vor wenn ein Objekt den Collider von einen andere Objekt überlappeb har; deswegen 0.5f*
            else if (mycol.bounds.center.y - mycol.bounds.extents.y > col.bounds.center.y + 0.5f * col.bounds.extents.y)
            {
                enemy.OnDeath();
                // += ist gleichzeitig ein get und set der Klasse GameData
                GameData.Instance.Score += 10;
                JumpedOnEnemy(enemy.bumpSpeed);
                
            }
            else
            {
                GameData.Instance.Lives -= 1;
                OnDeath();
            }
        }
    }

    

    //save user Input for moving, turning and jumping
    void GetInput()
    {
        if (inputSettings.FORWARD_AXIS.Length != 0)
            forwardInput = Input.GetAxis(inputSettings.FORWARD_AXIS);
        if (inputSettings.SIDEWAYS_AXIS.Length != 0)
            sidewaysInput = Input.GetAxis(inputSettings.SIDEWAYS_AXIS);
        if (inputSettings.TURN_AXIS.Length != 0)
            turnInput = Input.GetAxis(inputSettings.TURN_AXIS);
        if (inputSettings.TURN_AXIS_Y.Length != 0)
            turnY = Input.GetAxis(inputSettings.TURN_AXIS_Y);
        if (inputSettings.JUMP_AXIS.Length != 0)
            jumpInput = Input.GetAxisRaw(inputSettings.JUMP_AXIS);
    }

    //changes hte Rigidbody's x- and z-velocity depending on the input
    void Run()
    {
        velocity.z = forwardInput * moveSettings.runVelocity;
        velocity.x = sidewaysInput * moveSettings.runVelocity;
        velocity.y = playerRigidbody.velocity.y;
        playerRigidbody.velocity = transform.TransformDirection(velocity);
    }

    //change the Transform's rotatioin depending on the input
    void Turn()
    {
        if (Mathf.Abs(turnInput) > 0)
        {
            targetRotation *= Quaternion.AngleAxis(moveSettings.rotateVelocity *
            turnInput * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRotation;
        if (Mathf.Abs(turnY) > 0)
        {
            targetRotationY *= Quaternion.AngleAxis(moveSettings.rotateVelocity *
            turnInput * Time.deltaTime, Vector3.up);
            targetRotationY *= Quaternion.AngleAxis(moveSettings.rotateVelocity *
            turnY * Time.deltaTime, Vector3.left);
        }
        transform.GetChild(0).rotation = targetRotationY;
    }

    //add force to the Rigidbody depending on the input and on whether the GameObject is currently grounded
    void Jump()
    {
        if (jumpInput != 0 && Grounded())
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x,
            moveSettings.jumpVelocity, velocity.z);
        }
    }

    //checks if the character is on the ground
    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, moveSettings.distanceToGround, moveSettings.ground);
    }

    public void Spawn()
    {
        transform.position = spawn.position;
    }

    //place platforms
    void Place()
    {
        if (Input.GetKeyDown("q"))
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y - 1.5f,
                transform.position.z) + (transform.forward * 2);
            Instantiate(plat, pos, Quaternion.identity);
        }
    }

    void JumpedOnEnemy(float bumpSpeed)
    {
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, bumpSpeed, playerRigidbody.velocity.z);
    }

    //writes players Info
    public static void UpdateStats()
    {
        playerStats.text = "Score: " + GameData.Instance.Score
        + "\nLives: " + GameData.Instance.Lives;
    }

    public void HealPlayer()
    {

    }

    public void HurtPlayer()
    {

    }

}
