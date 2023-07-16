using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public void LoadLevel(string levelName){
        StartCoroutine(LoadLevelTransition(levelName));
    }

    public IEnumerator LoadLevelTransition(string levelName){
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(levelName);
    }
}
