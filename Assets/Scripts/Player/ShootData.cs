// Monobehaviourを継承していないクラスでもinspectorで表示できるようにする属性

[System.Serializable]
public struct ShootData
{
    public float RotationSpeed;     // 回転スピード
    public float AddPower;          // 力を溜める速度
    public float MaxPower;          // 打つ力の最大値
    public float MinPower;          // 打つ力の最小値
}
