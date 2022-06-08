using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class ConsumablePickup : MonoBehaviour
{
    public float value;

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;

        if (gameObject.tag == "Health")
        {
            collision.gameObject.GetComponent<Character>().Heal(value);
        }
            
        else if (gameObject.tag == "Mana")
        {
            collision.gameObject.GetComponent<Character>().MPHeal(value);
        }
            
        else if (gameObject.tag == "Tarot")
        {

            collision.gameObject.GetComponent<TarotManager>().AddTarot((int)value);
        }
            

        Destroy(gameObject);
    }
}
