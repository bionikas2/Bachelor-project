using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{

    public Animator animator;

    // Update is called once per frame
    private int levelToLoad;
    
    public void play()
    {
        FadeToLevel(1);
        GameController.Health = 6;
        GameController.BulletSize = 0.5f;
        GameController.FireRate = 0.5f;
        GameController.MoveSpeed = 5f;

        FindObjectOfType<AudioManager>().Play("meniu");
         
    }

    public void meniu()
    {
        FindObjectOfType<AudioManager>().Play("meniu");
        FadeToLevel(0);

    }

    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("Fade_Out");

    }

    public void OnFadeComplete()
    {
        if(levelToLoad == 1)
        SceneManager.LoadScene(1);
        
        if(levelToLoad == 0)
        SceneManager.LoadScene(0);

    }

    public void howtoPlay()
    {
        FindObjectOfType<AudioManager>().Play("meniu");
    }

    public void quit()
    {
        FindObjectOfType<AudioManager>().Play("meniu");
        FadeToLevel(0);
        Application.Quit();

    }
    
}
