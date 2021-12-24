using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;
    public float boundX = 1.50f;
    public float boundY = 1.00f;

    private void LateUpdate()
    {
        Vector3 delta = Vector3.zero;

        //To check if player is in bounds of x axis
        float deltaX = lookAt.position.x - transform.position.x;
        if ( deltaX > boundX || deltaX < -boundX)
        {
            
            if(transform.position.x < lookAt.position.x)//is center of camera (transform) < player (lookAt) then player is on the right of camera focus. So we will add to delta position
            {
                //delta.x = distance from camera focus to player - bound = distance player has moved out of bounds (+)
                delta.x = deltaX - boundX;
            }
            else // on the left
            {
                //delta.x = distance from camera focus to player + bound = distance player has moved out of bounds (-)
                delta.x = deltaX + boundX;
            }
        }

        //To check if player is in bounds of y axis
        float deltaY = lookAt.position.y - transform.position.y;
        if (deltaY > boundY || deltaY < -boundY)
        {

            if (transform.position.y < lookAt.position.y)
            {
                delta.y = deltaY - boundY;
            }
            else
            {
                delta.y = deltaY + boundY;
            }
        }

        //Move camera
        transform.position += new Vector3(delta.x, delta.y, 0);
    }
}
