using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSet : MonoBehaviour
{
    public static EffectSet Instance // singleton
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EffectSet>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("EffectSet");
                    instance = instanceContainer.AddComponent<EffectSet>();
                }
            }
            return instance;
        }
    }

    private static EffectSet instance;

    [Header("Enemy")]
    public GameObject enemyDmgText;
    [SerializeField] AudioClip enemyDamagedSFX;

    [Header("Player")]
    [SerializeField] AudioClip playerDamagedSFX;
    [SerializeField] AudioClip playerGetExpSFX;

    [Header("Others")]
    [SerializeField] AudioClip winGameSFX;
    [SerializeField] AudioClip loseGameSFX;

    #region Enemy
    public void PlayEnemyDamagedSFX()
    {
        AudioSource.PlayClipAtPoint(enemyDamagedSFX, Camera.main.transform.position, 0.7f);
    }
    #endregion

    #region PLAYER
    public void PlayPlayerDamagedSFX()
    {
        AudioSource.PlayClipAtPoint(playerDamagedSFX, Camera.main.transform.position);
    }
    public void PlayPlayerGetExpSFX()
    {
        AudioSource.PlayClipAtPoint(playerGetExpSFX, Camera.main.transform.position);
    }
    #endregion

    #region OTHERS
    public void PlayWinGameSFX()
    {
        AudioSource.PlayClipAtPoint(winGameSFX, Camera.main.transform.position);
    }
    public void PlayLoseGameSFX()
    {
        AudioSource.PlayClipAtPoint(loseGameSFX, Camera.main.transform.position);
    }
    #endregion
}
