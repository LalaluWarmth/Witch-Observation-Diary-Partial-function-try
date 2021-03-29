using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum FishState
{
    Wander,
    Nav,
    Eat,
    Dead//TODO
}

public class FishFSM : MonoBehaviour
{
    [HideInInspector]public FishState curFishState;
    private FishWander fishWander;
    private FishNav fishNav;
    private FishEat fishEat;

    private Transform _selfTrans;
    public float searchRadius;
    public LayerMask layerMask;

    private Collider2D[] _collider2Ds;
    [HideInInspector]public GameObject targetGO;
    private float _minDis;

    public Spline spline;
    private SplineNode[] _splineNodes;
    [SerializeField] private int _splineNodeIndex;
    [HideInInspector]public SplineNode nextNode;
    private List<Vector2> _splineNodesInitPos = new List<Vector2>();
    [Range(0, 1.3f)] public float randomPosRange;

    [HideInInspector]public bool isNaving;

    void Start()
    {
        _selfTrans = transform;
        curFishState = FishState.Wander;
        fishWander = GetComponent<FishWander>();
        fishNav = GetComponent<FishNav>();
        fishEat = GetComponent<FishEat>();
        _splineNodes = spline.SplineNodes;
        nextNode = _splineNodes[0];
        for (int i = 0; i < _splineNodes.Length; i++)
        {
            _splineNodesInitPos.Add(_splineNodes[i].Position);
        }

        isNaving = false;
    }


    void Update()
    {
        switch (curFishState)
        {
            case FishState.Wander:
                fishWander.MoveUpdate();
                break;
            case FishState.Nav:
                fishNav.NavToTarget(targetGO);
                break;
            case FishState.Eat:
                fishEat.EatAction(targetGO);
                break;
            case FishState.Dead://TODO
                break;
        }

        JudgeNextNode();
        if (!isNaving)
        {
            SearchForTarget();
        }
    }

    private void SearchForTarget()
    {
        _collider2Ds = Physics2D.OverlapCircleAll(transform.position, searchRadius, layerMask);
        _minDis = float.MaxValue;
        for (int i = 0; i < _collider2Ds.Length; i++)
        {
            Vector2 tempTargetPos = _collider2Ds[i].gameObject.transform.position;
            float dis = Vector2.Distance(transform.position, tempTargetPos);
            if (dis < _minDis)
            {
                targetGO = _collider2Ds[i].gameObject;
                _minDis = dis;
            }
        }

        if (targetGO)
        {
            curFishState = FishState.Nav;
        }
        else
        {
            curFishState = FishState.Wander;
        }
    }

    private void JudgeNextNode()
    {
        if (Vector2.Distance(nextNode.Position, _selfTrans.position) < 0.5f)
        {
            if (fishWander.moveDir)
            {
                _splineNodeIndex++;
                if (_splineNodeIndex >= _splineNodes.Length)
                {
                    _splineNodeIndex = 0;
                }

                nextNode = _splineNodes[_splineNodeIndex];
                RandomNodePosition();
            }
            else
            {
                _splineNodeIndex--;
                if (_splineNodeIndex < 0)
                {
                    _splineNodeIndex = _splineNodes.Length - 1;
                }

                nextNode = _splineNodes[_splineNodeIndex];
                RandomNodePosition();
            }
        }

        for (int i = 0; i < _splineNodes.Length; i++)
        {
            _splineNodesInitPos[i] = _splineNodes[i].Position;
        }
    }

    private void RandomNodePosition()
    {
        int tempIndex = _splineNodeIndex;
        int intervalIndex = _splineNodes.Length / 2 + 1;
        if (fishWander.moveDir)
        {
            tempIndex -= intervalIndex;
            if (tempIndex < 0)
            {
                tempIndex = _splineNodes.Length + tempIndex;
            }
        }
        else
        {
            tempIndex += intervalIndex;
            if (tempIndex >= _splineNodes.Length)
            {
                tempIndex = tempIndex - _splineNodes.Length;
            }
        }

        float xRange = Random.Range(-randomPosRange, randomPosRange);
        float yRange = Random.Range(-randomPosRange, randomPosRange);
        Vector2 newPos = new Vector2(_splineNodesInitPos[tempIndex].x + xRange,
            _splineNodesInitPos[tempIndex].y + yRange);
        _splineNodes[tempIndex].transform.DOMove(newPos, 1.5f);
    }
}