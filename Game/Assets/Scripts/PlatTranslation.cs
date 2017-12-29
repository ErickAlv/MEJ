using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatTranslation : MonoBehaviour, ITR
{

    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private float maxDist;
    [SerializeField]
    private int moveX;
    [SerializeField]
    private int moveY;
    [SerializeField]
    private int moveZ;
    private float originX;
    private float originY;
    private float originZ;
    private float amtToMove;
    private TimeReverse trscript;

    //Class to save the information for time reversing
    private class MyStatus : TRObject
    {
        public Vector3 myPosition;
    }

    #region ITR implementation
    public void SaveTRObject()
    {
        MyStatus status = new MyStatus();
        status.myPosition = transform.position;
        trscript.PushTRObject(status);

    }
    public void LoadTRObject(TRObject trobject)
    {
        MyStatus newStatus = (MyStatus)trobject;
        transform.position = newStatus.myPosition;
    }
    #endregion

    void Awake()
    {
        originX = transform.position.x;
        originY = transform.position.y;
        originZ = transform.position.z;
    }


    // Use this for initialization
    void Start()
    {
        trscript = GetComponent<TimeReverse>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameData.Instance.Paused && gameObject.GetComponent<TimeReverse>() != null)
            return;
        amtToMove = speed * Time.deltaTime;
        if (moveZ > 0)
        {

            transform.Translate(Vector3.forward * amtToMove, Space.World);
            if (transform.position.z >= originZ + maxDist)
            {
                moveZ *= -1;
            }
        }
        if (moveZ < 0)
        {
            transform.Translate(Vector3.back * amtToMove, Space.World);
            if (transform.position.z <= originZ - maxDist)
            {
                moveZ *= -1;
            }
        }
        if (moveX > 0)
        {

            transform.Translate(Vector3.right * amtToMove, Space.World);
            if (transform.position.x >= originX + maxDist)
            {
                moveX *= -1;
            }
        }
        if (moveX < 0)
        {
            transform.Translate(Vector3.left * amtToMove, Space.World);
            if (transform.position.x <= originX - maxDist)
            {
                moveX *= -1;
            }
        }
        if (moveY > 0)
        {

            transform.Translate(Vector3.up * amtToMove, Space.World);
            if (transform.position.y >= originY + maxDist)
            {
                moveY *= -1;
            }
        }
        if (moveY < 0)
        {
            transform.Translate(Vector3.down * amtToMove, Space.World);
            if (transform.position.y <= originY - maxDist)
            {
                moveY *= -1;
            }
        }
    }

    /*void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.collider.transform.SetParent(transform);
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.collider.transform.SetParent(null);
        }
    }*/
}
