                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    Stat _stat;

    [SerializeField]
    float _scanRange = 10f;

    [SerializeField]
    float _attackRange = 2;

    public override void Init()
    {
        _stat = gameObject.GetComponent<PlayerStat>();

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);

    }

    protected override void UpdateIdle()
    {
        Debug.Log("Monster UpdateIdle");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)        
            return;

        float distance = (player.transform.position - transform.position).magnitude;
        if(distance <= _scanRange)
        {
            _lockTarget = player;
            State = Define.State.Moving;
            return;
        }
    }
    protected override void UpdateMoving()
        {
        //플레이어가 내 사정거리보다 가까우면 공격
        if (_lockTarget != null)
        {
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= _attackRange)
            {
                State = Define.State.Skill;
                return;
            }
        }

        //이동
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            nma.SetDestination(_destPos);
            nma.speed = _stat.MoveSpeed;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }
    protected override void UpdateSkill()
    {
        Debug.Log("Monster UpdateSkill");
    }

    void OnHitEvent()
    {
        Debug.Log("Monster OnHitEvent");
    }
}
