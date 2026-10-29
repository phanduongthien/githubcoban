using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShooting : TowerAbstract
{
    [SerializeField] protected int currentFirePoint = 0;
    [SerializeField] protected float targetLoadSpeed = 1f;
    [SerializeField] protected float shootingSpeed = 0.5f;
    [SerializeField] protected float rotationSpeed = 200f;
    [SerializeField] protected EnemyCtrl target;
    [SerializeField] protected Bullet bullet;

   protected override void Start()
    {
        base.Start();
        Invoke(nameof(this.TargetLoading), targetLoadSpeed);
        Invoke(nameof(this.Shooting), shootingSpeed);
    }
    protected void FixedUpdate()
    {
        this.LookingAtTarget(); 
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
    }
    protected virtual void LookingAtTarget()
    {
        if (this.target == null) return;

        Vector3 direction = this.target.TowerTagettable.transform.position- this.towerCtrl.Rotator.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        this.towerCtrl.Rotator.rotation = Quaternion.RotateTowards(
            this.towerCtrl.Rotator.rotation,
            targetRotation,
            this.rotationSpeed * Time.fixedDeltaTime
        );
    }
    protected virtual void TargetLoading()
    {
        Invoke(nameof(this.TargetLoading), targetLoadSpeed);
        this.target =this.towerCtrl.TowerTageting.Nearest;
    }
   protected virtual void Shooting()
    {
        Invoke(nameof(this.Shooting), this.shootingSpeed);
      //  Debug.Log("Shooting");
        if (this.target == null)
        {
            Debug.Log("Target NULL");
            return;
        }
        //   Debug.Log("Spawn Bullet");
        FirePoint firePoint = this.getFirePoint();
      Bullet newBullet= this.towerCtrl.BullerSpawner.Spawn(this.towerCtrl.Bullet, firePoint.transform.position);
        Vector3 rotatorDirection = this.towerCtrl.Rotator.transform.forward;
        newBullet.transform.forward = rotatorDirection;
        newBullet.gameObject.SetActive(true);
    }
    protected virtual FirePoint getFirePoint()
    {
        FirePoint firePoint = towerCtrl.FirePoints[this.currentFirePoint];
        this.currentFirePoint++;
        if (this.currentFirePoint == this.towerCtrl.FirePoints.Count) this.currentFirePoint = 0;

        return firePoint;
    }

}
