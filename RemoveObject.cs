using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveObject : MonoBehaviour
{
    [SerializeField] float _wholeTime = 3.5f;
    float _time;


    private void Update()
    {
        _time += Time.deltaTime;
        if(_time >= _wholeTime)
        {
            Destroy(gameObject);
        }
    }
}
