using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaver : MonoBehaviour
{
    PlayerController m_controller;
    Vector3 lastSavePoint;

    const float SAVE_BOTTOM_BOUNDARY = -20;

    void Start()
    {
        lastSavePoint = transform.position;
        m_controller  = GetComponent<PlayerController>();
    }

    public void Save(){
        lastSavePoint = transform.position;
    }

    void FixedUpdate()
    {
        if( transform.position.y < SAVE_BOTTOM_BOUNDARY){
            transform.position    = lastSavePoint;
            m_controller.velocity = new Vector2(0,0);
        }
    }
}
