using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;
    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;
    [Header("Set Dynamically")]
    public float camZ;
    void Awake()
    {
        //Transform CameraTrans = transform.Find("Main Camera");
        //Camera = CameraTrans.gameObject;
        camZ = this.transform.position.z;
    }

    void FixedUpdate()
    {
       // if (POI == null) return;

        //获取兴趣点的位置
        //Vector3 destination = POI.transform.position;
        Vector3 destination;
        //如果兴趣点不存在，返回到P:[0,0,0]
        if(POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            //获取兴趣点的位置
            destination = POI.transform.position;
            //如果兴趣点是一个projectile实例，检查它是否已经停止
            if (POI.tag == "Projectile")
            {
                if(POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    //返回到默认视图
                    POI = null;
                    //在下一次时更新
                    return;
                }
            }
        }
        //限定x和y的最小值
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        //在摄像机当前位置和目标位置之间插值
        destination = Vector3.Lerp(transform.position, destination, easing);
        //保持destination.z的值为camZ,使摄像机距离足够远
        destination.z = camZ;
        //设置摄像机到目标位置
        transform.position = destination;
        //设置摄像机的投影面积，使地面始终处于画面中
        Camera.main.orthographicSize = destination.y + 10;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
