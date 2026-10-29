using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]

public class TowerTageting : SaiMonoBehaviour
{
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected SphereCollider sphereCollider;
    [SerializeField] protected EnemyCtrl nearest;
    public EnemyCtrl Nearest => nearest;
    [SerializeField] protected List<EnemyCtrl> enemies = new();

    [SerializeField] protected Transform origin;
    [SerializeField] protected LayerMask obstacleMask;
    protected void FixedUpdate()
    {
        this.FindNearest();
        this.RemoveDeadEnemy();
    }
    protected virtual void FindNearest()
    {
        float nearestDistance = Mathf.Infinity;
        float enemyDistance;
        this.nearest = null;
        foreach (EnemyCtrl enemyCtrl in this.enemies)
        {
            if (enemyCtrl == null) continue;
            if (!this.CanSeeTarget(enemyCtrl)) continue;
            enemyDistance = Vector3.Distance(transform.position,enemyCtrl.transform.position);
            if (enemyDistance < nearestDistance)
            {
                nearestDistance = enemyDistance;
                this.nearest = enemyCtrl;
            }
        }
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSphereCollider();
        this.LoadRigidBody();
        this.LoadOrigin();
    }
    protected virtual void LoadOrigin()
    {
        if (this.origin != null) return;
        this.origin = this.transform;
    }
    protected virtual void LoadSphereCollider()
    {
        if (sphereCollider != null) return;
        this.sphereCollider = GetComponent<SphereCollider>();
        this.sphereCollider.radius = 10f;
        this.sphereCollider.isTrigger = true;
    }
    protected virtual void LoadRigidBody()
    {
        if (rigid != null) return;
        this.rigid = GetComponent<Rigidbody>();
        this.rigid.useGravity = false;
    }
    public virtual void OnTriggerEnter(Collider collider)
    {
        Debug.Log("OnTriggerEnter" + collider.name);
        this.AddEnemy(collider);
    }
    protected virtual void OnTriggerExit(Collider collider)
    {
        this.RemoveEnemy(collider);
    }
    protected virtual void RemoveEnemy(Collider collider)
    {
        EnemyCtrl enemyCtrl = collider.GetComponentInParent<EnemyCtrl>();

        if (enemyCtrl == null) return;

        this.enemies.Remove(enemyCtrl);

        Debug.Log("Remove Enemy " + enemyCtrl.name);
    }
    /*   protected virtual void RemoveEnemy(Collider collider)
       {
           foreach (EnemyCtrl enemyCtrl in this.enemies)
           {
               if (collider.transform.parent == enemyCtrl.transform)
               {
                   this.enemies.Remove(enemyCtrl);
                   return;
               }
           }
       }*/
    public virtual void AddEnemy(Collider collider)
    {
        //  Debug.Log("Collider name = [" + collider.name + "]");
        EnemyCtrl enemyCtrl = collider.GetComponentInParent<EnemyCtrl>();
        if (enemyCtrl == null)
        {
            // Debug.Log("KHONG TIM THAY ENEMY CTRL");
            return;
        }
        if (this.enemies.Contains(enemyCtrl))
        {
            // Debug.Log("ENEMY DA CO TRONG LIST: " + enemyCtrl.name);
            return;
        }
        if (enemyCtrl.EnemyDameReceiver.IsDead()) return;
        this.enemies.Add(enemyCtrl);
        // Debug.Log("ADD THANH CONG: " + enemyCtrl.name);
    }
    protected virtual void RemoveDeadEnemy()
    {
        foreach (EnemyCtrl enemyCtrl in this.enemies)
        {
            if (enemyCtrl.EnemyDameReceiver.IsDead())
            {
                if (enemyCtrl == this.nearest) this.nearest = null;

                this.enemies.Remove(enemyCtrl);
                return;
            }
        }
    }
    protected virtual bool CanSeeTarget(EnemyCtrl target)
    {
        if (target == null || this.origin == null)
            return false;
        Vector3 direction = target.transform.position - this.origin.position;
        float distance = direction.magnitude;
        // Visualize Raycast rays in the Scene
        if (Physics.Raycast(this.origin.position,direction.normalized, 
            out RaycastHit hit, distance,obstacleMask))
        {
            Vector3 directionToCollider = hit.point - this.origin.position;
            float distanceToCollider = directionToCollider.magnitude;
            Debug.DrawRay(this.origin.position, directionToCollider.normalized * distanceToCollider, Color.red);
            return false;
        }
        Debug.DrawRay(this.origin.position, direction.normalized * distance, Color.green);
        return true;
    }
} 
    
    