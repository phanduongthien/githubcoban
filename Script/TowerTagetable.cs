using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
public class TowerTagettable : SaiMonoBehaviour
{
    [SerializeField] protected SphereCollider sphereCollider;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSphereComponent();
    } 
    protected virtual void LoadSphereComponent()
    {
        if (sphereCollider != null) return;
        this.sphereCollider = GetComponent<SphereCollider>();
        this.sphereCollider.radius = 0.5f;
        this.sphereCollider.isTrigger = true;
    }
}
