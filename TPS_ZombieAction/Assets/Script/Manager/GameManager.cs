using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager instance
    {
        get
        {
            if (!m_instance)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }

    private bool b_GameClear = false;

    //게임매니저가 사용할 UI효과
    [SerializeField] private Canvas GameManagerUI;
    private Image FadeImage;
    [SerializeField] private float FadeInout_SecondTime;
    private Color FadeColor;

    [SerializeField] private GameObject player;
    public PlayerAttacks playerAttack { get; private set; }
    public PlayerHealth playerHealth { get; private set; }
    public PlayerInput playerInput { get; private set; }
    public PlayerItemLooting playerItemLooting { get; private set; }
    public PlayerMovement playerMovement { get; private set; }

    [SerializeField] private GameObject Player_MainWeapon;
    [SerializeField] private GameObject Player_SubWeapon;
    [SerializeField] private GameObject Player_MeleeWeapon;

    [SerializeField] private GetItem[] dropItem;//드랍되는 아이템 목록
    [SerializeField] [Range(0, 1)] private float f_DropPercent = 0.25f;//드랍확률(0.25 = 25%)

    private List<GetItem> dropItemList = new List<GetItem>();//드랍된 아이템 목록

    private void Awake()
    {
        Setup();
        FadeImage = GameManagerUI.transform.Find("PadeInout_Image").GetComponent<Image>();
        FadeImage.gameObject.SetActive(false);

        DontDestroyOnLoad(gameObject);
    }
    public void Setup()
    {
        player = GameObject.Find("Player");
        if (player == null) Debug.Log("player Null");
        else
        {
            playerAttack = player.GetComponent<PlayerAttacks>();
            if (playerAttack)
            {
                playerAttack.WeaponLoad(Player_MainWeapon, Player_SubWeapon, Player_MeleeWeapon);
            }
            playerHealth = player.GetComponent<PlayerHealth>();
            playerInput = player.GetComponent<PlayerInput>();
            if (playerInput == null) Debug.Log("playerInput Null");
            playerItemLooting = player.GetComponent<PlayerItemLooting>();
            playerMovement = player.GetComponent<PlayerMovement>();
        }

        if (playerMovement)
        {
            if (playerMovement.enabled == true) playerMovement.enabled = false;
        }
        if (playerAttack)
        {
            if (playerAttack.enabled == true) playerAttack.enabled = false;
        }

        if (SceneManager.GetActiveScene().name == "Map_v1")
        {
            b_GameClear = false;
        }

        if(dropItemList.Count > 0) dropItemList.Clear();
        /*
        for (int i = 0; i < dropItem.Length; i++)
        {
            MakeItem(dropItem[i]);
        }
        */
    }

    public void PlayerInfomation_Save(string m_MainWeapon, string m_SubWeapon, string m_MeleeWeapon)
    {
        Player_MainWeapon = Resources.Load<GameObject>(string.Format("Prefabs/{0}", m_MainWeapon));
        Player_SubWeapon = Resources.Load<GameObject>(string.Format("Prefabs/{0}", m_SubWeapon));
        Player_MeleeWeapon = Resources.Load<GameObject>(string.Format("Prefabs/{0}", m_MeleeWeapon));
        if (!Player_MainWeapon || !Player_SubWeapon || !Player_MeleeWeapon) Debug.Log("GameManager Error : Not Found WeaponName");
    }

    public GetItem MakeItem(GetItem m_Item)
    {
        var item = Instantiate(m_Item);
        if (item) dropItemList.Add(item);//해당 아이템리스트에 방금 생성한 아이템을 Add
        item.gameObject.SetActive(false);//방금 생성된 아이템은 비활성화
        return item;
    }

    public void DropItem(Vector3 pos)
    {
        //랜덤으로 0에서 1까지의 소수를 가져옴
        float Percentage = Random.Range(0f, 1f);

        //만약 랜덤으로 나온 수치가 드랍확률보다 낮다면 드랍확률에 들어온것으로 판정하여 드랍목록에 있는 아이템 중 랜덤으로 하나를 드랍
        if (Percentage < f_DropPercent)
        {
            int itemNum = Random.Range(0, dropItem.Length);//랜덤한 아이템 종류를 선택

            var item = dropItemList.Find(i =>
            {
                if (!i.gameObject.activeSelf && i.type.Equals(dropItem[itemNum].type))
                {
                    return true;
                }
                return false;
            });
            //선택된 아이템의 종류(dropItem[itemNum])를 키값으로 전체 아이템리스트에서 해당 아이템리스트를 가져온 뒤,
            //Find함수로 비활성화된 오브젝트를 가져와 임시변수 item에 넣는다.

            if (item == null)
            {
                item = MakeItem(dropItem[itemNum]);//비활성화된 아이템이 없는경우 MakeItem 함수를 불러와 새로 생성한뒤 넣는다.
            }

            item.gameObject.SetActive(true);//가져온 아이템을 활성화
            item.transform.position = pos + (transform.up * 0.6f);//아이템 위치를 드랍되어야 할 위치로 이동
        }
    }

    public void GameClear()
    {
        b_GameClear = true;
        
        if(!playerHealth)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
        playerHealth.OnInvincibility();
        if (!playerMovement)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
        playerMovement.enabled = false;
        if (!playerAttack)
        {
            playerAttack = player.GetComponent<PlayerAttacks>();
        }
        playerAttack.enabled = false;

        OnCursorVisible();
    }

    //씬 이동을 처리할 함수
    public void LoadScene(string newSceneName)
    {
        //씬이 바뀔 때 만약 플레이어가 존재하는 씬이라면 혹시 모를 경우를 대비해 플레이어에게 걸어놓은 무적 등을 해제한다.
        //if (player) player.GetComponent<PlayerHealth>().OffInvincibility();

        //씬이 전환되기전에 화면에 페이드 인/페이드 아웃효과를 준다.
        FadeImage.gameObject.SetActive(true);

        StartCoroutine(FadeOut(newSceneName));
    }

    //씬이 넘어갈 때 화면 효과
    //점점 밝아짐
    public IEnumerator FadeIn()
    {
        FadeImage.color = Color.black;
        FadeColor = Color.clear;

        var t = 0f;

        while (1 >= t)
        {
            t += Time.deltaTime / FadeInout_SecondTime;
            FadeImage.color = Color.Lerp(Color.black, FadeColor, t);

            yield return null;
        }

        if (playerMovement)
        {
            if (playerMovement.enabled == false) playerMovement.enabled = true;
        }
        if (playerAttack)
        {
            if (playerAttack.enabled == false)
            {
                playerAttack.enabled = true;
                playerAttack.EquipMainWeapon();
            }
        }

        FadeImage.gameObject.SetActive(false);
    }
    //점점 어두워짐
    public IEnumerator FadeOut(string newSceneName)
    {
        FadeImage.color = Color.clear;
        FadeColor = Color.black;

        var t = 0f;

        while (1 >= t)
        {
            t += Time.deltaTime / FadeInout_SecondTime;
            FadeImage.color = Color.Lerp(Color.clear, FadeColor, t);

            yield return null;
        }
        //yield return new WaitForSeconds(FadeInout_SecondTime);

        SceneManager.LoadScene(newSceneName);

        StartCoroutine(FadeIn());
    }

    public void OnCursorVisible()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void OffCursorVisible()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}