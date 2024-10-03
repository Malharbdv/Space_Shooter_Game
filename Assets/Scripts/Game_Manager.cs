using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// [System.Serializable]
public class Game_Manager : MonoBehaviour {
    private bool _game_over;

    void Start() {
        _game_over = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.R) && _game_over) {
            SceneManager.LoadScene(1); //The current scene named "Game" has the number 0
        }
        
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // Close the game
            Application.Quit();

            // If you are testing in the Unity Editor, this will stop the play mode
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }

    public void Game_Over() {
        _game_over = true;
    }
}
