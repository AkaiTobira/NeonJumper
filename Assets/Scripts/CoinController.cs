using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if( other.tag == "Player"){
            GameObject.Find("/SceneController").
            GetComponent<GameFlowController>().AddToScore(1);
            Destroy(gameObject);
        }
    }

}
