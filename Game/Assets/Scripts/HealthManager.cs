using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {
    public int currentHealth = 5;
    public int maxHealth = 5;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HurtPlayer(int damage) {
        currentHealth -= damage;
    }

    public void HealPlayer(int healAmount) {
        currentHealth += healAmount;
        if(currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
    }
}
