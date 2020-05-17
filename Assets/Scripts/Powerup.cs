﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField] // 0 = TripleShot 1 = Speed 2 = Shields
    private int _powerupID;

    [SerializeField]
    private AudioClip _clip;


    // Update is called once per frame
    void Update()
    {

        // move down at a speed of 3 (adjust in the inspector)
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // when we leave the screen, destroy this object
        if(transform.position.y < -5.8)
        {
            Destroy(this.gameObject);
        }
    }


    // OnTriggerCollision
    // Only be collectible by the player (HINT: use tags)
    // On collected, destroy this object
    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if(player != null)
            {
                switch(_powerupID)
                {
                    case 0: 
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }


}
