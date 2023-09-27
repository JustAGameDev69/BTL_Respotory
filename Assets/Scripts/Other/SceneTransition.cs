using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    //Scene Transition from Brackeys

    public int pauseTime = 1;

    [SerializeField] private Animator animator;


    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(pauseTime);


    }

}
