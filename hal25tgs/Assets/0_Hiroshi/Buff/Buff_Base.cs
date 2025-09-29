using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff_Base : MonoBehaviour
{
    public abstract void BuffActivate();    // バフ起動
    public abstract void BuffDeactivate();  // バフ終了
}
