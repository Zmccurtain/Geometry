using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public float MaxHP = 0;
    public float HP = 0;
    private PlayerController player;
    private Slider bar;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        bar = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        MaxHP = player.MaxHP;
        bar.maxValue = MaxHP;
        HP = player.HP;
        bar.value = HP;
    }
}
