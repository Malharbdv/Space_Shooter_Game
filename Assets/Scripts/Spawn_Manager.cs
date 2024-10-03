using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Manager : MonoBehaviour {
    private GameObject enemy_prefab; 
    [SerializeField] private float _enemy_spawn_time = 1.5f; //In how much time a new enemy spawns?
    [SerializeField] private GameObject _Enemy_Container; //Container for all the enemies.
    
    private bool player_alive = true;

    [SerializeField] private GameObject[] _powerup_prefabs;
    private int _number_of_powerups = 3;     //This is the number of powerups in the game right now. Needs to be updated whenever adding a powerup.
    [SerializeField] private float _powerup_spawn_time; //In how much time a new powerup spawns?

    void Start() {
        enemy_prefab = Resources.Load<GameObject>("Enemy");

        if (enemy_prefab == null) 
            Debug.LogError("The enemy prefab was not found in the Resources folder.");
    }

    public void Start_Spawning() {
        StartCoroutine(Spawn_Enemy());
        StartCoroutine(Spawn_Powerup());
    }

    IEnumerator Spawn_Enemy() {
        while (player_alive) {
            yield return new WaitForSeconds(_enemy_spawn_time);
            GameObject new_enemy = Instantiate(enemy_prefab, Vector3.zero, Quaternion.identity);
            new_enemy.transform.parent = _Enemy_Container.transform; 
        }
    }

    IEnumerator Spawn_Powerup() {
        while(player_alive) {
            GameObject random_powerup_prefab = _powerup_prefabs[Random.Range(0, _number_of_powerups)];
            /*  index of "Triple_Shot_Powerup" is 0
                index of "Speed_Powerup" is 1
                index of "Shield_Powerup" is 2. */

            if (random_powerup_prefab == null) 
                Debug.LogError("The powerup prefab has not been assigned yet!");

            _powerup_spawn_time = Random.Range(10, 16); 
            yield return new WaitForSeconds(_powerup_spawn_time);
            GameObject new_powerup = Instantiate(random_powerup_prefab, Vector3.zero, Quaternion.identity);
            new_powerup.transform.parent = this.gameObject.transform; 
        }
    }

    public void Player_Died() {
        player_alive = false;
        Destroy(this.gameObject);
    }
}