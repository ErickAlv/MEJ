using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible : MonoBehaviour {

    public Renderer rend;
    [SerializeField]
    private float timeOn;
    [SerializeField]
    private float timeOff;

    public bool Hide = false;
    //private Vector3 velocity;


    private void Start()
    {
        rend = GetComponent<Renderer>();
        StartCoroutine("HideUnhide");
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("Player"))
        {
           //GetComponent<Collider>().isTrigger = true;
        }
    }

    IEnumerator HideUnhide()
    {
        while (true)
        {
            yield return (new WaitForSeconds(timeOff));
            gameObject.layer = 0;
            GetComponent<Collider>().isTrigger = false;
            rend.enabled = true;

            yield return (new WaitForSeconds(timeOn));
            gameObject.layer = 2;
            rend.enabled = false;
            GetComponent<Collider>().isTrigger = true;
        }
    }
}
