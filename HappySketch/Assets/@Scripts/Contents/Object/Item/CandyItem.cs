using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECandyItemType
{
    RedCandyItem = 0,
    GreenCandyItem,
    BlueCandyItem,
    BoomCandyItem,
    StarCandyItem,
    Max
}

public class CandyItem : BaseItem
{
    [field: SerializeField, ReadOnly]
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

    public void OnCollected(Define.ETeamType teamType)
    {
        EEffectType effectType = (CandyItemType == ECandyItemType.BoomCandyItem) ?
            EEffectType.BoomCandyBurstEffect : EEffectType.CandyItemBurstEffect;

        ObjectCreator.SpawnEffect<BaseEffectObject>(effectType, transform.position);
        Managers.Resource.Destroy(gameObject);
    }
}
