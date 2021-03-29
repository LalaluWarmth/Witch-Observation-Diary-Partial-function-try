using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FishNav : MonoBehaviour
{
    private FishWander fishWander;

    private FishFSM fishFsm;
    private Transform _selfTrans;
    private float _degree;


    void Start()
    {
        fishFsm = GetComponent<FishFSM>();
        fishWander = GetComponent<FishWander>();
        _selfTrans = transform;
    }

    public void NavToTarget(GameObject targetGO)
    {
        if (!fishFsm.isNaving)
        {
            fishFsm.isNaving = true;
            fishFsm.nextNode.transform.DOMove(targetGO.transform.position, 1f);
        }


        if (Vector2.Distance(targetGO.transform.position, _selfTrans.position) < 0.5f)
        {
            OnArrival();
        }

        if (Vector2.Distance(targetGO.transform.position, _selfTrans.position) > fishFsm.searchRadius+0.5f)
        {
            fishFsm.targetGO = null;
            fishFsm.isNaving = false;
            fishFsm.curFishState = FishState.Wander;
        }

        fishWander.MoveUpdate();
    }

    private void OnArrival()
    {
        fishFsm.curFishState = FishState.Eat;
    }
}