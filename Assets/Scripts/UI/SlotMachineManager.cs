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

    public List<int> startList = new List<int>();
    public List<int> resultIndexList = new List<int>();
    int itemCount = 3;

    public Image resultImage;

    private void OnEnable()
    {
        SlotMachineStart();
    }

    private void SlotMachineStart()
    {
        startList.Clear();
        for (int i = 0; i < itemCount * slotButton.Length; i++)
        {
            startList.Add(i);
        }

        resultIndexList.Clear();
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

        for (int i = 0; i < slotButton.Length; i++)
        {
            StartCoroutine(StartSlot(i));
            slotButton[i].interactable = true;
        }
    }

    int[] answer = { 2, 2, 2  };
    IEnumerator StartSlot(int slotIndex)
    {
        // SETUP FIRST POSITION
        float divider = slotIndex == 0 ? 1 : slotIndex;
        float substracter = slotIndex == 0 ? 1 : 0;
        slotSkillObject[slotIndex].transform.localPosition = new Vector3(0, 200f + ((1 - substracter) / divider) * 400f - (answer[slotIndex] % 3 * 200f), 0);

        for (int i = 0; i < (itemCount * (6 + slotIndex * 4) + answer[slotIndex]) * 2; i++)
        {
            slotSkillObject[slotIndex].transform.localPosition -= new Vector3(0, 200f, 0);
            if(slotSkillObject[slotIndex].transform.localPosition.y < 0f)
            {
                slotSkillObject[slotIndex].transform.localPosition += new Vector3(0, 600f, 0);
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void ClickButton(int index)
    {
        Debug.Log(resultIndexList[index]);
        //resultImage.sprite = skillSprite[resultIndexList[index]];
        PlayerData.Instance.PlayerSkill[resultIndexList[index]] += 1;
        FindObjectOfType<LevelHandler>().PlayerAfterLevelUp();
    }
}
