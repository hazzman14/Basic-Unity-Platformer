using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{
    public bool up;
    public bool down;
    public bool forward;
    public bool backwards;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        up = Input.GetKey("w");
        down = Input.GetKey("s");
        forward = Input.GetKey("d");
        backwards = Input.GetKey("a");
        
        if (forward || up || down)
        {
            animator.SetBool("isWalkingForwards", true);
        }
        else
        {
            animator.SetBool("isWalkingForwards", false);
        }
        
        if (gameObject.GetComponent<Player>().tempSuperSpeed == true && (forward || up || down))
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        
        if (Input.GetKey("space") && GameObject.Find("Player").GetComponent<Player>().tempSuperJump==true)
        {
            animator.SetBool("isSuperJumping", true);
        }
        else
        {
            animator.SetBool("isSuperJumping", false);
        }
        
        if (Input.GetKey("space"))
        {
            if (GameObject.Find("Player").GetComponent<Player>().tempSuperJump==true)
            {
                animator.speed = 0.66f;
            }
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
            animator.speed = 1f;
        }
        
        
        
        if (backwards)
        {
            animator.SetBool("isWalkingBackwards", true);
        }
        else
        {
            animator.SetBool("isWalkingBackwards", false);
        }

    }
}
