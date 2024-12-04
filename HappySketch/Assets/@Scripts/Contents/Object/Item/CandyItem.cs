using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECandyItemType
{
    RedCandyItem,
    GreenCandyItem,
    BlueCandyItem,
    BoomCandyItem,
    StarCandyItem,
    Max
}

public class CandyItem : BaseItem
{
    public ECandyItemType CandyItemType { get; protected set; }
    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public override void SetInfo(ItemParam param = null)
    {
        base.SetInfo(param);

        if(param is CandyItemParam candyItemParam)
        {
            itemType = EItemType.CandyItem;
            CandyItemType = candyItemParam.CandyItemType;
        }
        else
        {
#if DEBUG
            Debug.LogWarning("CandyItemParam is Null");
#endif
            Managers.Resource.Destroy(this.gameObject);
        }
    }

    public void OnCollected()
    {
        // 파괴되고, 이펙트 생성
    }
}
