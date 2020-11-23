using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    public Vector3 velocity;
    private void Update()
    {
        this.transform.Translate(velocity);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Asteroid")
        {
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }
}
