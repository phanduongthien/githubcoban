using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCtrl : SaiMonoBehaviour
{
    [SerializeField] protected Transform model;
    [SerializeField] protected Transform rotator;
    public Transform Rotator => rotator;
    [SerializeField] protected TowerTageting towerTageting;
    public TowerTageting TowerTageting => towerTageting;
    [SerializeField] protected BullerSpawner bulletSpawner;
    public BullerSpawner BullerSpawner => bulletSpawner;
    [SerializeField] protected Bullet bullet;
    [SerializeField] protected string bulletName="Bullet";
    public Bullet Bullet => bullet;

    [SerializeField] protected BulletPrefab bulletPrefab;
    public BulletPrefab BulletPrefab => bulletPrefab;

    [SerializeField] protected List<FirePoint> firePoints = new();
    public List<FirePoint> FirePoints => firePoints;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadModel();
        this.LoadTowerTargeting();
        this.LoadBullerSpawner();
        this.LoadFirePoints();
        this.LoadBulletPrefab(); // Tìm BulletPrefab trước
        this.LoadBullet();       // Sau đó mới tìm Bullet
    }
    protected override void Awake()
    {
        base.Awake();
        this.hidePrefabs();
    }
    protected virtual void LoadModel()
    {
        if (this.model != null) return;
        this.model = transform.Find("Model");
        this.rotator = this.model.Find("Rotator");
        Debug.Log(transform.name + "Load model", gameObject);
    }
    protected virtual void LoadBullet()
    {
        if (bullet != null) return;
        this.bullet=this.bulletPrefab.getByName(this.bulletName);        
        Debug.Log(transform.name + "LoadBuller", gameObject);
    }
    protected virtual void LoadTowerTargeting()
    {
        if (this.towerTageting != null) return;
        this.towerTageting = transform.GetComponentInChildren<TowerTageting>();
        this.towerTageting.transform.localPosition = new Vector3(0,0.5f,0);
        Debug.Log(transform.name + "LoadTowerTargeting", gameObject);
    }
    protected virtual void LoadBullerSpawner()
    {
        if (bulletSpawner != null) return;
        this.bulletSpawner = FindObjectOfType<BullerSpawner>();
        Debug.Log(transform.name + "LoadBullerSpawner", gameObject);
    }
    protected virtual void LoadFirePoints()
    {
        if (this.firePoints.Count > 0) return;
        this.firePoints.AddRange(transform.GetComponentsInChildren<FirePoint>());
    }
    protected virtual void hidePrefabs()
    {
        this.bullet.gameObject.SetActive(false);
    }
    protected virtual void LoadBulletPrefab(){

        if (bulletPrefab != null) return;
        this.bulletPrefab = GameObject.FindAnyObjectByType<BulletPrefab>();
        Debug.Log(transform.name + "LoadBulletPrefab", gameObject);
        this.LoadBullet();
    }
}
