using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private float _speed = 2.0f;
    [SerializeField] private Player _player;
    [SerializeField] private Animator _enemy_animator;
    [SerializeField] private Collider2D _enemy_collider;
    [SerializeField] private Audio_Manager _audio_manager;
    [SerializeField] private GameObject _enemy_laser_prefab;

    void Start() {
        float random_x = Random.Range(-9.2f, 9.2f);
        transform.position = new Vector3(random_x, 7.0f, 0);
        _player = GameObject.Find("Player").GetComponent<Player>();
        _enemy_animator = GetComponent<Animator>();
        _enemy_collider = GetComponent<Collider2D>();   
        _audio_manager = GameObject.Find("Audio_Manager").GetComponent<Audio_Manager>();        

        if (_audio_manager == null) Debug.LogError("The audio manager wasn't found by the Enemy script.");
        if (_player == null) Debug.LogError("The Player was not found by the Enemy Script.");
        if (_enemy_animator == null) Debug.LogError("The animator was not found by the Enemy Script.");
        if (_enemy_collider == null) Debug.LogError("The Collider wasn't found by the Enemy Script.");
        if (_enemy_laser_prefab == null) Debug.LogError("The Enemy Laser Prefab wasn't found by the Enemy Script.");

        StartCoroutine(Fire_Laser());
    }

    void Update() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        //Spawn the same object on top when it goes out of bounds.
        if (transform.position.y <= -7.0f) {
            _player.Change_Score(false);
            float random_x = Random.Range(-9.2f, 9.2f);
            transform.position = new Vector3(random_x, 7.0f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //Collision with game objects destroys the game object as well as the enemy.
        
        if (other.tag == "Laser") {
            _player.Change_Score(true);
            Destroy(other.gameObject);
            _speed = 0;
            _enemy_animator.SetTrigger("Enemy_Destroyed");
            _enemy_collider.enabled = false;
            StopAllCoroutines();    
            _audio_manager.Play_Explosion_Sound();
            Destroy(this.gameObject, 2.4f);
        }   

        else if (other.tag == "Player") {
            _player.Damage_Player(this.gameObject);
            _speed = 0;
            _enemy_animator.SetTrigger("Enemy_Destroyed");
            _enemy_collider.enabled = false;
            StopAllCoroutines();
            _audio_manager.Play_Explosion_Sound();
            Destroy(this.gameObject, 2.4f);
        }
    }

    IEnumerator Fire_Laser() {
        while (_enemy_collider.enabled) {
            yield return new WaitForSeconds(3.0f);
            Vector3 laser_start = transform.position + new Vector3(0, -1.8f, 0); //The starting position for the laser relative to the player.
            Instantiate(_enemy_laser_prefab, laser_start, Quaternion.identity);
        }
    }
}