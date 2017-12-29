using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys : MonoBehaviour {

    public GameObject pickUpKeys;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player") {
            GameData.Instance.Keys+=1;
            Instantiate(pickUpKeys,transform.position,transform.rotation);
            Destroy(gameObject);
        }
    }
}
