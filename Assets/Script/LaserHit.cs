using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHit : MonoBehaviour
{
    [Header("Effect")]
    public GameObject explosion;

    [Header("Sounds")]
    public GameObject[] explosionSounds;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Laser")
        {
            //Patlama efekti vermesi için
            GameObject exp = GameObject.Instantiate(explosion, collision.gameObject.transform.position, collision.gameObject.transform.rotation) as GameObject;
            //Ses efekti vermesi için
            GameObject sound = GameObject.Instantiate(explosionSounds[Random.Range(0,explosionSounds.Length)], collision.gameObject.transform.position, collision.gameObject.transform.rotation) as GameObject;

            //Lazeri yok edicek
            Destroy(collision.gameObject);

            //Efekti yok edicek
            GameObject.Destroy(exp, 1.8f);
            //Sesi yok edicek
            GameObject.Destroy(sound, 3f);
        }
    }
}
