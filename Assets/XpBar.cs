using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class XpBar : MonoBehaviour
{
    // Start is called before the first frame update
    private Slider bar;
    private Text xp;
    private Text toNext;
    private Text level;
    private PlayerController player;
    void Start()
    {
        
        bar = GetComponent<Slider>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        foreach (Text i in GetComponentsInChildren<Text>())
        {
            if (i.gameObject.name.Equals("XP")) xp = i;
            if (i.gameObject.name.Equals("XpToNext")) toNext = i;
            if (i.gameObject.name.Equals("Level")) level = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bar.maxValue = player.xpPerLevel;
        bar.value = player.xp;
        xp.text = "XP: " + player.xp;
        toNext.text = "XP to Next Level: " + player.xpToNext;
        level.text = player.Level.ToString();
        
    }
}
