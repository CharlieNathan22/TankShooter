using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelOnClick : MonoBehaviour
{
    //Loads a scene when a button is clicked
    public void loadLevel(int trackIndex)
    {
        SceneManager.LoadScene(trackIndex);
    }
}
