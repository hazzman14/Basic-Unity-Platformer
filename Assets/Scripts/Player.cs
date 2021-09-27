using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Transform groundCheckTransform;
    public LayerMask playerMask;
    public Material superJump;
    public Material superSpeed;
    public Material player;
    public Material flag;
    public Material flight;
    public Text powerupText;
    private float horizontalInput;
    private float verticalInput;
    public bool tempSuperJump;
    public bool tempSuperSpeed;
    public bool tempFly;
    private bool jumpKeyWasPressed;
    private Rigidbody rigidbodyComponent;
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

        if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0 && tempFly == false)
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
        switch (other.gameObject.layer)
        {
            case 6:
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
                    case "Flight":
                        StartCoroutine(tempFlying());
                        break;
                }
                break;
            case 7:
                SceneManager.LoadScene (sceneName:"Endscreen");
                break;
            case 8:
                other.GetComponent<Renderer>().material = flag;
                respawnPoint = other.GetComponent<Transform>().position;
                break;
        }
    }
    //put these 2 methods into powerup class and set text there, hard to do ienumerators, - use instance?
    IEnumerator tempJumpBoost()
    {
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = superJump;
        // gameObject.GetComponent<Renderer>().material = superJump;
        tempSuperJump = true;
        powerupText.text = "Super Jump!";
        yield return new WaitForSeconds(5);
        powerupText.text = "";
        tempSuperJump = false;
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = player;
        // gameObject.GetComponent<Renderer>().material = player;
    }
    
    IEnumerator tempSpeedBoost()
    {
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = superSpeed;
        tempSuperSpeed = true;
        powerupText.text = "Super Speed!";
        yield return new WaitForSeconds(5);
        gameObject.GetComponent<animationStateController>().animator.SetBool("isRunning",false);
        powerupText.text = "";
        tempSuperSpeed = false;
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = player;
    }
    
    IEnumerator tempFlying()
    {
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = flight;
        tempFly = true;
        powerupText.text = "Flappy Bird!";
        yield return new WaitForSeconds(5);
        powerupText.text = "";
        tempFly = false;
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = player;
    }

    public void Respawn()
    {
        gameObject.GetComponent<Transform>().position = respawnPoint;
    }
}
