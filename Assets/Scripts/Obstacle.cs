using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, ICollider
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!Player.gameOver)
            transform.Translate(25f * Time.deltaTime * Vector3.left);

        if(transform.position.x < -15)
        {
            Destroy(gameObject);
        }
    }
}
