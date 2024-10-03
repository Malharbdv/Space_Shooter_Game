using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {
    [SerializeField] private float _rotate_speed = 20;
    [SerializeField] private GameObject _explosion_prefab;
    [SerializeField] private Spawn_Manager _spawn_Manager;
    [SerializeField] private Audio_Manager _audio_manager;

    void Start() {
        transform.position = new Vector3(0, 3.5f, 0);
        _spawn_Manager = GameObject.Find("Spawn_Manager").GetComponent<Spawn_Manager>();
        _audio_manager = GameObject.Find("Audio_Manager").GetComponent<Audio_Manager>();

        if (_spawn_Manager == null) Debug.LogError("The Spawn Manager wasn't found by the Asteroid script.");
        if (_audio_manager == null) Debug.LogError("The Audio Manager wasn't found by the Asteroid script.");
    }

    void Update() {
        transform.Rotate(Vector3.forward * _rotate_speed * Time.deltaTime);         
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Laser") {
            Destroy(other.gameObject);
            Instantiate(_explosion_prefab, transform.position, Quaternion.identity);
            _audio_manager.Play_Explosion_Sound();
            _spawn_Manager.Start_Spawning();
            Destroy(this.gameObject, 0.25f);
        }
    }
}
