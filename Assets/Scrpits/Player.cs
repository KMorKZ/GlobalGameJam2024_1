using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<Joke> mainJokes = new List<Joke>();
    public List<Joke> inventoryJokes = new List<Joke>();
    [SerializeField] private int maxMainSlots;

    [SerializeField] private UImanager uimanager;
    [SerializeField] private ThirdPersonController tpc;
    private float currentcoolDown;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            Joke joke = other.GetComponent<Joke>();
            joke.Destruction();
            inventoryJokes.Add(joke);

            if(joke.jokeType == "Dad")
            {
                uimanager.dadInventorySlots[joke.jokeIndex - 1].GetComponent<InventorySlot>().JokeAdd(joke);
            }
            else if (joke.jokeType == "Anecdote")
            {
                uimanager.anecdoteInventorySlots[joke.jokeIndex - 1].GetComponent<InventorySlot>().JokeAdd(joke);
            }
            else if (joke.jokeType == "Dark")
            {
                uimanager.darkInventorySlots[joke.jokeIndex - 1].GetComponent<InventorySlot>().JokeAdd(joke);
            }
            else if (joke.jokeType == "Dance")
            {
                uimanager.danceInventorySlots[joke.jokeIndex - 1].GetComponent<InventorySlot>().JokeAdd(joke);
            }

            JokeAdd(joke);


        }
    }

    private void Update()
    {
        if (currentcoolDown > 0f)
        {
            currentcoolDown -= Time.deltaTime;
        }
        else currentcoolDown = 0f;
        
        if (currentcoolDown <= 0 && Input.GetKeyDown(KeyCode.Mouse1))
        {
            //Use main slot joke
            if (mainJokes.Count == 0) return;
            for (int i = 0; i < mainJokes.Count; i++)
            {
                if(mainJokes[i].curSlot == 0)
                {
                    //use this joke
                    JokeUse(mainJokes[i]);
                    break;
                }
            }
        }

        JokeSwap();
        // UI animation
    }
    private void JokeUse(Joke joke)
    {
        currentcoolDown = joke.cooldownTime;

        //JokeRadius
        //JokeAudio and Anim
        joke.OnUse();
        tpc.PlayAnimation(joke);
    }
    private void JokeSwap()
    {
        int currentJokeIndex = 0;
        for (int i = 0; i < mainJokes.Count; i++)
        {
            if (mainJokes[i].current == true)
            {
                currentJokeIndex = i;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentJokeIndex == 0)
            {
                mainJokes[currentJokeIndex].current = false;
                currentJokeIndex =  mainJokes.Count - 1;
                mainJokes[currentJokeIndex].current = true;
            }
            else
            {
                mainJokes[currentJokeIndex].current = false;
                currentJokeIndex -= 1;
                mainJokes[currentJokeIndex].current = true;
            }

            uimanager.MainSlotsSort(currentJokeIndex, mainJokes);
            Debug.Log("Moved to Left");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentJokeIndex == mainJokes.Count - 1)
            {
                mainJokes[currentJokeIndex].current = false;
                currentJokeIndex = 0;
                mainJokes[currentJokeIndex].current = true;
            }
            else
            {
                mainJokes[currentJokeIndex].current = false;
                currentJokeIndex += 1;
                mainJokes[currentJokeIndex].current = true;
            }

            uimanager.MainSlotsSort(currentJokeIndex, mainJokes);
            Debug.Log("Moved to Right");

        }
    }
    public void JokeAdd(Joke added)
    {
        if (mainJokes.Count == 0)
            added.current = true;
        if (mainJokes.Count < maxMainSlots)
            mainJokes.Add(added);
        else
            inventoryJokes.Add(added);

        uimanager.MainSlotUpdate(mainJokes, mainJokes.Count - 1, true);

    }
    public void JokeRemove(Joke joke)
    {
        uimanager.MainSlotUpdate(mainJokes, mainJokes.IndexOf(joke), false);
        mainJokes.Remove(joke);

    }
}
