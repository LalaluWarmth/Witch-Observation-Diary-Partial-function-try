using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEat : MonoBehaviour
{
    private FishFSM fishFsm;

    void Start()
    {
        fishFsm = GetComponent<FishFSM>();
    }


    public void EatAction(GameObject TargetGO)
    {
        fishFsm.targetGO = null;
        Destroy(TargetGO);
        fishFsm.isNaving = false;
        fishFsm.curFishState = FishState.Wander;
    }
}