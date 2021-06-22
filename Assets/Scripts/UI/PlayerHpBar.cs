using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHpBar : MonoBehaviour
{
    public static PlayerHpBar Instance // singleton     
    {
        get
        {
            if ( instance == null )
            {
                instance = FindObjectOfType<PlayerHpBar> ( );
                if ( instance == null )
                {
                    var instanceContainer = new GameObject ( "PlayerHpBar" );
                    instance = instanceContainer.AddComponent<PlayerHpBar> ( );
                }
            }
            return instance;
        }
    }
    private static PlayerHpBar instance;

    public Transform player;
    public Slider hpBar;
    public float maxHp;
    public float currentHp;

    public GameObject HpLineFolder;

    public TMP_Text playerHpText;
    float unitHp = 200f;

    // Start is called before the first frame update
    void Start ()
    {
        float scaleX = (1000f / unitHp) / (maxHp / unitHp);
        HpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);

        foreach (Transform child in HpLineFolder.transform)
        {
            child.gameObject.transform.localScale = new Vector3(scaleX, 1, 1);
        }

        HpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
        hpBar.value = currentHp / maxHp;
        playerHpText.text = "" + currentHp;
    }

    public void GetHpBoost ( )
    {
        float increasedHp = Mathf.Round(0.2f * maxHp);
        maxHp += increasedHp;
        currentHp += increasedHp;
        float scaleX = ( 1000f / unitHp ) / ( maxHp / unitHp );
        HpLineFolder.GetComponent<HorizontalLayoutGroup> ( ).gameObject.SetActive ( false );

        foreach ( Transform child in HpLineFolder.transform)
        {
            child.gameObject.transform.localScale = new Vector3 ( scaleX, 1, 1 );
        }

        HpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
    }

    public void GetAttacked(int damage)
    {
        currentHp -= damage;
        if(currentHp <= 0)
        {
            currentHp = 0;
            FindObjectOfType<LevelHandler>().LoseGame();
        }
    }
}
