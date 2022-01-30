using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    public Controller player;
    public Transform bot;
    public Transform top;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        Vector3 newPos = new Vector3(0f, 0f, -10f);
        if (player.moveLegs)
        {
            newPos.x = bot.position.x;
            newPos.y = bot.position.y;
        }
        else
        {
            newPos.x = top.position.x;
            newPos.y = top.position.y;
        }
        transform.position = newPos;
    }
}
