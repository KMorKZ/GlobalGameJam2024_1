using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joke : MonoBehaviour
{
    public string jokeType;
    public int jokeIndex;
    public bool avaible;
    public int placement;
    public bool current = false;
    public int curSlot;
    public float cooldownTime;
    public float jokeRadius;
    public string animationName;
    public string audioClipName;
    public AudioSource audioS;
    public void Destruction()
    {
        //animation
        GetComponent<SphereCollider>().enabled = false;
        GetComponentInChildren<MeshRenderer>().enabled = false;
    }
    public void OnUse()
    {
        if(animationName == "isJoking")
        {
            audioS.Play();
            Debug.Log("audio");
        }
    }

}
