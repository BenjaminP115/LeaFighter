using UnityEngine;

[CreateAssetMenu(menuName = "LEA/EnemyData", fileName = "new EnemyData")]
public class EnemyData : ScriptableObject
{
    public float Health;
    public float Damage;
    public float AttackDistance;

    public int Xp;

    [Header("Movement")]
    public Vector2 WaitTime;
    public float MoveDistance;
    public float DetectionDistance;
    public float MovementSpeed;
    public float FollowSpeed;
}
