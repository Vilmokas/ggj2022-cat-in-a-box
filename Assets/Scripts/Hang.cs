using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hang : MonoBehaviour
{

    public bool bar = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Hang")
        {
            bar = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Hang")
        {
            bar = false;
        }
    }
}
