using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    
    public static HealthManager main;

    public static int hp = 10;

    private void Awake() {
        main = this;
    }
}
