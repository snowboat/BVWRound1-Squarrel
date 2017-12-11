
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRecord : MonoBehaviour {
    public static AttackRecord _instance;

    public static int handAttacking = 2;
    public static bool isAttacking = false;
    public static int handAttackCount = 0;
    public static int totalAttackBeforeBossFight = 3;
}
