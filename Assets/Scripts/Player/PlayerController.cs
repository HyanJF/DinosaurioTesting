using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float maxMoveSpeed = 20f;
    public float acceleration = 30f;
    public float deceleration = 40f;
    public float jumpForce = 30f;

    [Header("Dash")]
    public float dashForce = 50f;
    public float dashCooldown = 1f;

    [Header("Attack")]
    public float attackCooldown = 0.5f;
    private bool attackReady = true;
    public AttackHitbox attackHitbox;

    [Header("Bullet")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float shootCooldown = 0.25f;
    private bool shootReady = true;

    [Header("Fisicas Y PowerUps")]
    private Rigidbody2D rb;
    private bool isGrounded = true;
    private bool isAlive = true;
    private bool isSliding = false;
    private bool canDoubleJump = false;
    private bool isDashing = false;
    private bool canDashNow = true;

    public Transform visualTransform;
    public Animator anim;

    private Vector3 originalScale;
    private BoxCollider2D boxC;
    private Vector2 size2D;
    private Vector2 offset2D;

    private float currentMoveSpeed = 0f;
    private float lastVerticalInput = 0f;

    [Header("Power-Up")]
    public bool enableDoubleJump = false;
    public bool enableDash = false;
    public bool enableAttack = false;
    public bool enableShoot = false;
    public bool enableShield = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxC = GetComponent<BoxCollider2D>();

        size2D = boxC.size;
        offset2D = boxC.offset;

        if (visualTransform == null)
        {
            Debug.LogError("Debes asignar el objeto visual (hijo escalado) al script.");
        }
        else
        {
            originalScale = visualTransform.localScale;
        }

        if (attackHitbox != null)
            attackHitbox.Deactivate();

        isAlive = true;
    }

    void Update()
    {
        if (!isAlive) return;

        float moveInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Movimiento horizontal suave
        if (moveInput != 0)
            currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, moveInput * maxMoveSpeed, acceleration * Time.deltaTime);
        else
            currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, 0, deceleration * Time.deltaTime);

        // Solo aplicar velocidad si no está dasheando
        if (!isDashing)
            rb.linearVelocity = new Vector2(currentMoveSpeed, rb.linearVelocity.y);

        anim.SetFloat("Speed", Mathf.Abs(currentMoveSpeed));

        // Saltar y doble salto
        if (verticalInput > 0 && lastVerticalInput <= 0 && !isSliding)
        {
            if (isGrounded)
            {
                Jump();
                canDoubleJump = enableDoubleJump;
            }
            else if (enableDoubleJump && canDoubleJump)
            {
                Jump();
                canDoubleJump = false;
            }
        }

        // Dash solo si está habilitado, no deslizando y puede usar dash ahora
        if (enableDash && Input.GetKeyDown(KeyCode.LeftShift) && canDashNow && !isSliding)
        {
            StartCoroutine(PerformDash());
        }

        // Deslizamiento
        if (verticalInput < 0 && isGrounded && !isSliding && rb.linearVelocity.y == 0 && moveInput != 0)
        {
            StartSlide();
        }
        else if ((verticalInput >= 0 || !isGrounded || moveInput == 0) && isSliding)
        {
            EndSlide();
        }

        // Ataque
        TryAttack(moveInput);

        // Disparo
        TryShoot(moveInput);

        // Voltear visual
        if (moveInput != 0 && visualTransform != null)
        {
            visualTransform.localScale = new Vector3(
                Mathf.Abs(originalScale.x) * Mathf.Sign(moveInput),
                visualTransform.localScale.y,
                visualTransform.localScale.z
            );
        }

        // Limitar posición en X
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -Mathf.Infinity, 8f);
        transform.position = pos;

        lastVerticalInput = verticalInput;
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
        anim.SetTrigger("Jump");
    }

    private IEnumerator PerformDash()
    {
        canDashNow = false;
        isDashing = true;

        float dashDirection = Mathf.Sign(currentMoveSpeed != 0 ? currentMoveSpeed : visualTransform.localScale.x);
        rb.linearVelocity = new Vector2(dashDirection * dashForce, 0);

        anim.SetTrigger("Dash");

        yield return new WaitForSeconds(dashCooldown);

        isDashing = false;
        canDashNow = true;
    }

    private void StartSlide()
    {
        isSliding = true;
        anim.SetBool("Slide", true);

        boxC.size = new Vector2(size2D.x, size2D.y * 0.5f);
        boxC.offset = new Vector2(offset2D.x, -0.08f);

        if (visualTransform != null)
        {
            visualTransform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);
        }
    }

    private void EndSlide()
    {
        isSliding = false;
        anim.SetBool("Slide", false);

        boxC.size = size2D;
        boxC.offset = offset2D;

        if (visualTransform != null)
        {
            visualTransform.localScale = originalScale;
        }
    }

    private void TryAttack(float moveInput)
    {
        if (!enableAttack || !attackReady || isSliding || isDashing) return;

        if (Input.GetButtonDown("Fire1"))
        {
            attackReady = false;

            if (!isGrounded)
                anim.SetTrigger("AttackAir");
            else if (Mathf.Abs(moveInput) > 0.1f)
                anim.SetTrigger("AttackRun");
            else
                anim.SetTrigger("AttackIdle");

            StartCoroutine(PerformAttack());

            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    private IEnumerator PerformAttack()
    {
        yield return new WaitForSeconds(0.1f); // Delay opcional
        if (attackHitbox != null)
            attackHitbox.Activate();

        yield return new WaitForSeconds(0.2f);
        if (attackHitbox != null)
            attackHitbox.Deactivate();
    }

    private void ResetAttack()
    {
        attackReady = true;
    }

    private void TryShoot(float moveInput)
    {
        if (!enableShoot || !shootReady || isSliding || enableAttack) return;

        if (Input.GetMouseButtonDown(1))
        {
            shootReady = false;

            if (!isGrounded)
                anim.SetTrigger("Shoot");
            else if (Mathf.Abs(moveInput) > 0.1f)
                anim.SetTrigger("ShootRun");
            else
                anim.SetTrigger("Shoot");

            Shoot();

            Invoke(nameof(ResetShoot), shootCooldown);
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        BulletController bulletCtrl = bullet.GetComponent<BulletController>();

        Vector2 direction = visualTransform.localScale.x > 0 ? Vector2.right : Vector2.left;
        bulletCtrl.SetDirection(direction);
    }

    private void ResetShoot()
    {
        shootReady = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard") && isAlive)
        {
            if (enableShield)
            {
                Debug.Log("Escudo activo: daño evitado");
                // Aquí podrías agregar efecto visual o sonido para el escudo
                return; // no sufre daño
            }

            isAlive = false;
            anim.SetTrigger("Death");
            rb.linearVelocity = Vector2.zero;
            Invoke("GameOver", 1.2f);
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        Debug.Log("GAME OVER");
    }
}
