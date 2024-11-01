using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECreatureType
{
    Player,
    Monster,

    Max
}

public abstract class Creature : BaseObject
{
    public ECreatureType CreatureType { get; protected set; }
    public BoxCollider Collider { get; private set; }
    protected Rigidbody Rigid { get; private set; }

    [SerializeField, ReadOnly]
    protected CollisionTrigger collisionTrigger = null;
    protected Animator animator;

    protected virtual void Reset()
    {
        Rigid = Util.GetOrAddComponent<Rigidbody>(this.gameObject);
        animator = Util.GetOrAddComponent<Animator>(this.gameObject);

        collisionTrigger = Util.FindChild<CollisionTrigger>(this.gameObject, "CollisionTriggerObj", true);
        if(collisionTrigger == null)
        {
            GameObject go = new GameObject();
            go.name = "CollisionTriggerObj";
            go.AddComponent<CollisionTrigger>();
            go.transform.parent = this.transform;
            this.transform.localPosition = Vector3.zero;
            collisionTrigger = go.GetComponent<CollisionTrigger>();
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Rigid = GetComponent<Rigidbody>();
        Rigid.freezeRotation = true;

        Collider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();

        if(collisionTrigger != null)
        {
            collisionTrigger.OnCollisionTiggerEnter -= OnCollisionTriggerEnter;
            collisionTrigger.OnCollisionTiggerEnter += OnCollisionTriggerEnter;
        }

        return true;
    }

    public abstract void SetInfo(int templateId);

    public virtual void OnCollisionTriggerEnter(Collider other)
    {

    }
    
    #region Rigid
    protected void SetRigidVelocity(Vector3 velocity)
    {
        Rigid.velocity = velocity;
    }

    protected void SetRigidVelocity(Vector2 velocity)
    {
        Vector3 veloctiy = new Vector3(velocity.x, Rigid.velocity.y, velocity.y);
        Rigid.velocity = veloctiy;
    }

    protected void InitRigidVelocityX()
    {
        Rigid.velocity = new Vector3(0, Rigid.velocity.y, Rigid.velocity.z);
    }

    protected void SetRigidVelocityX(float velocityX)
    {
        Rigid.velocity = new Vector3(velocityX, Rigid.velocity.y, Rigid.velocity.z);
    }

    protected void InitRigidVelocityY()
    {
        Rigid.velocity = new Vector3(Rigid.velocity.x, 0, Rigid.velocity.z);
    }

    protected void SetRigidVelocityY(float velocityY)
    {
        Rigid.velocity = new Vector3(Rigid.velocity.x, velocityY, Rigid.velocity.z);
    }

    protected void InitRigidVelocityZ()
    {
        Rigid.velocity = new Vector3(Rigid.velocity.x, 0, Rigid.velocity.z);
    }

    protected void SetRigidVelocityZ(float velocityZ)
    {
        Rigid.velocity = new Vector3(Rigid.velocity.x, Rigid.velocity.y, velocityZ);
    }

    protected void InitRigidVelocity()
    {
        Rigid.velocity = Vector3.zero;
    }
    #endregion

}
