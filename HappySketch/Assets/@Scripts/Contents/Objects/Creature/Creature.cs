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

    protected Animator animator;

    protected virtual void Reset()
    {
        Collider = Util.GetOrAddComponent<BoxCollider>(this.gameObject);
        Collider.isTrigger = true;

        Rigid = Util.GetOrAddComponent<Rigidbody>(this.gameObject);
        Rigid.isKinematic = true;

        animator = Util.GetOrAddComponent<Animator>(this.gameObject);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Collider = GetComponent<BoxCollider>();
        Collider.isTrigger = true;

        Rigid = GetComponent<Rigidbody>();
        Rigid.isKinematic = true;

        animator = GetComponent<Animator>();

        return true;
    }

    public abstract void SetInfo(int templateId);
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
