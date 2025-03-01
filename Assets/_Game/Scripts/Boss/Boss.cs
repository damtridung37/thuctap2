using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public float health;
    public Enemy[] enemies;
    public float spawnOffset;

    private float halfHealth;
    private Animator anim;

    public int damage;

    private Slider healthBar;

    private SceneTransitions sceneTransitions;

    private Vector3 previousPosition;

    public float shotTime;
    public float timeBetweenShots;
    public BossProjectile projectile;
    public Transform shotPoint;
    public int numberOfProjectiles = 12;

    private void Start()
    {
        halfHealth = health / 2;
        anim = GetComponent<Animator>();
        previousPosition = transform.position;
        healthBar = FindObjectOfType<Slider>();
        healthBar.maxValue = health;
        healthBar.value = health;
        sceneTransitions = FindObjectOfType<SceneTransitions>();
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.Paused) return;
        FlipDirection();
        previousPosition = transform.position;

        if (Time.time >= shotTime)
        {
            ShootInCircle();
            shotTime = Time.time + timeBetweenShots;
        }
    }

    private void ShootInCircle()
    {
        float angleStep = 360f / numberOfProjectiles; // Góc chia đều cho mỗi viên đạn
        float startAngle = 0f; // Góc bắt đầu

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float currentAngle = startAngle + i * angleStep; // Tính góc hiện tại
            BossProjectile temp = Instantiate(projectile, shotPoint.position, Quaternion.identity); // Tạo đạn
            temp.Init(currentAngle); // Truyền góc để đạn di chuyển đúng hướng
        }
    }

    private void FlipDirection()
    {
        if (transform.position.x > previousPosition.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (transform.position.x < previousPosition.x)
        {

            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        healthBar.value = health;
        if (health <= 0)
        {
            Destroy(this.gameObject);
            healthBar.gameObject.SetActive(false);
            sceneTransitions.LoadScene("Win");
        }

        if (health <= halfHealth)
        {
            anim.SetTrigger("stage2");
            damage = 9999;
        }

        WaveSpawner.Instance.GetRandomEnemy();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().TakeDamage(damage);
        }
    }
}
