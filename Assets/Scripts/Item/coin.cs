using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    public int maxBounce;//ÆÃ±â´Â È½¼ö
    public float xForce;
    public float yForce;
    public float gravity;

    private Vector2 direction;
    private int currentBounce = 0;
    private bool isGrounded;


    private float maxHeight;
    private float currentheight;
    public Transform sprite;
    public Transform shadow;

    private float time=0;

    private void Start()
    {
        currentheight = Random.Range(yForce - 1, yForce);
        maxHeight = currentheight;
        Initialize(new Vector2(Random.Range(-xForce, xForce), Random.Range(-xForce, xForce)));
    }

    private void OnEnable()
    {
        currentheight = Random.Range(yForce - 1, yForce);
        maxHeight = currentheight;
        Initialize(new Vector2(Random.Range(-xForce, xForce), Random.Range(-xForce, xForce)));
    }


    private void Update()
    {
        //original form --> if (!isGrounded)
        if (time<1.5f && !isGrounded)
        {
            time += Time.deltaTime;

            currentheight += -gravity * Time.deltaTime;
            sprite.position += new Vector3(0, currentheight, 0) * Time.deltaTime;
            transform.position += (Vector3)direction * Time.deltaTime;

            float totalVelocity = Mathf.Abs(currentheight) + Mathf.Abs(maxHeight);
            float scaleXY = Mathf.Abs(currentheight) / totalVelocity;
            //shadow.localScale = Vector2.one * Mathf.Clamp(scaleXY, 0.5f, 1.0f);
            CheckGroundHit();


        }

    }


    void Initialize(Vector2 _direction)
    {
        isGrounded = false;
        maxHeight /= 1.5f;
        direction = _direction;
        currentheight = maxHeight;
        currentBounce++;
        time = 0;
    }

    void CheckGroundHit() {

        if (sprite.position.y < shadow.position.y)
        {
            sprite.position = shadow.position;
            //shadow.localScale = Vector2.one;

            if (currentBounce < maxBounce) { Initialize(direction / 1.5f); }
            else
            {
                isGrounded = true;
               
            }

            if (GetComponent<AfterEffect>() != null)
            { StartCoroutine(GetComponent<AfterEffect>().StartAfterEffect()); }

        }


    }

}
