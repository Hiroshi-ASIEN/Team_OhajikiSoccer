using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Ghost : Buff_Base
{
    [SerializeField] private GameManager_S m_GameManager;
    public override void BuffActivate()
    {
        Debug.Log("プレイターバフ：貫通");
        // 具体的な効果をこの下に記述
        GhostSwitch(true);
    }

    public override void BuffDeactivate()
    {
        GhostSwitch(false);
    }

    // プレイヤーの当たり判定貫通化スイッチ
    private void GhostSwitch(bool _switch)
    {
        for (int t = (int)TeamName.TeamA; t <= (int)TeamName.TeamB; t++)
        {
            TeamObject team = m_GameManager.GetTeamObject((TeamName)t);

            if (team == null)
            {
                Debug.LogError("m_Team が null");
                return;
            }

            var players = team.GetPlayersArray();

            if (players == null)
            {
                Debug.Log(((TeamName)t).ToString());

                Debug.LogError("GetPlayersArray() が null");
                return;
            }
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == null)
                {
                    Debug.LogError($"players[{i}] が null");
                    continue;
                }

                // プレイヤーの当たり判定を引数の値にする
                if (_switch)
                {
                    players[i].gameObject.layer = (int)LayerNum.Ghost;
                }
                else if (!_switch)
                {
                    players[i].gameObject.layer = (int)LayerNum.Player;
                }
            }
        }
    }

}
