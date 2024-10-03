using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private int _powerup_id;

    /* The Powerup ID allows the player to distinguish between the three different powerups.
    I didn't want to use strings everywhere because it gets not only bulky but also inefficient passing this gameobject to the function.
    powerup_id of "Triple_Shot_Powerup" is 0
    powerup_id of "Speed_Powerup" is 1
    powerup_id of "Shield_Powerup" is 2. */

    void Start() {
        float random_x = Random.Range(-9.2f, 9.2f);
        transform.position = new Vector3(random_x, 7.0f, 0);
    }

    void Update() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7.0f) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Player player = other.transform.GetComponent<Player>();

            if  (player != null) { player.Activate_Powerup(_powerup_id); }
            else    { Debug.LogError("The Player wasn't found by the powerup prefab."); }
            Destroy(this.gameObject);
        }
    }
}
