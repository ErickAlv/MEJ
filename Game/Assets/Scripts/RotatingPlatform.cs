using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour, ITR {
    public float rotationSpeed;
    public float rotX;
    public float rotY;
    public float rotZ;

    private TimeReverse trscript;

    //Class to save th eonformation for time reversing
    private class MyStatus : TRObject
    {
        public Quaternion myRotation;
    }

    // Use this for initialization
    void Start () {
        trscript = GetComponent<TimeReverse>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.Instance.Paused && gameObject.GetComponent<TimeReverse>() != null)
            return;
        if (rotZ > 0)
        {
            transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime,
         Space.Self);
        }
        if(rotZ < 0)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime,
        Space.Self);
        }
        if (rotY > 0)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime,
        Space.Self);
        }
        if (rotY < 0)
        {
            transform.Rotate(Vector3.down, rotationSpeed * Time.deltaTime,
        Space.Self);
        }
        if (rotX > 0)
        {
            transform.Rotate(Vector3.left, rotationSpeed * Time.deltaTime,
        Space.Self);
        }
        if (rotX < 0)
        {
            transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime,
        Space.Self);
        }
    }

    public void SaveTRObject()
    {
        MyStatus status = new MyStatus();
        status.myRotation = transform.rotation;
        trscript.PushTRObject(status);
    }
    public void LoadTRObject(TRObject trobject)
    {
        MyStatus newStatus = (MyStatus)trobject;
        transform.rotation = newStatus.myRotation;
    }
}
