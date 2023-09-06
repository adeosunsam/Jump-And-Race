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
            transform.Translate(Vector3.left * Time.deltaTime * 25f);

        if(transform.position.x < -15)
        {
            Destroy(gameObject);
        }
    }
}
