using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UImanager : MonoBehaviour
{
    // BOOTS OF TRAVEL
    public GameObject[] mainSlots;
    public GameObject[] mainInventorySlots;
    public GameObject[] dadInventorySlots;
    public GameObject[] anecdoteInventorySlots;
    public GameObject[] darkInventorySlots;
    public GameObject[] danceInventorySlots;
    public Color32[] colors;
    [SerializeField] private GameObject inventoryHandler;
    [SerializeField] private Animator inventoryAnim;
    [SerializeField] private ThirdPersonController tpc;

    private bool isInventoryOpen = false;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isInventoryOpen)
            {
                //Open inventory
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                tpc.cursorIsLocked = false;
                inventoryHandler.SetActive(true);
                inventoryAnim.SetBool("Closed", false);

                isInventoryOpen = true;
            }
            else if (isInventoryOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                tpc.cursorIsLocked = true;
                inventoryHandler.SetActive(false); 
                inventoryAnim.SetBool("Closed", true);

                isInventoryOpen = false;
            }
        }
    }

    public void MainSlotUpdate(List<Joke> mainJokes, int index, bool isAdded)
    {
        // Fully rework this updating system to maintain deleting and adding things into
        Joke joke = mainJokes[index];

        if (isAdded)
        {
            if (mainJokes.Count == 1)
            {
                slotFiller(0, joke);
            }
            else if (mainJokes.Count == 2)
            {
                slotFiller(1, joke);
            }
            else if (mainJokes.Count == 3)
            {
                slotFiller(2, joke);
            }
            else if (mainJokes.Count == 4)
            {
                mainJokes[index - 1].curSlot = 3;
                slotFiller(2, joke);
            }
            else if (mainJokes.Count == 5)
            {
                mainJokes[index - 1].curSlot = 3;
                mainJokes[index - 2].curSlot = 3;
                slotFiller(2, joke);
            }
        }
        else if (!isAdded)
        {
            if (joke.curSlot == 3)
            {
                return;
            }
            else if (joke.curSlot == 0)
            {
                if(mainJokes.Count > index + 1)
                    slotFiller(0, mainJokes[index + 1]);

                if (mainJokes.Count > index + 2)
                    slotFiller(1, mainJokes[index + 2]);
            }

            else if (joke.curSlot == 1)
            {
                if (mainJokes.Count > index + 1)
                    slotFiller(1, mainJokes[index + 1]);
            }
            else if (joke.curSlot == 2)
            {
                slotFiller(2, mainJokes[index - 1]);
            }
            slotFiller(4, joke);

        }
    }
    
    public void MainSlotsSort(int curIndex, List<Joke> mainJokes)
    {
        if (mainJokes.Count == 0)
            return;
        mainSlots[0].GetComponentInChildren<TextMeshProUGUI>().text = "____";
        mainSlots[1].GetComponentInChildren<TextMeshProUGUI>().text = "____";
        mainSlots[2].GetComponentInChildren<TextMeshProUGUI>().text = "____";

        for (int i = 0; i < mainJokes.Count; i++)
        {
            mainJokes[i].curSlot = 3;

        }
        slotFiller(0, mainJokes[curIndex]);
        if (mainJokes.Count < 2)
            return;
        if(curIndex == 0)
        {
            slotFiller(1, mainJokes[curIndex + 1]);
            if (mainJokes.Count < 3)
                return;
            slotFiller(2, mainJokes[mainJokes.Count - 1]);
        }
        else if (curIndex == mainJokes.Count - 1)
        {
            slotFiller(1, mainJokes[0]);
            if (mainJokes.Count < 3)
                return;
            slotFiller(2, mainJokes[curIndex - 1]);
        }
        else
        {
            slotFiller(1, mainJokes[curIndex + 1]);
            if (mainJokes.Count < 3)
                return;
            slotFiller(2, mainJokes[curIndex - 1]);
        }
    }

    private void slotFiller(int index, Joke joke)
    {
        if(index < 4)
        {
            //mainSlots[index].GetComponent<Image>().color = ColorSelector(joke.jokeType);

            mainSlots[index].GetComponentInChildren<TextMeshProUGUI>().text = joke.jokeType + " " + joke.jokeIndex.ToString();

        }
        else
        {
            //mainSlots[joke.curSlot].GetComponent<Image>().color = ColorSelector("empty");
            mainSlots[joke.curSlot].GetComponentInChildren<TextMeshProUGUI>().text = "____";

        }
        joke.curSlot = index;
    }

    private Color32 ColorSelector(string jokeType)
    {
        if(jokeType == "Dad")
        {
            return colors[0];
        }
        else if (jokeType == "Anecdote")
        {
            return colors[1];
        }
        else if (jokeType == "Dark")
        {
            return colors[2];
        }
        else if(jokeType == "Dance")
        {
            return colors[3];
        }
        else
        {
            return colors[4];
        }
    }
}
