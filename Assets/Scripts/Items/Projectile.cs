using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private int damage;

    [SerializeField] private AudioSource shotSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        shotSound.Play();

    }

    public void Init(float velocity, float duration, int damage)
    {
        this.damage = damage;
        rb.velocity = transform.right * velocity;
        StartCoroutine(DoDestroy(duration));
    }

    IEnumerator DoDestroy(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall")) Destroy(this.gameObject);
        if (gameObject.layer == collision.gameObject.layer || gameObject.tag == "Room") return;
        if (collision.transform.TryGetComponent(out Character ch))
        {
            ch.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
