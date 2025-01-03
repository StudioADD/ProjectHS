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

    protected Collider col;
    protected Rigidbody rigid;
    protected Animator animator;

    [SerializeField, ReadOnly]
    protected CollisionTrigger collisionTrigger = null;

    protected virtual void Reset()
    {
        rigid = Util.GetOrAddComponent<Rigidbody>(this.gameObject);
        animator = Util.GetOrAddComponent<Animator>(this.gameObject);

        collisionTrigger = Util.FindChild<CollisionTrigger>(this.gameObject, "CollisionTriggerObj", true);
        if (collisionTrigger == null)
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

        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;

        col = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();

        if (collisionTrigger != null)
        {
            collisionTrigger.OnCollisionTriggerEnter -= OnCollisionTriggerEnter;
            collisionTrigger.OnCollisionTriggerEnter += OnCollisionTriggerEnter;

            collisionTrigger.OnCollisionTriggerExit -= OnCollisionTriggerExit;
            collisionTrigger.OnCollisionTriggerExit += OnCollisionTriggerExit;
        }

        return true;
    }

    public abstract void SetInfo(int templateId);

    public float GetColliderHeight()
    {
        return Util.GetColliderHeight(col);
    }

    public virtual void OnCollisionTriggerEnter(Collider other)
    {

    }
    public virtual void OnCollisionTriggerExit(Collider other)
    {

    }
    #region Rigid
    protected Vector3 GetRigidVelocity()
    {
        return rigid.velocity;
    }

    protected void SetRigidVelocity(Vector3 velocity)
    {
        rigid.velocity = velocity;
    }

    protected void InitRigidVelocityX()
    {
        rigid.velocity = new Vector3(0, rigid.velocity.y, rigid.velocity.z);
    }

    protected void SetRigidVelocityX(float velocityX)
    {
        rigid.velocity = new Vector3(velocityX, rigid.velocity.y, rigid.velocity.z);
    }

    protected void InitRigidVelocityY()
    {
        rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
    }

    protected void SetRigidVelocityY(float velocityY)
    {
        rigid.velocity = new Vector3(rigid.velocity.x, velocityY, rigid.velocity.z);
    }

    protected void InitRigidVelocityZ()
    {
        rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
    }

    protected void SetRigidVelocityZ(float velocityZ)
    {
        rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y, velocityZ);
    }

    protected void InitRigidVelocity()
    {
        rigid.velocity = Vector3.zero;
    }
    protected void SetRigidbodyConstraints()
    {
        rigid.constraints = RigidbodyConstraints.FreezePositionX |
                             RigidbodyConstraints.FreezePositionZ |
                             RigidbodyConstraints.FreezeRotation;

    }
    #endregion

}
