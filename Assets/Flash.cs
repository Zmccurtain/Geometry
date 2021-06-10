using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Flash : MonoBehaviour
{
    public float flashTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (flashTime <= 0) Destroy(gameObject);
        else flashTime -= Time.deltaTime; 
    }
}
