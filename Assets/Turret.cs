using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform gunRotationPoint;
    [SerializeField] private LayerMask enemyMask; // turrets' target layer
    [SerializeField] private GameObject bulletPrefab; // the bullet shot out
    [SerializeField] private Transform firingPoint; // tip of the barrel
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float bps = 1f; // Bullets Per Second
    [SerializeField] private float debuffRate = 1f; // rate of slowing on hit enemies
    [SerializeField] private int baseUpgradeCost = 100;

    private float bpsBase;
    private float targetingRangeBase;

    private Transform target;
    private float timeUntilFire;

    private int level = 1;

    private void Start()
    {
        bpsBase = bps;
        targetingRangeBase = targetingRange;

        upgradeButton.onClick.AddListener(Upgrade); // run the upgrade function
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange()) // no longer in range
        {
            target = null; // reset target
        } else // if a target iswithin range, shoot
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bps) // reload rate
            {
                Shoot();
                timeUntilFire = 0f; // reset timer
            }
        }
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2) transform.forward, 0f, enemyMask);

        // hit something
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        gunRotationPoint.rotation = Quaternion.RotateTowards(gunRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime); // rotate only gun toward the enemy
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) < targetingRange;
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity); // instant of the bullet
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
        bulletScript.SetDebuffRate(debuffRate);
    }

    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false); // undo hovering state
    }

    public void Upgrade()
    {
        if (CalculateCost() >= LevelManager.main.currency) return; // can't afford

        LevelManager.main.SpendCurrency(CalculateCost());

        level++;

        bps = CalculateBps();
        targetingRange = CalculateRange();

        CloseUpgradeUI(); // close the UI window
    }

    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f)); // factor of upgrade cost per level
    }

    private float CalculateBps()
    {
        return bpsBase * Mathf.Pow(level, 0.6f);
    }

    private float CalculateRange()
    {
        return bpsBase * Mathf.Pow(level, 0.4f);
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
    
}
