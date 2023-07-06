using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float speed;
    public float walktime;

    private float timer;

    private bool walkRight = true;
    
    public int health;

    private Rigidbody2D rig;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= walktime)
        {
            walkRight = !walkRight;
            timer = 0f;
        }

        if (walkRight)
        {
            transform.eulerAngles = new Vector2(0, 0);
            rig.velocity = Vector2.right * speed;
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 180);
            rig.velocity = Vector2.left * speed;
        }
    }

    public void Damage(int dmg)
    {
        anim.SetTrigger("hit");
        health -= dmg;
        if (health <= 0)
        {
            StartCoroutine("die");
        }
    }

    IEnumerator die()
    {
        speed = 0;
        anim.SetTrigger("death");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
