using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    
    PlayerController m_controller;
    Animator m_animator;

    CollisionDetectorPlayer m_detector;

    float direction_prev = 0;

    void Start()
    {
        m_detector   = GetComponent<CollisionDetectorPlayer>();
        m_controller = GetComponent<PlayerController>();
        m_animator   = GetComponent<Animator>();
    }

    private void AdaptDirection(){
        float direction = Mathf.Sign(m_controller.velocity.x);
        direction_prev = direction;
        var scale = transform.localScale;
        scale.x   = direction * Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private void JumpAnimation(){
        if( m_controller.isJumpingKeyPresed() ){
            m_animator.SetTrigger("Jump");
        }else if( m_detector.isOnGround()){
            m_animator.ResetTrigger("Jump");
        }
    }

    void Update(){
        m_animator.SetBool("hitGround", m_detector.isOnGround());

        if( m_detector.canClimbOnLadder() && m_controller.isClimbKeyPressed() ){
            m_animator.SetBool("isClimbing", true);
        }else{
            m_animator.SetBool("isClimbing", false);
        }

        m_animator.SetFloat("isRunning", Mathf.Abs( m_controller.velocity.x ));
        m_animator.SetFloat("isFalling", Mathf.Abs( m_controller.velocity.y ));

        if( m_controller.velocity.x != 0 ){
            AdaptDirection();
        }
        JumpAnimation();
    }
}
