using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetting", menuName = "Game/PlayerSetting")]
public class PlayerSetting : ScriptableObject
{
    public GameObject playerBallObject;
    public GameObject billiardCueObject;
    public ShootData shootData;
    public PlayerBall playerBall;
    public BilliardCue billiardCue;
}
