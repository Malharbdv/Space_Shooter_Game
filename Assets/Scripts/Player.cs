using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float _speed = 5f;  //Speed of the player's movements.

    [SerializeField] private float _fire_rate = 0.15f;    //Cooldown time for the laser firing.
    private float _fire_time = -1f;     //Timer for when the next shot can be fired.
    [SerializeField] private GameObject _laser_prefab;   //Laser_prefab to be shot out by the player.

    //Explosion after the player dies. It will handle its own sound FX.
    [SerializeField] private GameObject _explosion_prefab;

    //Player Variables.
    [SerializeField] private int _player_lives;   //The leftover lives of the player at any point in the game.
    [SerializeField] private int _max_lives = 3;    //Number of lives player has at the start of the game.
    [SerializeField] private int _score = 0;
    [SerializeField] private bool _invincibility;
    [SerializeField] private float _invincibility_time = 1.0f;

    //Getting all the Scene Components.
    private Spawn_Manager _spawn_manager;
    private UI_Manager _ui_manager;
    private Audio_Manager _audio_manager;

    //Sprite Renderer for the player.
    private SpriteRenderer _sprite_renderer;

    //Engine damaging variables from here on out;
    [SerializeField] private GameObject _right_engine; 
    [SerializeField] private GameObject _left_engine;

    //Powerup variables to be used only for powerups from here on out:

    //Triple Laser Powerups Variables.
    [SerializeField] private bool _triple_shot_powerup_active = false;
    [SerializeField] private GameObject _triple_laser_prefab;

    //Speed Powerup Variables.
    [SerializeField] private bool _speed_powerup_active = false;
    [SerializeField] private float _speed_powerup_multiplier = 2.0f; 
    private GameObject _player_thruster;
    [SerializeField] private float _power_down_timer = 5.0f;

    //Shield Powerup Variables
    [SerializeField] private bool _shield_powerup_active = false;
    private GameObject _player_shield;

    void Start() {
        //Assign starting position (0, -4, 0)
        transform.position = new Vector3(0, -4, 0);
        
        _laser_prefab = Resources.Load<GameObject>("Laser");    //We really need to name this folder Resources, without which our prefabs don't work.

        _spawn_manager = GameObject.Find("Spawn_Manager").GetComponent<Spawn_Manager>();
        _ui_manager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        _audio_manager = GameObject.Find("Audio_Manager").GetComponent<Audio_Manager>();  

        _sprite_renderer = GetComponent<SpriteRenderer>();      
        
        _triple_laser_prefab = Resources.Load<GameObject>("Triple_Laser_Shot");
        _player_shield = GameObject.Find("Player_Shield");
        _player_thruster = GameObject.Find("Thruster");

        if (_laser_prefab == null) Debug.LogError("The Laser Prefab not found in Resources folder!");
        if (_spawn_manager == null) Debug.LogError("The Spawn Manager wasn't found!");  
        if (_ui_manager == null) Debug.LogError("The UI Manager wasn't found by the Player Script.");
        if (_triple_laser_prefab == null) Debug.LogError("The Triple Laser Shot Prefab not found in Resources folder!");
        if (_player_shield == null) Debug.LogError("The Player Shield wasn't found by the player.");
        if (_player_thruster == null) Debug.LogError("The Player Thruster Wasn't found by the player Script.");
        if (_explosion_prefab == null) Debug.LogError("The explosion prefab wasn't found by the Player script");
        if (_audio_manager == null) Debug.LogError("The Audio Manager wasn't found by the Player Script.");

        _player_thruster.SetActive(false);
        _player_shield.SetActive(false);
        _right_engine.SetActive(false);
        _left_engine.SetActive(false);
        _player_lives = _max_lives;
        _invincibility = false;
    }

    void Update() {
        Player_Movement();
        Shoot_Laser();
    }

    void Player_Movement() {
        //Converting input to the movement of the player.
        float horizontal_input = Input.GetAxis("Horizontal");
        float vertical_input = Input.GetAxis("Vertical");   

        Vector3 direction = new Vector3(horizontal_input, vertical_input, 0);

        if (_speed_powerup_active) 
            transform.Translate(direction * _speed * _speed_powerup_multiplier * Time.deltaTime);
        else 
            transform.Translate(direction * _speed * Time.deltaTime);
        //Time.deltaTime is basically conversion to real-time from the default 60fps time.
        
        float newX = Mathf.Repeat(transform.position.x + 11.3f, 22.6f) - 11.3f;
        float newY = Mathf.Clamp(transform.position.y, -5f, 5f);
          
        transform.position = new Vector3(newX, newY, 0);
    }

    void Shoot_Laser() {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _fire_time) {
            _fire_time = Time.time + _fire_rate;    //Updating the time for when the next shot can be fired.
            Vector3 laser_start = transform.position + new Vector3(0, 1, 0); //The starting position for the laser relative to the player.
            _audio_manager.Play_Laser_Sound();

            if (_triple_shot_powerup_active) {
                Instantiate(_triple_laser_prefab, laser_start, Quaternion.identity);
            } else {
                Instantiate(_laser_prefab, laser_start, Quaternion.identity); //Quaternion.identity basically means no rotation
            }
        }
    }

    void Manage_Player_Shield(bool activate) {
        _player_shield.SetActive(activate);
    }


    public void Damage_Player(GameObject enemy_object) {
        if (_invincibility == true) return;

        if (_shield_powerup_active) {
            _shield_powerup_active = false;
            Manage_Player_Shield(_shield_powerup_active);
            return;
        }
        _player_lives--;

        if (_player_lives < 0) 
            _player_lives = 0;

        switch (_player_lives) {
            case 1:
                Damage_Engine(float.PositiveInfinity);
                break;
            
            case 2:
                float enemy_position = enemy_object.transform.position.x;
                Damage_Engine(transform.position.x - enemy_position);
                break;
            
            default:
                break;    
            }

        _ui_manager.Update_Lives_Visualizer(_player_lives);

        if (_player_lives == 0) {
            _spawn_manager.Player_Died();
            Instantiate(_explosion_prefab, this.transform.position, Quaternion.identity);
            _audio_manager.Play_Explosion_Sound();
            _ui_manager.Game_Over_Sequence();
            Destroy(this.gameObject);
        }
        else {
            StartCoroutine(Player_Invincible());
        }
    }

    IEnumerator Player_Invincible() {
        _invincibility = true;
        StartCoroutine(Invincibility_Flicker());
        yield return new WaitForSeconds(_invincibility_time);
        StopCoroutine(Invincibility_Flicker());
        _invincibility = false;
    }

    IEnumerator Invincibility_Flicker() {
        while (_invincibility) {
            Color temp_colour = _sprite_renderer.color;
            temp_colour.a = 0.5f;
            _sprite_renderer.color = temp_colour;
            yield return new WaitForSeconds(0.2f);
            temp_colour = _sprite_renderer.color;
            temp_colour.a = 1f;
            _sprite_renderer.color = temp_colour;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void Damage_Engine(float engine_side) {
        if (engine_side == float.PositiveInfinity) {
            _right_engine.SetActive(true);
            _left_engine.SetActive(true);
        }

        else if (engine_side > 0)
            _left_engine.SetActive(true);

        else if (engine_side <= 0)  {
            _right_engine.SetActive(true);
        }
    }


    public void Activate_Powerup(int powerup_id) {
        _audio_manager.Play_Powerup_Sound();

        if (powerup_id == 0) {
            _triple_shot_powerup_active = true;
            StartCoroutine(Power_Down("Triple_Shot_Powerup"));
        }

        else if (powerup_id == 1) {
            _speed_powerup_active = true;
            _player_thruster.SetActive(true);
            StartCoroutine(Power_Down("Speed_Powerup"));
        }

        else if (powerup_id == 2) {
            _shield_powerup_active = true;
            Manage_Player_Shield(_shield_powerup_active);
        }
    }

    private IEnumerator Power_Down(string powerup_tag) {
        yield return new WaitForSeconds(_power_down_timer);

        //I'm gonna let this powerup_tag stay as it is since I don't need to change it often.
        //This will also help me understand which powerup id means which powerup without having to go to the powerup script. 
        if (powerup_tag == "Triple_Shot_Powerup") {     
            _triple_shot_powerup_active = false;
        }

        else if (powerup_tag == "Speed_Powerup") {
            _speed_powerup_active = false;
            _player_thruster.SetActive(false);
        }
    }

    public void Change_Score(bool increase) {
        
        if (increase) _score += 10;
        else _score -= 10;

        _ui_manager.Update_Score(_score);
    }
}
