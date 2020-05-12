using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameFlowController : MonoBehaviour
{
    [SerializeField] PlayerController player                  = null;
    [SerializeField] Transform        HUD_PlayerLifes         = null;
    [SerializeField] Text             HUD_ScoreDisplayer      = null;
    [SerializeField] Text             endWindowScoreDisplayer = null;
    [SerializeField] Transform        endWindow               = null;

    private int MAX_NUMBER_OF_LIFES = 5;
    private int numberOfLives = 5;
    private int score         = 0;

    void Start() {
        SetScoreToHUD();
        UpdatePlayerLifesHUD();
    }

    private void GameOver(){
        endWindow.GetComponent<Animation>().Play();
        player.DeletePlayer();
        numberOfLives = 0;
        UpdatePlayerLifesHUD();
    }

    private void UpdatePlayerLifesHUD(){
        for( int i = 0; i < MAX_NUMBER_OF_LIFES; i ++){
            HUD_PlayerLifes.GetChild(i).gameObject.SetActive( i < numberOfLives );
        }
    }

    private void SetScoreToHUD(){
        HUD_ScoreDisplayer.text      = "SCORE : " + score.ToString();
        endWindowScoreDisplayer.text = "SCORE : " + score.ToString();
    }

    public void ReducePlayerLife(){
        numberOfLives -= 1;
        UpdatePlayerLifesHUD();
        if( numberOfLives <= 0 ) {
            GameOver();
        }
    }

    public void AddToScore( int value ){
        score += value;
        SetScoreToHUD();
    }

    public void ReloadScenes(){
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
}
