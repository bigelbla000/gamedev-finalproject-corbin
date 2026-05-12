using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage;
    public PlayerHealth playerHealth;
    public PlayerMovement playerMovement;
   
    public int health;
    public int maxHealth;
    public GameObject deathEffect;
void Start() {
    health = maxHealth;
}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
           
            playerHealth.TakeDamage(damage);
            //Debug.Log(collision.gameObject.name);
        }
    }

    public void TakeDamage (int damage)
    {
        Debug.Log("Sanity Check");
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die ()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
