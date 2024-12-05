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

        if (Managers.Scene.CurrScene is GameScene gameScene)
        {
            int score;
            switch (CandyItemType)
            {
                case ECandyItemType.RedCandyItem:
                    score = 100;
                    break;
                case ECandyItemType.GreenCandyItem:
                    score = 300;
                    break;
                case ECandyItemType.BlueCandyItem:
                    score = 500;
                    break;
                case ECandyItemType.BoomCandyItem:
                    score = -500;
                    break;
                default:
                    score = -1;
                    break;
            }

            if(score != -1)
            {
                UIScoreTextParam param = new UIScoreTextParam(score,
                gameScene.GetTeamCamera(teamType).WorldToScreenPoint(transform.position),
                new Color(0, 0, 0, 255));

                Managers.UI.SpawnObjectUI<UI_ScoreText>(param, Managers.UI.SceneUI.transform);
            }
        }

        ObjectCreator.SpawnEffect<BaseEffectObject>(effectType, transform.position);
        Managers.Resource.Destroy(gameObject);
    }
}
