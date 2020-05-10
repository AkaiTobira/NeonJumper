using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetectorEnemy : CollisionDetector
{

    void Start() {
        horizontalRayNumber -= 1;
    }
    override protected void ProcessCollision(){
        Vector2 saveTransition = transition;
        ProcessCollisionVertical(   DIR_DOWN );
        ProcessCollisionHorizontal( DIR_LEFT );
        ProcessCollisionHorizontal( DIR_RIGHT);
        ProcessHitFromHead();
        transition = saveTransition;
    }

    public bool isHitInHead(){
        return collisionInfo.above;
    }

    public bool isHitNotInHead(){
        return collisionInfo.below || collisionInfo.left || collisionInfo.right;
    }

    private void ProcessHitFromHead(){
        float rayLenght  = Mathf.Abs (transition.y) + skinSize;

        for( int i = -1; i <= 1; i++){
            Vector2 rayOrigin = new Vector2( (borders.left + borders.right)/2.0f + 
                                             -1 * i * verticalDistanceBeetweenRays, 
                                             borders.top  );

            RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin,
                new Vector2( 0, DIR_UP),
                rayLenght,
                m_collsionMask
            );

            if( hit ){
                rayLenght  = hit.distance;
                collisionInfo.above = true;
            }
            Debug.DrawRay(
                rayOrigin,
                new Vector2( 0, DIR_UP) * rayLenght,
                new Color(1,1,0)
             );
        }
        
    }
}
