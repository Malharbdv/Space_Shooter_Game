using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour {
    [SerializeField] private Text _score_text;
    [SerializeField] private Text _game_over_text;
    [SerializeField] private Text _restart_prompt_text;
    [SerializeField] private Game_Manager _game_manager;
    [SerializeField] private float _game_over_flicker_timer = 0.5f;

    [SerializeField] private Image _lives_visualizer_image;
    [SerializeField] private Player player;
    [SerializeField] private Sprite[] _player_lives_sprites;

    void Start() {
        player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null) {
            Debug.LogError("The Player was not found by the UI manager.");
        }

        _game_manager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        if (_game_manager == null) {
            Debug.LogError("The Game Manager Gameobject wasn't found by the UI manager.");
        }

        _restart_prompt_text.gameObject.SetActive(false);
        if (_game_over_text != null) 
            _game_over_text.gameObject.SetActive(false);
        else 
            Debug.LogError("Game Over Text not assigned in the UI manager.");
    }

    public void Update_Score(int current_player_score) {
        _score_text.text = "Score: " + current_player_score;
    }

    public void Update_Lives_Visualizer(int remaining_player_lives) {
        _lives_visualizer_image.sprite   = _player_lives_sprites[remaining_player_lives];
    }

    public void Game_Over_Sequence() {
        _restart_prompt_text.gameObject.SetActive(true);
        _game_manager.Game_Over();
        StartCoroutine(Game_Over_Text_Flicker());
    }

    IEnumerator Game_Over_Text_Flicker() {
        while (true) {
            _game_over_text.gameObject.SetActive(true);
            yield return new WaitForSeconds(_game_over_flicker_timer);
            _game_over_text.gameObject.SetActive(false);
            yield return new WaitForSeconds(_game_over_flicker_timer);
        }
    }   
}
