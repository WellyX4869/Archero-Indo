using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineManager : MonoBehaviour
{
    public GameObject[] slotSkillObject;
    public Button[] slotButton;

    public Sprite[] skillSprite;

    [System.Serializable]
    public class DisplayItemSlot
    {
        public List<Image> slotSprite = new List<Image>();
    }

    public DisplayItemSlot[] displayItemSlots;
    public Image displayResultImage;

    public List<int> startList = new List<int>();
    public List<int> resultIndexList = new List<int>();
    int itemCount = 3;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < itemCount * slotButton.Length; i++)
        {
            startList.Add(i);
        }

        for (int i = 0; i < slotButton.Length; i++)
        {
            for (int j = 0; j < itemCount; j++)
            {
                slotButton[i].interactable = false;

                int randomIndex = Random.Range(0, startList.Count);

                if (i == 0 && j == 1 || i == 1 && j == 0 || i == 2 && j == 2)
                {
                    resultIndexList.Add(startList[randomIndex]);
                }

                displayItemSlots[i].slotSprite[j].sprite = skillSprite[startList[randomIndex]];

                if (j == 0)
                {
                    displayItemSlots[i].slotSprite[itemCount].sprite = skillSprite[startList[randomIndex]];
                }

                startList.RemoveAt(randomIndex);
            }
        }

        //StartCoroutine(StartSlot1());
        //StartCoroutine(StartSlot2());
        //StartCoroutine(StartSlot3());

        for(int i = 0; i < slotButton.Length; i++)
        {
            StartCoroutine(StartSlot(i));
        }
    }

    int[] answer = { 2, 3, 1  };
    IEnumerator StartSlot(int slotIndex)
    {
        for(int i = 0; i < (itemCount * (6 + slotIndex * 4) + answer[slotIndex]) * 2; i++)
        {
            slotSkillObject[slotIndex].transform.localPosition -= new Vector3(0, 50f, 0);
            if(slotSkillObject[slotIndex].transform.localPosition.y < 50f)
            {
                slotSkillObject[slotIndex].transform.localPosition += new Vector3(0, 300f, 0);
            }
            yield return new WaitForSeconds(0.02f);
        }

        for(int i = 0; i < itemCount; i++)
        {
            slotButton[i].interactable = true; 
        }
    }

    public void ClickButton(int index)
    {
        displayResultImage.sprite = skillSprite[resultIndexList[index]];
    }
}
