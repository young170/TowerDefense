using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public LevelManager levelManager;
    public HealthManager healthManager;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;
    private bool isSpeedChanged = false;

    // Start is called before the first frame update
    private void Start()
    {
        target = LevelManager.main.path[pathIndex];
    }

    // Update is called once per frame
    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex == LevelManager.main.path.Length)
            {
                HealthManager.hp--;

                EnemySpawner.onEnemyDestroy.Invoke(); // call the listener
                Destroy(gameObject);

                if (HealthManager.hp == 0) {
                    Application.Quit();
                }

                return;
            } else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * moveSpeed;
    }

    public void SetMoveSpeedByRate(float rate)
    {
        if (!isSpeedChanged)
        {
            this.moveSpeed = moveSpeed * rate;
            isSpeedChanged = !isSpeedChanged;
        }
    }
}
