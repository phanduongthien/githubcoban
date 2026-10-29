using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerAbstract : SaiMonoBehaviour
{
    [SerializeField] protected TowerCtrl towerCtrl;
    public TowerCtrl TowerCtrl => towerCtrl;
    protected virtual void Start()
    {
        
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTowerCtrl();
    }
    protected virtual void LoadTowerCtrl()
    {
        if (this.towerCtrl != null) return;
        this.towerCtrl = transform.parent.GetComponent<TowerCtrl>();
        Debug.Log(transform.name + "LoadTowerControl", gameObject);
        Debug.Log(transform.name + "da load thanh cong", gameObject);
    }

}
