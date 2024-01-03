using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    public GameObject towerObj; // default empty
    public Turret turret;
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
        if (UIManager.main.IsHoveringUI()) return; // over UI not plot

        // later open menu: upgrade, sell, etc.
        if (towerObj != null)
        {
            turret.OpenMenuUI();
            
            return;
        }

        Tower towerToBuild = BuildManager.main.GetSelectedTower(); // return the towerObj

        if (towerToBuild.cost > LevelManager.main.currency) return; // can't afford

        LevelManager.main.SpendCurrency(towerToBuild.cost); // spend money on selected towerObj to build

        towerObj = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        turret = towerObj.GetComponent<Turret>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
