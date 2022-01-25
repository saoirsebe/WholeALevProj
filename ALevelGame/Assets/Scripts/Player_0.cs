using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_0 : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Vector3 moveDelta;
    private RaycastHit2D hit;

    private GameObject PickTaskObj;
    private PickTask PickTaskScript;

    private bool firstFind;


    // Start is called before the first frame update
    void Start()
    {
        firstFind = false;

        boxCollider = GetComponent<BoxCollider2D>();

        PickTaskObj = GameObject.Find("PickTask");
        PickTaskScript = PickTaskObj.GetComponent<PickTask>();
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // Reset moveDelta
        moveDelta = new Vector3(x, y, 0);

        //Face left/right
        if (moveDelta.x > 0)
        {
            transform.localScale = Vector3.one;
        }
        else if (moveDelta.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //Makes sure we can move in this direction by casting a box there first. If box returns null we are free to move.
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //Movement
            transform.Translate(0,moveDelta.y * Time.deltaTime*2,0);
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x,0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //Movement
            transform.Translate(moveDelta.x * Time.deltaTime*2, 0, 0);
        }


    }

    /// <summary>
    /// When objectToFind is found then run TaskFinished in PickTask script, reset player position and open second instructions
    /// </summary>
    /// <param name="other"></param>The object that has been collided with
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("aa");
        if(PickTaskScript.objectToFindName == other.name)
        {
            if (firstFind == false)
            {
                PickTaskScript.TaskFinished(firstFind);
                ResetPosition();
                firstFind = true;
            }
            else
            {
                PickTaskScript.TaskFinished(firstFind);
            } 
        }
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(35, 11, 0);
    }
}
