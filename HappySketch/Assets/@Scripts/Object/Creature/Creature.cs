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
        Rigid = Util.GetOrAddComponent<Rigidbody>(this.gameObject);
        animator = Util.GetOrAddComponent<Animator>(this.gameObject);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Collider = GetComponent<BoxCollider>();
        Rigid = GetComponent<Rigidbody>();
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

    protected void InitRigidVelocityY()
    {
        Rigid.velocity = new Vector3(Rigid.velocity.x, 0, Rigid.velocity.z);
    }

    protected void SetRigidVelocityY(float velocityY)
    {
        Rigid.velocity = new Vector3(Rigid.velocity.x, velocityY, Rigid.velocity.z);
    }

    protected void InitRigidVelocity()
    {
        Rigid.velocity = Vector3.zero;
    }
    #endregion

}
