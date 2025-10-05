
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
       private Renderer rend;
       public Animator anim;
       public GameObject healthLoot;
       public int maxHealth = 100;
       public int currentHealth;

       void Start(){
              rend = GetComponentInChildren<Renderer> ();
              anim = GetComponentInChildren<Animator> ();
              currentHealth = maxHealth;
       }

       public void TakeDamage(int damage){
              currentHealth -= damage;
              rend.material.color = new Color(2.4f, 0.9f, 0.9f, 1f);
        //StartCoroutine(ResetColor());
        //anim.SetTrigger ("Hurt");
              Debug.Log("taking damage!");
              if (currentHealth <= 0)
        {
            Die();
        }
       }

       void Die(){
              Destroy(gameObject);

              Debug.Log("dead1");
              StartCoroutine(Death());
       }

       IEnumerator Death(){
              yield return new WaitForSeconds(0.5f);
              Debug.Log("dead2");
              Destroy(gameObject);
       }

       IEnumerator ResetColor(){
              yield return new WaitForSeconds(0.5f);
              rend.material.color = Color.white;
       }
}
