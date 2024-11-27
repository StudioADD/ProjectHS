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

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Player player))
        {
            // 부스터 아이템 카운트 줘야 함

            // 이펙트가 이 자리에 터지면서 삭제되야 함
        }
    }
}
