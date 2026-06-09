using UnityEngine;
using System.Collections;


public class Enemy : MonoBehaviour
{
    private SpriteRenderer spriteRen;
    private Rigidbody2D rb;
    private Transform playerTarget;

    [Header("Statystyki")]
    [SerializeField] private int maxHp = 30;
    [SerializeField] private float moveSpeed = 10f;
    private int currentHp;

    [SerializeField] private float knockbackForce = 15f;
    private float knockbackTime = 0.15f;
    private bool isKnocked;

    [SerializeField] private float blinkDuration = 0.1f;
    private Color blinkColor = Color.red;
    private Color basicColor;

  
    void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRen = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        basicColor = spriteRen.color;
        currentHp = maxHp;
    }

    
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        if (!isKnocked)
        {
            Vector2 enemyDir = (playerTarget.position - transform.position).normalized;
            rb.linearVelocity = enemyDir * moveSpeed;
        }
    }
    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        currentHp -= damage;
        
        Debug.Log($"{gameObject.name} take {damage}damage!!");
        Debug.Log($"{gameObject.name} {currentHp}hp left!!");
        StartCoroutine(BlinkCouroutine());
        StartCoroutine(KnockBackCourotine(hitDirection));
        if (currentHp <= 0)
        {
            Die();
        }
    }
    private IEnumerator BlinkCouroutine()
    {
        spriteRen.color = blinkColor;
        yield return new WaitForSeconds(blinkDuration);
        spriteRen.color = basicColor;
    }
    private IEnumerator KnockBackCourotine(Vector2 knocbackDir)
    {
        isKnocked = true;
        rb.linearVelocity = knocbackDir * knockbackForce;
        yield return new WaitForSeconds(knockbackTime);
        isKnocked = false;
        rb.linearVelocity = Vector2.zero;
    }
    private void Die()
    {
        Debug.Log($"{gameObject.name} Die!!");
        Destroy(gameObject);
    }
}
