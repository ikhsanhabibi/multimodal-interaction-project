using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class Player : MonoBehaviour
{
    public float playerSpeed;
    private Rigidbody2D rb;
    private Vector2 playerDirection;
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    private bool voiceCommand = false;

	public GameObject bulletPrefab;
    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        actions.Add("left", Left);
        actions.Add("right", Right);
        actions.Add("shoot", Shoot);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += CaughtKeyword;
        keywordRecognizer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        // keyboard input is only accepted when there is no voice command
        if (!voiceCommand)
        {
            float directionX = Input.GetAxisRaw("Horizontal");
            playerDirection = new Vector2(0, directionX).normalized;

        }
    }


    void FixedUpdate(){
        rb.velocity = new Vector2(playerDirection.y * playerSpeed, 0 );
    }

    private void CaughtKeyword(PhraseRecognizedEventArgs speech)
    {
        voiceCommand = true;
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
        voiceCommand = false;
    }

    private void Left()
    {
        transform.Translate(-1, 0, 0);
    }

    private void Right()
    {
        transform.Translate(1, 0, 0);
    }

    private void Shoot()
    {
        
       	Instantiate(bulletPrefab, firePoint.position + new Vector3(0, (float)1.5, 0), firePoint.rotation);
    }
}
