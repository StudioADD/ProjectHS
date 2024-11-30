using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterItem : BaseItem
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        itemType = EItemType.BoosterItem;

        return true;
    }

    public override void SetInfo(EItemType itemType)
    {

    }

    private void OnDestroyEffect()
    {
        ObjectCreator.SpawnEffect<BaseEffectObject>(EEffectType.ItemBurstEffect, this.transform.position);
        Managers.Resource.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Player player))
        {
            // 부스터 아이템 카운트 줘야 함
            player.GetBooster();
            OnDestroyEffect();
        }
    }
}
