using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineManager : MonoBehaviour
{
    public GameObject[] slotSkillObject;
    public Button[] slotButton;

    public List<Sprite> skillSprite = new List<Sprite>();

    [System.Serializable]
    public class DisplayItemSlot
    {
        public List<Image> slotSprite = new List<Image>();
    }

    public DisplayItemSlot[] displayItemSlots;

    public List<int> startList = new List<int>();
    public List<int> resultIndexList = new List<int>();
    public List<bool> includedList = new List<bool>();
    int itemCount = 3;

    public Image resultImage;

    private void OnEnable()
    {
        IncludedSkill();
        SlotMachineStart();
    }

    private void SlotMachineStart()
    {
        startList.Clear();
        int counter = 0;
        for (int i = 0; i < skillSprite.Count; i++)
        {
            if (!includedList[i])
            {
                startList.Add(i);
            }
            else
            {
                counter++;
                if (counter >= 3)
                {
                    startList.Add(i);
                }
            }
        }

        // GET 3 SKILL ANSWERS THAT STILL NOT REACHING LIMIT
        List<int> answerList = new List<int>();
        int answerFound = 0;
        while (answerFound < 3)
        {
            int randomIndex = Random.Range(0, startList.Count);
            if (includedList[startList[randomIndex]] == true)
            {
                answerList.Add(startList[randomIndex]);
                startList.RemoveAt(randomIndex);
                answerFound++;
            }
        }

        resultIndexList.Clear();
        for (int i = 0; i < slotButton.Length; i++)
        {
            for (int j = 0; j < itemCount; j++)
            {
                slotButton[i].interactable = false;

                int randomIndex = Random.Range(0, startList.Count);
                int index = startList[randomIndex];
                if (i == 0 && j == 1 || i == 1 && j == 0 || i == 2 && j == 2)
                {
                    index = answerList[i];
                    resultIndexList.Add(index);
                }
                else
                {
                    startList.RemoveAt(randomIndex);
                }

                displayItemSlots[i].slotSprite[j].sprite = skillSprite[index];

                if (j == 0)
                {
                    displayItemSlots[i].slotSprite[itemCount].sprite = skillSprite[index];
                }

            }
        }

        for (int i = 0; i < slotButton.Length; i++)
        {
            StartCoroutine(StartSlot(i));
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
        
        for(int i = 0; i<itemCount; i++)
        {
            slotButton[i].interactable = true;
        }
    }
    private void IncludedSkill()
    {
        includedList.Clear();
        for (int i = 0; i < skillSprite.Count; i++)
        {
            if (PlayerData.Instance.PlayerSkill[i] >= PlayerData.Instance.PlayerSkillLimit[i])
                includedList.Add(false);
            else includedList.Add(true);
        }
    }
    public void ClickButton(int index)
    {
        PlayerData.Instance.PlayerSkill[resultIndexList[index]] += 1;
        #region HANDLE MISCELLANOUS SKILLS
        if (resultIndexList[index] == 7) // Level Up Increase
        {
            FindObjectOfType<GameSession>().expAbsorbed *= 1.5f;
        }
        else if(resultIndexList[index] == 8) // Attack Boost
        {
            PlayerData.Instance.AttackBoost();
        }
        else if(resultIndexList[index] == 9) // Attack Speed Boost
        {
            PlayerTargeting.Instance.BoostAttackSpeed();
        }
        else if(resultIndexList[index] == 10) // Health Boost
        {
            PlayerHpBar.Instance.GetHpBoost();
        }
        else if (resultIndexList[index] == 10) // Health Boost
        {
            PlayerData.Instance.CriticalBoost();
        }
        #endregion
        FindObjectOfType<LevelHandler>().PlayerAfterLevelUp();
    }
}
