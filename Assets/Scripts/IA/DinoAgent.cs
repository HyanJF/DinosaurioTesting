using TMPro;
using UnityEngine;

public class DinoAgent : MonoBehaviour
{
    public Genome genome;
    public float fitness = 0;
    public bool isAlive = true;

    public int reachedGoalRound = 0;  // <-- NUEVO: Ronda alcanzada por el agente

    private Rigidbody2D rb;
    private float timeAlive = 0f;
    [SerializeField] private bool isGrounded = true;
    private bool isJumpScheduled = false;
    public TextMeshPro uiText;
    private ObstacleType upcomingObstacleType = ObstacleType.Low;
    private float distanceToObstacle = Mathf.Infinity;

    // Variables para caída acelerada
    private bool isJumping = false;
    private float jumpStartTime = 0f;
    private float floatDuration = 0.3f;
    private float fallMultiplier = 2.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Genome newGenome)
    {
        genome = newGenome;
        isAlive = true;
        fitness = 0;
        timeAlive = 0f;
        isGrounded = true;
        isJumpScheduled = false;
        reachedGoalRound = 0;  // <-- Reiniciar progreso ronda
        gameObject.SetActive(true);
        transform.rotation = Quaternion.identity;
        GetComponent<SpriteRenderer>().color = Random.ColorHSV();

        if (uiText != null)
            uiText.text = $"F: {fitness:F1}\nL:{genome.lowJumpForce:F1}\nM:{genome.midJumpForce:F1}\nH:{genome.highJumpForce:F1}\nD:{genome.jumpThreshold:F1}";
    }

    void Update()
    {
        if (!isAlive) return;

        timeAlive += Time.deltaTime;
        fitness = timeAlive;

        if (uiText != null)
        {
            uiText.text =
                $"<color=#00BFFF><b>Low Jump:</b></color> <color=#FFFFFF>{genome.lowJumpForce:F1}</color>  " +
                $"<color=#1E90FF><b>Mid Jump:</b></color> <color=#FFFFFF>{genome.midJumpForce:F1}</color>  " +
                $"<color=#0000CD><b>High Jump:</b></color> <color=#FFFFFF>{genome.highJumpForce:F1}</color>\n" +
                $"<color=#FF4500><b>Next Obstacle:</b></color> <color=#FFFFFF>{upcomingObstacleType}</color>\n" +
                $"<color=#8A2BE2><b>Status:</b></color> <color=#FFFFFF>{(isGrounded ? "Grounded" : "Jumping")}</color>\n" +
                $"<color=#FFA500><b>Optimal Dist:</b></color> <color=#FFFFFF>{genome.optimalJumpDistance:F1}</color>\n" +
                $"<color=#32CD32><b>Round Reached:</b></color> <color=#FFFFFF>{reachedGoalRound}</color>";
        }

        Vector2 direction = Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, genome.jumpThreshold, LayerMask.GetMask("Obstacle"));

        if (hit.collider != null && hit.collider.CompareTag("Obstacle"))
        {
            distanceToObstacle = hit.distance;

            Obstacle obstacle = hit.collider.GetComponent<Obstacle>();
            if (obstacle != null)
                upcomingObstacleType = obstacle.type;

            if (!isJumpScheduled && Mathf.Abs(distanceToObstacle - genome.jumpThreshold) < 0.5f)
            {
                isJumpScheduled = true;
                Invoke(nameof(Jump), genome.reactionTime);
            }
        }
        else
        {
            distanceToObstacle = Mathf.Infinity;

            if (isJumpScheduled)
            {
                CancelInvoke(nameof(Jump));
                isJumpScheduled = false;
            }
        }

        if (rb.linearVelocity.y < 0) // cayendo
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (isJumping) // durante el salto
        {
            if (Time.time - jumpStartTime > floatDuration)
            {
                isJumping = false;
            }
        }
    }

    void Jump()
    {
        if (isGrounded && isAlive)
        {
            float jumpForce = genome.lowJumpForce;

            switch (upcomingObstacleType)
            {
                case ObstacleType.Medium:
                    jumpForce = genome.midJumpForce;
                    break;
                case ObstacleType.High:
                    jumpForce = genome.highJumpForce;
                    break;
            }

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            isJumpScheduled = false;

            isJumping = true;
            jumpStartTime = Time.time;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground"))
            isGrounded = true;

        if (other.collider.CompareTag("Obstacle"))
        {
            isAlive = false;
            gameObject.SetActive(false);
        }
    }
}
