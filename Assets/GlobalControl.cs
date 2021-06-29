using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;
    public static PlayerController player;
    public float speed = 10;
    public float bulletSpeed = 30;
    public float bulletCD = 1f;
    public float bulletDamage = 20;
    public float MaxHP = 100;
    public float HP = 100;
    public float xp = 0;
    public float xpPerLevel = 100;
    public int Level = 1;

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
        player.AddXp(amount);
    }

    public static float dist(Transform i, Transform j)
    {
        return Mathf.Sqrt(Mathf.Pow((i.position.x - j.position.x), 2) + Mathf.Pow((i.position.y - j.position.y),2));
    }
}

