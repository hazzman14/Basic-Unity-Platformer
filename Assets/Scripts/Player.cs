using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Material flag;
    [SerializeField] Text powerupText;
    private bool jumpKeyWasPressed;
    private float horizontalInput;
    private float verticalInput;
    private Rigidbody rigidbodyComponent;
    [SerializeField] bool tempSuperJump;
    [SerializeField] bool tempSuperSpeed;
    public Vector3 respawnPoint = new Vector3(-2, 1, .5f);

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
        
        if (rigidbodyComponent.transform.position.y < -4)
        {
            Respawn();
        }
        
    }
    
    //FixedUpdate is called once every physics update, not affected by FPS, 100 a second default
    private void FixedUpdate()
    {
        float speedMult = 2f;
        if (tempSuperSpeed)
        {
            speedMult *= 2f;
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
        
        if (other.gameObject.layer == 8)
        {
            other.GetComponent<Renderer>().material = flag;
            respawnPoint = other.GetComponent<Transform>().position;
        }
    }
    //put these 2 methods into powerup class and set text there, hard to do ienumerators, - use instance?
    IEnumerator tempJumpBoost()
    {
        transform.GetComponent<Renderer>().material = superJump;
        tempSuperJump = true;
        powerupText.text = "Super Jump!";
        yield return new WaitForSeconds(5);
        powerupText.text = "";
        tempSuperJump = false;
        transform.GetComponent<Renderer>().material = player;
    }
    
    IEnumerator tempSpeedBoost()
    {
        transform.GetComponent<Renderer>().material = superSpeed;
        tempSuperSpeed = true;
        powerupText.text = "Super Speed!";
        yield return new WaitForSeconds(5);
        powerupText.text = "";
        tempSuperSpeed = false;
        transform.GetComponent<Renderer>().material = player;
    }

    public void Respawn()
    {
        gameObject.GetComponent<Transform>().position = respawnPoint;
    }
}
