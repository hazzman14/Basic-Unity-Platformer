using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] Material superJump;
    [SerializeField] Material superSpeed;
    [SerializeField] Material player;
    private bool jumpKeyWasPressed;
    private float horizontalInput;
    private float verticalInput;
    private Rigidbody rigidbodyComponent;
    private bool tempSuperJump;
    private bool tempSuperSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyWasPressed = true;
        }

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }
    
    //FixedUpdate is called once every physics update, not affected by FPS, 100 a second default
    private void FixedUpdate()
    {
        float speedMult = 2f;
        if (tempSuperSpeed)
        {
            speedMult *= 1.5f;
        }
        rigidbodyComponent.velocity = new Vector3(horizontalInput * speedMult,rigidbodyComponent.velocity.y,verticalInput * speedMult);

        if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0)
        {
            return;
        }
        
        if (jumpKeyWasPressed)
        {
            float jumpPower = 5f;
            if (tempSuperJump)
            {
                jumpPower *= 1.5f;
            }
            
            rigidbodyComponent.AddForce(Vector3.up * jumpPower,ForceMode.VelocityChange);
            jumpKeyWasPressed = false;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Destroy(other.gameObject);
            GameObject.Find("Score Text").GetComponent<Score>().score++;
            
            switch (other.name)
            {
                case "Super Jump":
                    StartCoroutine(tempJumpBoost());
                    break;
                
                case "Super Speed":
                    StartCoroutine(tempSpeedBoost());
                    break;
            }
        }
        
        if (other.gameObject.layer == 7)
        {
            SceneManager.LoadScene (sceneName:"Endscreen");
        }
    }

    IEnumerator tempJumpBoost()
    {
        transform.GetComponent<Renderer>().material = superJump;
        tempSuperJump = true;
        yield return new WaitForSeconds(5);
        tempSuperJump = false;
        transform.GetComponent<Renderer>().material = player;
    }
    
    IEnumerator tempSpeedBoost()
    {
        transform.GetComponent<Renderer>().material = superSpeed;
        tempSuperSpeed = true;
        yield return new WaitForSeconds(5);
        tempSuperSpeed = false;
        transform.GetComponent<Renderer>().material = player;
    }
    
}
