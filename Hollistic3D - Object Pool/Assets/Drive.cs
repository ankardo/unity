using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Drive : MonoBehaviour
{
    public float speed = 10.0f;
    public GameObject bullet;

    public GameObject explosion;

    public Slider healthBar;

    void Update()
    {
        float translation = Input.GetAxis("Horizontal") * speed;
        translation *= Time.deltaTime;
        transform.Translate(translation, 0, 0);

        if (Input.GetKeyDown("space"))
        {

            GameObject b = Pool.instance.Get("Bullet");
            if (b != null)
            {
                b.transform.position = this.transform.position;
                b.SetActive(true);
            }
        }
        Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position) + new Vector3(0, -70, 0);
        healthBar.transform.position = screenPos;

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Asteroid")
        {
            other.gameObject.SetActive(false);
            healthBar.value -= 50;
            if (healthBar.value <= 0)
            {
                this.gameObject.SetActive(false);
                healthBar.gameObject.SetActive(false);
                Instantiate(explosion, this.transform.position, Quaternion.identity);
            }
        }
    }
}