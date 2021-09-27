using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("d"))
        {
            animator.SetBool("isWalkingForwards", true);
        }
        else
        {
            animator.SetBool("isWalkingForwards", false);
        }
        
        if (Input.GetKey("left shift") && Input.GetKey("d") )
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        
        if (Input.GetKey("space"))
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
        
        if (Input.GetKey("a"))
        {
            animator.SetBool("isWalkingBackwards", true);
        }
        else
        {
            animator.SetBool("isWalkingBackwards", false);
        }
        
        
    }
}
