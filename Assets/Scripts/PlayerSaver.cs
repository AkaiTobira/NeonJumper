using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaver : MonoBehaviour
{
    const float SAVE_BOTTOM_BOUNDARY = -20;

    void FixedUpdate()
    {
        if( transform.position.y < SAVE_BOTTOM_BOUNDARY){
                GameObject.Find("/SceneController").
                GetComponent<GameFlowController>().GameOver();
        }
    }
}
