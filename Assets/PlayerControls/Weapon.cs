using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
   public Transform firePoint;
   public GameObject bulletPrefab;
   public float bulletSpeed;
   [Range(0.5f, 5f)]
   public float cooldownTime;
   private bool cooldownActive;
   private float cooldownTimer;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetButtonDown("Fire1") && !cooldownActive)
        {
            Shoot();
        }
        if(cooldownActive && (cooldownTimer > 0f)) {
            cooldownTimer -= Time.deltaTime;
        }
        if(cooldownTimer <= 0f) {
            cooldownActive = false;
            cooldownTimer = cooldownTime;
        }
        
    }
    void Start() {
        cooldownTimer = cooldownTime;
    }

    void Shoot()
    {
        Transform firepointposition = this.transform.GetChild(transform.childCount -1).transform;

        GameObject newProjectile = Instantiate(bulletPrefab, firePoint.position, this.transform.rotation);
        Rigidbody2D firing = newProjectile.GetComponent<Rigidbody2D>();
        firing.AddForce((firePoint.position - this.transform.position) * bulletSpeed, ForceMode2D.Impulse);
        cooldownActive = true;
    }

//DEBUG SEGMENT -- DISABLE IF COMPILING FOR FINAL.
public float GetcooldownTimer() {
    return cooldownTimer;
}
public bool GetcooldownActive() {
    return cooldownActive;
}

}
