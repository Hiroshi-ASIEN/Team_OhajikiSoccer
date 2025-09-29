using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;


//======================================
//2025/06/24 �͏�@�ǋL
//======================================


[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform m_Ball;
    private Transform m_OwnGoal;
    private Transform m_OpponentGoal;
    private Rigidbody m_Rb;


    [Header("���")]
    [SerializeField] private GameObject m_Arrow;

    [Header("�G�l�~�[�p�����[�^�[")]
    [SerializeField] float m_MinSpeed = 5.0f;
    [SerializeField] float m_MaxSpeed = 8.0f;
    [SerializeField] float m_RandomAngle = 15.0f;
    [SerializeField] float m_BackThreshold = 0.1f;
    [SerializeField] float m_TurnSpeed = 10.0f;

    [Header("�u���b�N�p�����[�^�[")]
    [SerializeField] float m_BlockDistanceThreshold = 3.0f; 
    [SerializeField] float m_BlockChance = 0.7f;
    [SerializeField] float m_DiagonalDistanceTreshold = 8.0f;

    [Header("�V���[�e�B���O�ݒ�")]
    [SerializeField] float m_ShotSpeed = 10.0f;
    [SerializeField] float m_MaxCurveAngle = 30.0f;
    [SerializeField] float m_BlockCkeckRadius = 1.0f;

    Vector3 m_NextDirection;
    float m_NextSpeed;

    [HideInInspector]public bool m_IsItemChaser = false;
    [HideInInspector]public ItemObject m_TargeItem = null;

    //======================================
    //2025/06/24 �͏�@�ǋL

    //�G�l�~�[�𓝍�����`�[���I�u�W�F�N�g
    AITeamObject m_Team;
    
    //Boost�A�C�e���ɑΉ����邽�߂́A���x�̊���
    //Boost���g�p����Ƃ�����1.2f�ɕύX�����
    float m_SpeedRate = 1.0f;
    //======================================




    void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_Rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        m_Ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Transform>();
        m_OwnGoal = GameObject.FindGameObjectWithTag("OwnGoal").GetComponent<Transform>();
        m_OpponentGoal = GameObject.FindGameObjectWithTag("OpponentGoal").GetComponent<Transform>();

        if (m_Arrow)
        {
            m_Arrow.SetActive(false);
        }

    }


    private bool ErrorCheck()
    {
        if (!m_OwnGoal)
        {
            Debug.LogWarning("m_OwnGoal �� null �܂��͔j���ς݂ł�");
            return true;
        }
        if (!m_Ball)
        {
            Debug.LogWarning("m_OwnGoal �� null �܂��͔j���ς݂ł�");
            return true;
        }
        if (!m_OpponentGoal)
        {
            Debug.LogWarning("m_OwnGoal �� null �܂��͔j���ς݂ł�");
            return true;
        }

        return false;
    }

    private void Search()
    {
        m_Ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Transform>();
        m_OwnGoal = GameObject.FindGameObjectWithTag("OwnGoal").GetComponent<Transform>();
        m_OpponentGoal = GameObject.FindGameObjectWithTag("OpponentGoal").GetComponent<Transform>();
    }

    void Update()
    {

        if(IsTouchingBall())
        {
            ShootBall();
            Debug.Log(IsTouchingBall());
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 vel = m_Rb.velocity;
        Vector3 flatVel = new Vector3(vel.x, 0.0f, vel.z);

        if(flatVel.sqrMagnitude > 0.001f)
        {
            Quaternion target = Quaternion.LookRotation(flatVel.normalized, Vector3.up);

            m_Rb.MoveRotation(
                Quaternion.Slerp(m_Rb.rotation, target, m_TurnSpeed * Time.fixedDeltaTime));
        }

        
    }

    void LateUpdate()
    {
        if (m_Arrow && m_Arrow.activeSelf)
        {
            Vector3 flat = new Vector3(m_NextDirection.x, 0f, m_NextDirection.z).normalized;
            Quaternion arrowRot = Quaternion.FromToRotation(Vector3.left, flat);
            m_Arrow.transform.rotation = arrowRot;
        }

    }

    public void PrepareMove(ItemObject item)
    {
        m_IsItemChaser = (item != null);
        m_TargeItem = item;

        if(m_IsItemChaser && m_TargeItem != null)
        {
            Vector3 dir =  (m_TargeItem.transform.position - transform.position).normalized;
            m_NextDirection = dir;
        }
        else
        {
            //���̃^�[���̈ړ�����������
            Vector3 dir = DecideDirection();

            //�����_���p�x
            dir = Quaternion.Euler(0.0f, Random.Range(-m_RandomAngle, m_RandomAngle), 0.0f) * dir;

            m_NextDirection = dir.normalized;
        }

        //�����_���p���[
        m_NextSpeed = Random.Range(m_MinSpeed, m_MaxSpeed);

        if(m_Arrow)
        {
            m_Arrow.SetActive(true);
        }
    }

    public void Move()
    {
        if (m_Arrow)
        {
            m_Arrow.SetActive(false);
        }

        //======================================
        //2025/06/27 �͏�@�ǋL�@
        //�v�Z���̖����� * m_SpeedRate��ǋL
        m_Rb.velocity = m_NextDirection * m_NextSpeed * m_SpeedRate;
    }


    public void FacePlayer()
    {
        m_Rb.velocity = Vector3.zero;

        transform.rotation = Quaternion.LookRotation(Vector3.left, Vector3.up);
        
        m_Rb.MoveRotation(transform.rotation);

        if (m_Arrow)
        {
            m_Arrow.SetActive(false);
        }
    }

    //
    Vector3 DecideDirection()
    {
        if (ErrorCheck())
        {
            Search();
        }
        //�O�̔���(���ă{�[�����O�ɂ��邩)
        bool isFront = m_Ball.position.x < transform.position.x;
        bool isBehind = m_Ball.position.x > transform.position.x;

        if (isFront)
        {
            return (m_Ball.position - transform.position).normalized;
        }
        else if (isBehind) //��딻��i�{�[�������ɂ��邩�j
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            
            bool anyCollinear = false;

            if (ErrorCheck())
            {
                Search();
            }

            foreach(GameObject p in players)
            {
                Vector3 playerPos = p.transform.position;
                Vector3 toBallFromP = (m_Ball.position - playerPos).normalized;
                Vector3 toGoalFromP = (m_OwnGoal.position - playerPos).normalized;

                float angle = Vector3.Angle(toGoalFromP, toBallFromP);

                //�S�[���̒��S�_�ƃv���C���[��̈꒼����Ƀ{�[�������邩
                if (angle < m_BackThreshold * 180.0f)
                {
                    anyCollinear = true;
                    break;
                }
            }

            //�S�[���̒��S�ڊ|���Č��ɖ߂�
            if(!anyCollinear)
            {
                return (m_OwnGoal.position - transform.position).normalized;
            }

            float distToBall = Vector3.Distance(transform.position, m_Ball.position);
            
            if(distToBall > m_DiagonalDistanceTreshold)
            {
                return ComputeDiagonalBack();
            }

            if(distToBall < m_BlockDistanceThreshold)
            {
                if(Random.value <= m_BlockChance)
                {
                    return ComputeBlockBehind();
                }
                else
                {
                    return ComputeDiagonalBack();
                }
            }

            if (Random.value < 0.5f)
            {
                return ComputeBlockBehind();
            }
            else
            {
                return ComputeDiagonalBack();
            }

        }
        else
        {
            //�S�[���̒��S�ڊ|���Č��ɖ߂�
            return (m_OwnGoal.position - transform.position).normalized; ;
        }
    }

    private Vector3 ComputeDiagonalBack()
    {
        Vector3 toBallFromE = (m_Ball.position - transform.position).normalized;
        Vector3 prep = Vector3.Cross(Vector3.up, toBallFromE).normalized;
        float side = (Vector3.Dot(prep,toBallFromE) > 0.0f) ? -1.0f : 1.0f;
        Vector3 diagBack = (-transform.forward + side * transform.right).normalized;
        return diagBack;
    }

    private Vector3 ComputeBlockBehind()
    {
        Vector3 toGoalFromBall = (m_OwnGoal.position - m_Ball.position).normalized;

        float blockOffset = 2f;

        Vector3 blockPos = m_Ball.position + toGoalFromBall * blockOffset;

        return (blockPos - transform.position).normalized;
    }

    private bool IsTouchingBall()
    {
        return Vector3.Distance(transform.position, m_Ball.position) < 5.0f;
    }


    private void ShootBall()
    {
        Vector3 shootDir = ComputeShootingDirection();
        Rigidbody ballRb = m_Ball.GetComponent<Rigidbody>();

        ballRb.velocity = shootDir * m_ShotSpeed;
    }

    private Vector3 ComputeShootingDirection()
    {
        Vector3 baseDir = (m_OpponentGoal.position - m_Ball.position).normalized;

        Ray ray = new Ray(m_Ball.position, baseDir);
        float maxDistance = Vector3.Distance(m_Ball.position, m_OpponentGoal.position);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        Transform nearestBlocker = null;
        float nearestDist = Mathf.Infinity;

        foreach(GameObject e in players)
        {
            Vector3 playerPos = e.transform.position;

            float proj = Vector3.Dot(playerPos - m_Ball.position, baseDir);

            if(proj < 0.0f || proj > maxDistance)
            {
                continue;
            }

            Vector3 clostestPoint = m_Ball.position + baseDir * proj;
            float perpDist = Vector3.Distance(playerPos, clostestPoint);

            if(perpDist < m_BlockCkeckRadius && proj < nearestDist)
            {
                nearestDist = proj;
                nearestBlocker = e.transform;
            }
        }

        if(nearestBlocker == null)
        {
            return baseDir;
        }

        Vector3 prep = Vector3.Cross(Vector3.up, baseDir).normalized;

        Vector3 toBlocker = (nearestBlocker.position - m_Ball.position).normalized;
        float dotOnRightSide = Vector3.Dot(prep, toBlocker);
        float sign = dotOnRightSide > 0.0f ? 1.0f : -1.0f;

        Quaternion curveRot = Quaternion.Euler(0, sign * m_MaxCurveAngle, 0.0f);

        return (curveRot * baseDir).normalized;
    }



    //======================================
    //2025/06/24 �͏�@�ǋL
    //�`�[���̃Q�b�^�[�A�Z�b�^�[�iAITeamObject����Ă΂��j
    public void SetTeam(AITeamObject _team)
    {
        m_Team = _team;
    }
    public AITeamObject GetTeam() 
    { 
        return m_Team; 
    }


    //�X�s�[�h���[�g�̃Q�b�^�[�A�Z�b�^�[�iItem_Boost_Newest����Ă΂��j
    public void SetSpeedRate(float rate)
    {
        m_SpeedRate = rate;
    }
    public float GetSpeedRate()
    {
        return m_SpeedRate;
    }
    //======================================


}
