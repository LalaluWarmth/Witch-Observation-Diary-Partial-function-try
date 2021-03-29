using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishWander : MonoBehaviour
{
    public Spline spline;

    private Transform _selfTrans;
    public Transform renderTrans;
    private float passedTime;
    public float speed = 0.02f; //游动速度
    public float rotationOffset; //角度偏移
    public bool moveClockwise;//游动顺逆时针
    private WrapMode wrapMode = WrapMode.Loop;

    public bool moveDir; //判断游动方向
    private float _lastParam;
    private float _clampedParam;

    void Start()
    {
        passedTime = 0;
        _selfTrans = transform;
        if (!moveClockwise)
        {
            renderTrans.eulerAngles = Vector3.down * 90;
            _lastParam = -1;
        }
        else
        {
            renderTrans.eulerAngles = Vector3.down * -90;
            _lastParam = 2;
        }
    }

    public void MoveUpdate()
    {
        if (!moveClockwise)
        {
            passedTime += Time.deltaTime * speed;
        }
        else
        {
            passedTime -= Time.deltaTime * speed;
        }
        

        _clampedParam = WrapValue(passedTime, 0f, 1f, wrapMode); //根据类型计算归一化时间戳
        _selfTrans.rotation =
            spline.GetOrientationOnSpline(WrapValue(passedTime + rotationOffset, 0f, 1f, wrapMode));
        _selfTrans.position = spline.GetPositionOnSpline(_clampedParam);
        moveDir = JudgeMoveDir();
        _lastParam = _clampedParam;
    }

    private float WrapValue(float v, float start, float end, WrapMode wMode)
    {
        switch (wMode)
        {
            case WrapMode.Clamp:
            case WrapMode.ClampForever:
                return Mathf.Clamp(v, start, end);
            case WrapMode.Default:
            case WrapMode.Loop:
                return Mathf.Repeat(v, end - start) + start;
            case WrapMode.PingPong:
                return Mathf.PingPong(v, end - start) + start;
            default:
                return v;
        }
    }

    private bool JudgeMoveDir()
    {
        return _lastParam < _clampedParam;
    }
}