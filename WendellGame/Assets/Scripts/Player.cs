using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 10;
    public float speed;
    public float jumpForce;
    
    private bool isJumping;
    private bool doublejump;
    private bool isAtk;

    public static float movement;

    private Rigidbody2D rig;
    private Animator anim;

    public Vector3 respawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        respawnPoint = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        
        GameController.instance.UpdateLives(health);
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Attack();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // se não pressionar nada valor é 0. Pressionar direita, valor máximo 1. Esquerda valor máximo -1
        movement = Input.GetAxis("Horizontal");
        
        // adciona velocidade ao corpo no eixo x e y
        rig.velocity = new Vector2(movement * speed, rig.velocity.y);
        
        //andando pra direita
        if (movement > 0)
        {
            if(!isJumping)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        
        //andando pra esquerda
        if (movement < 0)
        {
            if(!isJumping)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0,180,0);
        }
        
        //parado
        if (movement == 0 && !isJumping && !isAtk)
        {
            anim.SetInteger("transition", 0);
        }
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            if(!isJumping)
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                doublejump = true;
                isJumping = true;
                
            }
            else
            {
                if(doublejump)
                {
                    anim.SetInteger("transition", 2);
                    rig.AddForce(new Vector2(0, jumpForce * 1), ForceMode2D.Impulse);
                    doublejump = false;
                }
            }        
        }

    }
    
    void Attack()
        {
            StartCoroutine("Atk");
        }
    
        IEnumerator Atk()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isAtk = true;
                anim.SetInteger("transition", 3);
                yield return new WaitForSeconds(0.55f);
                isAtk = false;
                anim.SetInteger("transition", 0);
            }
        }

        public void Damage(int dmg)
        {
            health -= dmg;
            GameController.instance.UpdateLives(health);
            anim.SetTrigger("hit");

            if (transform.rotation.y == 0)
            {
                transform.position += new Vector3(-0.5f, 0, 0);
            }

            if (transform.rotation.y == 180)
            {
                transform.position += new Vector3(0.5f, 0, 0);
            }
            
            if (health <= 0)
            {
                //chamar o game over
                
            }
        }

        public void Increaselife(int value)
        {
            health += value;
            GameController.instance.UpdateLives(health);
        }
    
    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.layer == 8)
        {
            isJumping = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "falldetector")
        {
            health -= 1;
            GameController.instance.UpdateLives(health);
            transform.position = respawnPoint;
        }
        else if (collider.gameObject.tag == "Checkpoint")
        {
            collider.gameObject.GetComponent<Checkpoint>().Activate();
            respawnPoint = new Vector3(collider.gameObject.transform.position.x, collider.gameObject.transform.position.y + 1, collider.gameObject.transform.position.z);
        }
    }
}

