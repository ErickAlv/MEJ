using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatFall : MonoBehaviour, ITR {

    public Rigidbody rb;
    void Start()
    {
        trscript = GetComponent<TimeReverse>();
        rb = GetComponent<Rigidbody>();
    }
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

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(Destroy());
        }
    }
    
    IEnumerator Destroy()
    {
        yield return (new WaitForSeconds(0.2f));
        rb.isKinematic = false;
        rb.useGravity = true;
        yield return (new WaitForSeconds(2));
        Destroy(gameObject);
        //rigid.useGravity = true;
    }

}
