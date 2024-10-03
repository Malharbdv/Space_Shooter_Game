using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Laser : MonoBehaviour {
    [SerializeField] private float _speed = 8.0f;
    [SerializeField] private Player _player;

    void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);       

        if (transform.position.y < -6.8f) {
            Destroy(transform.parent?.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            _player.Damage_Player(this.gameObject);
            _speed = 0;
            Destroy(this.gameObject);
        }
    }
}
