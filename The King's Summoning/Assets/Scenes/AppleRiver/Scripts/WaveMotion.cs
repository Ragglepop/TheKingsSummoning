using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMotion : MonoBehaviour
{
    public float speed = 90.0f;
    public float amplitude = 0.0f;
    private float start_degree = 0.0f;
    private Vector3 start_position;

    // Start is called before the first frame update
    void Start()
    {
        start_position = transform.position;
        start_degree = Random.Range(0.0f, 360.0f);
    }

    // Update is called once per frame
    void Update()
    {
        float degree = start_degree + Time.time * speed;
        float x = start_position.x + Mathf.Sin(degree * Mathf.Deg2Rad) * amplitude;
        float y = start_position.y + Mathf.Cos(degree * Mathf.Deg2Rad) * amplitude;
        transform.position = new Vector3(x, y, start_position.z);
    }
}
