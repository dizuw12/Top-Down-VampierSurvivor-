using System.Collections;
using TMPro;
using UnityEngine;


public class Player : MonoBehaviour
{

    private Rigidbody2D rb;
    private Vector2 MoveInput;
    private SpriteRenderer rbSprite;

    [Header("Zdrowie")]
    [SerializeField] private int maxHp = 100;
    [SerializeField] private float iFrameCooldown = 1f;
    [SerializeField] private TextMeshProUGUI hpCounterText;
    private int currentHp;
    private float iFrameCooldownCounter;
    private bool isattacked;

    [Header("Ustawienia postaci")]
    [SerializeField] private float MoveSpeed = 5f;
    [SerializeField] private float DashPower = 50f;
    [SerializeField] private float DashTime = 0.08f;
    [SerializeField] private float DashCooldown = 2f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI cooldownText;

    [Header("Attack")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackOffSet = 0.6f;
    [SerializeField] private float attackTime = 0.6f;
    [SerializeField] private int attackdamage = 10;
    [SerializeField] private LayerMask enemyLayers;

    private float blinkTime = 1f;
    private Color basicColor;
    private bool isattacking;
    private float attackCooldownCounter;
    private float dashCooldownCounter;
    private bool isdashing;
    private float dashCounter;
    private Vector2 lastMoveInput;



    void Start()
    {
       rbSprite = GetComponent<SpriteRenderer>();
       rb = GetComponent<Rigidbody2D>();
       currentHp = maxHp;
        basicColor = rbSprite.color;

    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        Movment();
        DashCooldownCounter();
        IFrameCooldownHandle();



    }


    private void FixedUpdate()
    {
        if (isdashing)
        {
            rb.linearVelocity = lastMoveInput * DashPower;
        }
        else if (isattacking)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = MoveInput * MoveSpeed;
        }
        

    }
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isdashing && dashCooldownCounter <= 0)
        {
            Dash();
        }
        if (Input.GetKeyDown(KeyCode.J) && !isdashing && !isattacking)
        {
            Attack();
        }
    }


    private void Movment()
    {
        
        
        if (isdashing)
        {
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0)
            {
                isdashing = false;
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
            }
            return;
        }
        if (isattacking)
        {
            attackCooldownCounter -= Time.deltaTime;
            if (attackCooldownCounter <= 0)
            {
                isattacking = false;
            }
            return;
        }
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        MoveInput = new Vector2(moveX, moveY);
        if (MoveInput.magnitude > 1)
        {
            MoveInput = MoveInput.normalized;
        }
        if (MoveInput != Vector2.zero)
        {
            lastMoveInput = MoveInput;
        }
        
    }

    private void DashCooldownCounter()
    {
        if (dashCooldownCounter > 0)
        {
            dashCooldownCounter -= Time.deltaTime;
            if (cooldownText != null)
                cooldownText.text = dashCooldownCounter.ToString("F1"); 
        }
        else
        {
            if (cooldownText != null)
                cooldownText.text = "READY";
        }
        
    }
    private void IFrameCooldownHandle()
    {
        if (isattacked)
        {
            iFrameCooldownCounter -= Time.deltaTime;
            if (iFrameCooldownCounter <= 0)
                isattacked = false;
        }
    }

    private void Dash()
    {
        if (lastMoveInput == Vector2.zero)
        {
            lastMoveInput = new Vector2(0, -1);
        }
        isdashing = true;
        dashCounter = DashTime;
        dashCooldownCounter = DashCooldown;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
    }
    private void Attack()
    {
        if (lastMoveInput == Vector2.zero)
        {
            lastMoveInput = new Vector2(0, -1);
        }
        isattacking = true;
        attackCooldownCounter = attackTime;

        Vector2 attackposition = (Vector2)transform.position + (lastMoveInput * attackOffSet);

        Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackposition, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitenemies)
        {
            Debug.Log($"Wróg dosta³: {enemy.name}");
            Enemy enemyScript = enemy.GetComponent<Enemy>();

            if (enemyScript != null)
                enemyScript.TakeDamage(attackdamage, lastMoveInput);
        }
    }
    public void TakeDamage(int damage)
    {
        if (!isattacked && !isdashing)
        {
            StartCoroutine(BlinkCouroutine());
            currentHp -= damage;
            Debug.Log($"{gameObject.name} take {damage} damage!!!");
            if (currentHp <= 0)
                FindFirstObjectByType<GameManager>().GameOver();
            isattacked = true;
            iFrameCooldownCounter = iFrameCooldown;
            hpCounterText.text = currentHp.ToString();
        }


    }
    private IEnumerator BlinkCouroutine()
    {
        rbSprite.color = Color.red;
        yield return new WaitForSeconds(blinkTime);
        rbSprite.color = basicColor;
    }
    
    private void OnDrawGizmos()
    {
        Vector2 drawDirection = lastMoveInput == Vector2.zero ? new Vector2(0, -1) : lastMoveInput;

        Vector2 attackPosition = (Vector2)transform.position + (drawDirection * attackOffSet);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition, attackRange);
    }

}
