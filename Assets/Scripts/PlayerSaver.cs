using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaver : MonoBehaviour
{
    CollisionDetectorPlayer m_controller;
    Queue<Vector3> m_queue = new Queue<Vector3>();

    Vector3 lastSavePosition;

    const float SAVE_BOTTOM_BOUNDARY = -20;
    const float SAVE_DELAY           = 10;

    void Start() {
        m_controller = GetComponent<CollisionDetectorPlayer>();
        lastSavePosition = transform.position;
        for( int i = 0; i < SAVE_DELAY; i++){
            m_queue.Enqueue(transform.position);
        }
    }

    private void SaveToQueue(){
        if( m_controller.isOnGround() ){
            lastSavePosition = m_queue.Dequeue();
            m_queue.Enqueue(transform.position);
        }
    }

    public void Save(){
        lastSavePosition = transform.position;
    }

    void FixedUpdate()
    {
        SaveToQueue();
        if( transform.position.y < SAVE_BOTTOM_BOUNDARY){
                transform.position = lastSavePosition;
                GameObject.Find("/SceneController").
                GetComponent<GameFlowController>().ReducePlayerLife();
        }
    }
}
