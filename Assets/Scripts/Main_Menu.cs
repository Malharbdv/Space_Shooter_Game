using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour {

    public void Load_Game() {
        SceneManager.LoadScene(1);  //The game scene has number 1 in build settings.
    }
}
