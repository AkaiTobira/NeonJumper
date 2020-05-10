using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player = null;

    public float maxRightBoundry;
    public float maxLeftBoundry;
    private float GetXPosition(){
        return Mathf.Min( Mathf.Max( maxLeftBoundry, player.position.x), maxRightBoundry ); 
    }

    void Update()
    {
        if( player == null ) return;
        Vector3 targetPosition = player.position;
        targetPosition.y = transform.position.y;
        targetPosition.z = transform.position.z;
        targetPosition.x = GetXPosition();
        transform.position = targetPosition; 
    }
}
