using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECandyItemType
{
    RedCandy,
    GreenCandy,
    BlueCandy,
    BoomCandy,
    StarCandy,
    Max
}

public class CandyItem : BaseItem
{
    Rigidbody rigid;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        rigid = GetComponent<Rigidbody>();

        return true;
    }

    public override void SetInfo(EItemType itemType)
    {
        rigid.velocity = new Vector3(0, 0, 5); // 임시
        StartCoroutine(CoDestroyCheck());
    }

    public void OnColliect()
    {
        // 파괴되고, 이펙트 생성
    }

    private IEnumerator CoDestroyCheck()
    {
        while (true)
        {
            if (this.transform.position.z < -660)
                break;

            yield return new WaitForSeconds(1);
        }

        Managers.Resource.Destroy(this.gameObject);
    }
}
