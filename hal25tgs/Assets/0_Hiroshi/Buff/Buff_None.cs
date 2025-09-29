using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_None : Buff_Base
{
    // Start is called before the first frame update
    public override void BuffActivate()
    {
        Debug.Log("バフ：なし");
        // 具体的な効果をこの下に記述
    }

    public override void BuffDeactivate()
    {
    }
}