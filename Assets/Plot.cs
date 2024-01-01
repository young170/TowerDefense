using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject tower; // default empty
    private Color startColor;

    // Start is called before the first frame update
    void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        // later open menu: upgrade, sell, etc.
        if (tower != null) return; // tower already exists

        Tower towerToBuild = BuildManager.main.GetSelectedTower(); // return the tower

        if (towerToBuild.cost > LevelManager.main.currency) // not enough money
        {
            Debug.Log("Can't afford this tower");
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost); // spend money on selected tower to build

        tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
