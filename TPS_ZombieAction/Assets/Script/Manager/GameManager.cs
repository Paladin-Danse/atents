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

    //���ӸŴ����� ����� UIȿ��
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

    [SerializeField] private GetItem[] dropItem;//����Ǵ� ������ ���
    [SerializeField] [Range(0, 1)] private float f_DropPercent = 0.25f;//���Ȯ��(0.25 = 25%)

    private List<GetItem> dropItemList = new List<GetItem>();//����� ������ ���

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
        if (item) dropItemList.Add(item);//�ش� �����۸���Ʈ�� ��� ������ �������� Add
        item.gameObject.SetActive(false);//��� ������ �������� ��Ȱ��ȭ
        return item;
    }

    public void DropItem(Vector3 pos)
    {
        //�������� 0���� 1������ �Ҽ��� ������
        float Percentage = Random.Range(0f, 1f);

        //���� �������� ���� ��ġ�� ���Ȯ������ ���ٸ� ���Ȯ���� ���°����� �����Ͽ� �����Ͽ� �ִ� ������ �� �������� �ϳ��� ���
        if (Percentage < f_DropPercent)
        {
            int itemNum = Random.Range(0, dropItem.Length);//������ ������ ������ ����

            var item = dropItemList.Find(i =>
            {
                if (!i.gameObject.activeSelf && i.type.Equals(dropItem[itemNum].type))
                {
                    return true;
                }
                return false;
            });
            //���õ� �������� ����(dropItem[itemNum])�� Ű������ ��ü �����۸���Ʈ���� �ش� �����۸���Ʈ�� ������ ��,
            //Find�Լ��� ��Ȱ��ȭ�� ������Ʈ�� ������ �ӽú��� item�� �ִ´�.

            if (item == null)
            {
                item = MakeItem(dropItem[itemNum]);//��Ȱ��ȭ�� �������� ���°�� MakeItem �Լ��� �ҷ��� ���� �����ѵ� �ִ´�.
            }

            item.gameObject.SetActive(true);//������ �������� Ȱ��ȭ
            item.transform.position = pos + (transform.up * 0.6f);//������ ��ġ�� ����Ǿ�� �� ��ġ�� �̵�
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

    //�� �̵��� ó���� �Լ�
    public void LoadScene(string newSceneName)
    {
        //���� �ٲ� �� ���� �÷��̾ �����ϴ� ���̶�� Ȥ�� �� ��츦 ����� �÷��̾�� �ɾ���� ���� ���� �����Ѵ�.
        //if (player) player.GetComponent<PlayerHealth>().OffInvincibility();

        //���� ��ȯ�Ǳ����� ȭ�鿡 ���̵� ��/���̵� �ƿ�ȿ���� �ش�.
        FadeImage.gameObject.SetActive(true);

        StartCoroutine(FadeOut(newSceneName));
    }

    //���� �Ѿ �� ȭ�� ȿ��
    //���� �����
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
    //���� ��ο���
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