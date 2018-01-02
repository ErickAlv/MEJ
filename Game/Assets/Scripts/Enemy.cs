using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour, ITR {

    public float speedX;
    public float speedZ;
    Rigidbody enemyRigidbody;
    public bool invincible;
    public float bumpSpeed;
    TimeReverse trscript;

    private class MyStatus : TRObject
    {
        public Vector3 myPosition;
        public Quaternion myRotation;
        public float mySpeedX;
        public float mySpeedZ;
        public float myBumpSpeed;
        public bool enabled;
        public bool invincible;
    }

    #region ITR implementation
    public void SaveTRObject()
    {
        MyStatus status = new MyStatus();
        status.myPosition = transform.position;
        status.myRotation = transform.rotation;
        status.myBumpSpeed = bumpSpeed;
        status.enabled = gameObject.GetComponent<Collider>().enabled;
        status.invincible = invincible;
        status.mySpeedX = speedX;
        status.mySpeedZ = speedZ;
        trscript.PushTRObject(status);
        enemyRigidbody.isKinematic = false;
    }
    public void LoadTRObject(TRObject trobject)
    {
        MyStatus newStatus = (MyStatus)trobject;
        transform.position = newStatus.myPosition;
        transform.rotation = newStatus.myRotation;
        bumpSpeed = newStatus.myBumpSpeed;
        gameObject.GetComponent<Collider>().enabled = newStatus.enabled;
        invincible = newStatus.invincible;
        speedX = newStatus.mySpeedX;
        speedZ = newStatus.mySpeedZ;
        // gravity should not influence the enemy while we reverse the time
        enemyRigidbody.isKinematic = true;
    }
    #endregion

    void Start()
    {
        trscript = GetComponent<TimeReverse>();
    }
    void Awake()
    {
        enemyRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (GameData.Instance.Paused && gameObject.GetComponent<TimeReverse>() != null)
            return;
        if (!enemyRigidbody.isKinematic)
        {
            enemyRigidbody.velocity = new Vector3(speedX, enemyRigidbody.velocity.y, speedZ);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "End")
        {
            speedX *= -1;
            speedZ *= -1;
        }
    }
    /*void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Box")
        {
            enemyRigidbody.velocity = new Vector3(0, enemyRigidbody.velocity.y, 0);
        }
    }*/

    public void OnDeath()
    {
        gameObject.GetComponent<Collider>().enabled = false;
    }
}
