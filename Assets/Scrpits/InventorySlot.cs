using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Sprite spriteIcon;
    [SerializeField] private Image spriteHolder;
    [SerializeField] private TextMeshProUGUI jokeText;
    [SerializeField] private int indexSlot;
    [SerializeField] private bool isMainInventorySlot;

    public Joke curJoke;
    private Player player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    private void Update()
    {
        if (isMainInventorySlot)
        {
            if (player.mainJokes.Count > indexSlot)
            {
                JokeAdd(player.mainJokes[indexSlot]);
            }
            else
            {
                ResetSlot();
            }
        }
    }
    public void OnPointerEnter()
    {
        if (jokeText.text != "")
            spriteHolder.sprite = spriteIcon;
    }
    public void OnPointerExit()
    {
            spriteHolder.sprite = null;
    }
    public void CurrentSlotAdd()
    {
        if (player.mainJokes.Count >= 5)
            return;
        bool toAdd = true;
        for(int i = 0; i <player.mainJokes.Count; i++)
        {
            if(player.mainJokes[i] == curJoke)
            {
                toAdd = false;
            }
        }
        if (toAdd)
        {
            curJoke.curSlot = 3;
            player.JokeAdd(curJoke);
            
        }
    }
    public void CurrentSlotRemove()
    {
        if (curJoke == null)
            return;
        player.JokeRemove(curJoke);
        ResetSlot();
    }
    public void ResetSlot()
    {
        curJoke = null;
        jokeText.text = "";
    }
    public void JokeAdd(Joke joke)
    {
        curJoke = joke;
        if (isMainInventorySlot)
        {
            jokeText.text = curJoke.jokeType + " " + curJoke.jokeIndex;
        }
        else
        {
            jokeText.text = curJoke.jokeIndex.ToString();
        }
    }
}
