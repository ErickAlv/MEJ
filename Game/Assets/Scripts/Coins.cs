using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour, ITR
{
    public float speed;
    private TimeReverse trscript;

    //Class to save the information for time reversing
    private class MyStatus : TRObject
    {
        //public Vector3 myPosition;
        public Quaternion myRotation;
        public bool rendenabled;
        public bool colenabled;
    }

    #region ITR implementation
    public void SaveTRObject()
    {
        MyStatus status = new MyStatus();
        //status.myPosition = transform.position;
        status.myRotation = transform.rotation;
        status.rendenabled = gameObject.GetComponent<Renderer>().enabled;
        status.colenabled = gameObject.GetComponent<Collider>().enabled;
        trscript.PushTRObject(status);
    }
    public void LoadTRObject(TRObject trobject)
    {
        MyStatus newStatus = (MyStatus)trobject;
        //transform.position = newStatus.myPosition;
        transform.rotation = newStatus.myRotation;
        gameObject.GetComponent<Renderer>().enabled = newStatus.rendenabled;
        gameObject.GetComponent<Collider>().enabled = newStatus.colenabled;
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        trscript = GetComponent<TimeReverse>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (GameData.Instance.Paused && gameObject.GetComponent<TimeReverse>() != null)
            return;*/
        transform.Rotate(Vector3.left, speed * Time.deltaTime, Space.Self);
    }
    void FixedUpdate()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameData.Instance.Score += 20;
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}
