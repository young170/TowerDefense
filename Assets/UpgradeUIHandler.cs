using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouse_over = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        UIManager.main.SetHoveringState(true); // send true
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        UIManager.main.SetHoveringState(false); // send false
        gameObject.SetActive(false); // close UI
    }
}
