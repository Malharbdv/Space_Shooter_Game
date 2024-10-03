using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shield : MonoBehaviour {
    // private Transform player;
    
    void Start() {
        // player = GameObject.Find("Player").GetComponent<Transform>();
    }
    void Update() {
        if (this.transform.parent != null) 
            transform.position = this.transform.parent.position;
        else Debug.LogError("The player was not found by the Player Shield.");
    }
}
