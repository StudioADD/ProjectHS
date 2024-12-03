using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[Serializable]
public class ItemParam { }

[Serializable]
public class CandyItemParam : ItemParam
{
    public ECandyItemType CandyItemType;

    public CandyItemParam(ECandyItemType candyItemType)
    {
        CandyItemType = candyItemType;
    }
}
