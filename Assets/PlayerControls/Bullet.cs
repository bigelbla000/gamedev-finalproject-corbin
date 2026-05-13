using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 20f;
    public int damage = 50;
    public Rigidbody2D rb;
    public float despawnTime;
    private float despawnTimer;
    //private bool hit; //Done because of the gameObject not destorying its self fast enough.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        despawnTimer = despawnTime;
    }

    void OnTriggerEnter2D (Collider2D hitInfo)
    {
        EnemyDamage enemy = hitInfo.GetComponent<EnemyDamage>();
        Debug.Log("a");
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log("aa");
        }
        Destroy(gameObject);
    }

}
