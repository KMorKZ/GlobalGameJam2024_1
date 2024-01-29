using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Animator anim;
    public string NPCname;
    public bool isAI;
    public string[] neutralJokes;
    public string[] positiveJokes;
    public string[] negativeJokes;
    public int reputationGives;
    public int reputationTakes;
    public int repeatTimes;
    public int repeatThreshold;
    public bool isCollided;
    public Transform player;
    public ParticleSystem[] ps;

    private Quaternion rotation;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rotation = transform.rotation;
    }
    public int Reaction(string jokeType)
    {
        for(int i=0; i < neutralJokes.Length; i++)
        {
            if(neutralJokes[i] == jokeType)
            {
                ps[2].Play();
                return 0;
            }
        }
        for (int i = 0; i < positiveJokes.Length; i++)
        {
            if (positiveJokes[i] == jokeType)
            {
                repeatTimes += 1;
                if (repeatTimes <= repeatThreshold)
                {
                    ps[0].Play();
                    return reputationGives;
                }

            }
        }
        for (int i = 0; i < negativeJokes.Length; i++)
        {
            if (negativeJokes[i] == jokeType)
            {
                ps[1].Play();
                return reputationTakes;
            }
        }
        ps[2].Play();
        return 0;
    }
    public float DistanceToPlayer()
    {
        Vector3 dist = transform.position - player.position;
        Debug.Log(dist.magnitude);
        return dist.magnitude;
    }
    public void LookAtPlayer()
    {
        anim.SetBool("isLaughing", true);

        if (isAI)
            return;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }
    public void StopLooking()
    {
        anim.SetBool("isLaughing", false);

        if (isAI)
            return;
        transform.rotation = rotation;
    }
}
