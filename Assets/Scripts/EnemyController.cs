using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector3[] travelPoint;
    [SerializeField] PlayerController playerController = null;
    private CollisionDetectorEnemy m_detector = null;
    public float moveSpeed;
    private int currentTravelPoint;

    void Start()
    {
        m_detector = GetComponent<CollisionDetectorEnemy>();
        currentTravelPoint = 0;
    }

    void MoveEnemy(){
        Vector3 distanceVector = travelPoint[currentTravelPoint] - transform.position;
        float distance         = distanceVector.magnitude;
        Vector3 velocity       = distanceVector.normalized * moveSpeed * Time.deltaTime;

        if( distance - velocity.magnitude <= 0.5 ){
            currentTravelPoint = ( currentTravelPoint + 1 ) % travelPoint.Length;
        }

        m_detector.Move(velocity);
    }

    float freezeActionTimeElapsed = 0.0f;
    bool isActionFreezed(){
        float TIMER_END = 1;
        freezeActionTimeElapsed += Time.deltaTime;
        if( freezeActionTimeElapsed < TIMER_END){
            return true;
        }
        return false;
    }

    void ProcessDestroy(){
        playerController.BounceUpFromEnemy();
        gameObject.GetComponent<Animator>().SetBool("isDead", true);
        Destroy (gameObject, gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        freezeActionTimeElapsed = 0;
        GameObject.Find("/SceneController").GetComponent<GameFlowController>().AddToScore(1);
    }

    void ProcessPlayerLifeLoss(){
        playerController.HurtPlayer();
        freezeActionTimeElapsed = 0;        
    }

    void ProcessHitActions(){
        if( !isActionFreezed()){
            if( m_detector.isHitInHead() ){
                ProcessDestroy();
            }else if( m_detector.isHitNotInHead()){
                ProcessPlayerLifeLoss();
            }
        }
    }

    void Update(){
        MoveEnemy();
        ProcessHitActions();
    }
}
