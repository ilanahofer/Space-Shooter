using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // public or private reference
    // data type (int, float, bool, string)
    // every variable has a name
    // optional value assigned
    // put an underscore before a private variable e.g. _speed
    [SerializeField]
    private float _speed = 6f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private Vector3 _laserOffset = new Vector3(0, 1.05f, 0);
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject shieldVisualizer;
    // variable reference to the shield visualizer
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    // variable to store the audio clip
    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;



    // Start is called before the first frame update
    void Start()
    {
        //take the current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>(); //Find the GameObject. Then Get Component
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the Player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }



    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // new Vector3(1, 0, 0) is the same as Vector3.right => One unit (meter) on the X and zero units on the Y and Z
        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);    // Time.deltaTime converts from one meter per frame (60 meter per second) to one meter per second => Time.deltaTime can be seen as one second
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);


        //if player position on the y is greater than 0
        //y position = 0
        //else if position on the y is less than -3.8f
        //y pos = -3.8f

        //if (transform.position.y >= 0)
        //{
        //    transform.position = new Vector3(transform.position.x, 0, 0);
        //}
        //else if (transform.position.y <= -3.8f)
        //{
        //    transform.position = new Vector3(transform.position.x, -3.8f, 0);
        //}

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        //if player on the x > 11
        //x pos = -11
        //else if player on the x is less than -11
        //x pos = 11

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        //if I hit the space key
        //spawn gameObject

        _canFire = Time.time + _fireRate;
        //Debug.Log("Space Key pressed");


        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
        }

        // play the laser audio clip
        _audioSource.Play();

    }


    public void Damage()
    {
        // if shield is active
        if(_isShieldActive == true)
        {
            _isShieldActive = false;
            shieldVisualizer.gameObject.SetActive(false);
            // deactivate shield visualizer
            return;
        }
        // do nothing...
        // deactivate shield;
        //return;

        
        //_lives = _lives -1;
        //_lives -= 1;
        _lives--;

        if (_lives == 2)
        {
            _rightEngine.gameObject.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.gameObject.SetActive(true);
        }
        // if lives is 2
        // enable right engine
        // else if lives is 1
        // enable left engine

        _uiManager.UpdateLives(_lives);

        //check if dead
        //destroy us
        if(_lives < 1)
        {
            //Communicate with the SpawnManager
            //Let them know to stop spawning
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }

    }


    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        // start the power down coroutine for triple shot
        StartCoroutine(TripleShotPowerDownRoutine());
    }


    // IEnumerator TripleShotPowerDownRoutine
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }
    // wait 5 seconds
    // set the triple shot to false


    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    // IWnumerator SpeedBoostPowerDownRoutine
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }


    public void ShieldActive()
    {
        _isShieldActive = true;
        // enable shield visualizer
        shieldVisualizer.gameObject.SetActive(true);
    }


    // method to add 10 to score!
    // Communicate with the UI to update the score!
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }


}
