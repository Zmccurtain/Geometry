using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    private PlayerController player;
    public float TrailTime;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        transform.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var targetX = player.transform.position.x;
        var targetY = player.transform.position.y;

        var rate = 1 / TrailTime * Time.deltaTime;

        transform.position = new Vector2(Mathf.MoveTowards(transform.position.x, targetX, rate),
                                         Mathf.MoveTowards(transform.position.y, targetY, rate));

    }
}
