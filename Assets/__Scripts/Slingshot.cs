using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static public Slingshot S;

    //在Unity检视面板设置的字段
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f;

    //动态设置的字段
    [Header("Set Dynamically")]
    public GameObject lauchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    private Rigidbody projectileRigidbody;

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }

    void Awake()
    {
        S = this;

        Transform launchPointTrans = transform.FindChild("LaunchPoint");
        lauchPoint = launchPointTrans.gameObject;
        lauchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }
    void OnMouseEnter()
    {
        print("Slingshot:OnMouseEnter()");
        lauchPoint.SetActive(true);

    }
    void OnMouseExit()
    {
        print("Slingshot:OnMouseExit()");
        lauchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        //玩家在鼠标光标悬停在弹弓上分时按下了鼠标左键
        aimingMode = true;
        //实例化一个弹丸
        projectile = Instantiate(prefabProjectile) as GameObject;
        //该示例的初始位置位于launchPoint处
        projectile.transform.position = launchPos;
        //设置当前的isKinematic属性
        projectileRigidbody=projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!aimingMode) return;

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //计算launchPos到mousePos3D两点间的坐标差
        Vector3 mouseDelta = mousePos3D - launchPos;
        //将mouseDelta坐标差限制在弹弓的球形碰撞器范围内
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude>maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        
        //将projectile移动到新位置
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
        }
    }
}
