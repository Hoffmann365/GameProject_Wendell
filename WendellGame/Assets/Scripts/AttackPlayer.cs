using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{

    private BoxCollider2D colliderAtkPLayer;

    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        colliderAtkPLayer = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Inversão da posição do colisor de ataque baseado na posição do player
        if (Player.movement < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (Player.movement > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().Damage(damage);
        }

        ;
    }
}
