using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //speed variable of 8
    [SerializeField]
    private float _speed = 8.0f;
    private bool _isEnemyLaser = false;



    // Update is called once per frame
    void Update()
    {
        if(_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }

    }

    void MoveUp()
    {
        //translate Laser up

        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        //if laser position is greater than 8 on the y
        //destroy the object

        if (transform.position.y > 8f)
        {
            // check if this object has a parent
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            // destroy the parent too!
            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        //translate Laser up

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        //if laser position is greater than 8 on the y
        //destroy the object

        if (transform.position.y < -8f)
        {
            // check if this object has a parent
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            // destroy the parent too!
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && _isEnemyLaser==true)
        {
            Player player = other.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }
        }
    }

}
