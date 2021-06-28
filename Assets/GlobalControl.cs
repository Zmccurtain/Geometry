using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;
    public float speed = 10;
    public float bulletSpeed = 30;
    public float bulletCD = .2f;
    public float bulletDamage = 20;
    public float MaxHP = 100;
    public float HP = 100;
    public float xp = 0;
    public float xpPerLevel = 100;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public static void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            if (trans.tag.Equals("Wall"))
            {
                trans.GetChild(0).gameObject.layer = layerNumber;
            }
        }
    }

    public static void gainXP(float amount)
    {
        PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>().addXP(amount);
    }
}

