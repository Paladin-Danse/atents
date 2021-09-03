using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    public static GameMgr Instance { get; private set; }

    private Player player;
    GameObject[] turrets;

    private Bullet bulletPrefeb;
    private List<Bullet> listBullet;

    [SerializeField] private float spawnRate_Min = 0.3f;
    [SerializeField] private float spawnRate_Max = 0.8f;
    private float SpawnRate = 1f;
    private float checkTime = 0;
    private float timer = 0;

    private void Awake()
    {
        if(null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            return;
        }

        Destroy(gameObject);
    }

    private void Start()
    {
        Init();
    }
    void Update()
    {
        if(player && player.isLive)
        {
            checkTime += Time.deltaTime;

            timer += Time.deltaTime;
            UIMgr.Instance.Timer = timer;

            if(SpawnRate <= checkTime)
            {
                checkTime = 0;
                SpawnRate = Random.Range(spawnRate_Min, spawnRate_Max);

                SpawnBullet();
            }
        }
    }

    void Init()
    {
        SpawnRate = 1f;
        spawnRate_Max = 0.8f;
        timer = 0;

        UIMgr.Instance.OnPlay();
        player = FindObjectOfType<Player>();
        if (!player) player.Init();

        bulletPrefeb = Resources.Load<Bullet>("Prefabs/Sphere");

        turrets = GameObject.FindGameObjectsWithTag("Respawn");
        listBullet = new List<Bullet>();
        for(int i=0; turrets.Length > i; i++)
        {
            var bullet = MakeBullet();
        }
    }

    Bullet MakeBullet()
    {
        if (bulletPrefeb)
        {
            var bullet = Instantiate(bulletPrefeb);
            if (bullet && player)
            {
                bullet.EventHadleOnCollisionPlayer += player.OnDamaged;
                bullet.EventHadleOnCollisionPlayer += () => { UIMgr.Instance.GameOver(timer); };
            }
            if (bullet) listBullet.Add(bullet);
            return bullet;
        }

        return null;
    }

    void SpawnBullet()
    {
        if (0 >= turrets.Length) return;

        var bullet = listBullet.Find(b => !b.gameObject.activeSelf);
        if (!bullet) bullet = MakeBullet();

        if(bullet)
        {
            var pos_index = Random.Range(0, turrets.Length);
            var pos = turrets[pos_index].transform.position + Vector3.up * 1.5f;
            bullet.SetPosition(pos);

            var dir = (player.position - pos).normalized;
            dir.y = 0.2f;

            var force = Random.Range(3, 8);
            bullet.OnFire(dir, force * 100);
        }
    }
}
