using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustStop : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody.Sleep();
            _rigidbody.useGravity = true;
        }
    }
}
