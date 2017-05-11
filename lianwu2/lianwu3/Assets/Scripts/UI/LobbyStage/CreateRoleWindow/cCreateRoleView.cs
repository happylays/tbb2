using UnityEngine;
using System;
using System.Collections;
using LoveDance.Client.Common;
using LoveDance.Client.Network;
using LoveDance.Client.Network.Login;
using LoveDance.Client.Logic.Ress;
using LoveDance.Client.Loader;

public class cCreateRoleView : View
{
    public override UIFlag UIID
    {
        get { return UIFlag.ui_createrole; }
    }

    //mvc
    private cCreateRoleClip _uiClip;

    private PlayerBase m_Male = null;

    [SerializeField]
    Camera m_ShowCamera = null;

    /// <summary>
    /// 创建角色界面人物属性
    /// </summary>
    [Serializable]
    class RoleCreate
    {
        [SerializeField]
        public GameLayer m_Layer = GameLayer.NONE;      //显示的layer
        [SerializeField]
        public Transform m_ShowRectTL = null;  //左上基准点
        [SerializeField]
        public Transform m_ShowRectBR = null; //右下基准点
        [SerializeField]
        public BoxCollider m_BoxCollider = null; //控制旋转碰撞
        [SerializeField]
        public GameObject m_EffectLight = null; //特效灯光
        [SerializeField]
        public UIImageButton m_BtnSelect = null; //人物选择按钮
        [SerializeField]
        public AudioClip m_AudioSelect = null; //人物选择音效

        [HideInInspector]
        public Light m_RoleLight = null;        //人物灯光
        [HideInInspector]
        public Light m_BackLight = null;     //人物后灯光
        [HideInInspector]
        public PlayerCreateStyle m_PlayerStyle = null; //人物style
    }

    [SerializeField]
    RoleCreate m_MaleRole = null;

    enum SkinType : byte
    {
        None,

        White,
        Yellow,
        Dark,
    }
    private SkinType m_SkinType = SkinType.White;

    void Start()
    {
        OnShowWnd();
    }

    public void show() 
    {
        //initView();
        //addEvent();
        //initData();
    }

    public void initView()
    {
        if (_uiClip == null)
        {
            _uiClip = new cCreateRoleClip(this.gameObject);            
        }
    }

    public void initData() { }

    public void hide() 
    {
        //view.hide();
        //view.hide();
        removeEvent();
    
    }

    public void update() { }
    
    public void addEvent() 
    {
        //UIEventListener.Get(_uiClip.btnOk).onClick = onEvent;

        //UIEventListener.Get(_uiClip.m_btnOk.gameObject).onClick = OnClickOk;
    }

    public void removeEvent() 
    {
        //UIEventListener.Get(_uiClip.m_btnOk.gameObject).onClick = null;

    }

    public void onEvent(GameObject go) 
    {
        //data = DynamicData.Get(id);
        //_uiClip.SetMethod(data);
    }

    public void OnClickOk(GameObject go)
    {
        //GameMsg_C2S_Login msg = new GameMsg_C2S_Login();        
        //NetworkMgr.SendMsg(msg);

        //NetworkMgr.DoMessage(GameMsgType.MSG_ACCOUNT_LoginResult);
    }
    
    // receive msg
    public void onMsg()//GameMsgBase msg) 
    {
        //baseMsg msg = msg;
    }
    
    public void staticMethod()
    {
        
    }

    // send msg
    public void dynamicMethod() 
    {
        //Network.send(msg);
    }

    public void destroy() 
    {
        //base.destroy();

        if (_uiClip != null)
        {
            _uiClip.destroy();            
        }
        _uiClip = null;
    }

    //old
    public void OnShowWnd()
    {
        initView();
        addEvent();
        initData();

        UICoroutine.uiCoroutine.StartCoroutine(LoadRole());
        
    }

    public void OnHideWnd()
    {
        removeEvent();
        //base.OnHideWnd();
    }

    IEnumerator LoadRole()
    {
        //yield return UICoroutine.uiCoroutine.StartCoroutine(ClientResourcesMgr.LoadClientResource());

        //yield return UICoroutine.uiCoroutine.StartCoroutine(UIMgr.ShowUIAsync(UIFlag.ui_activity, null));

        yield return UICoroutine.uiCoroutine.StartCoroutine(AnimationLoader.LoadRoleCreateAnimation());

        yield return UICoroutine.uiCoroutine.StartCoroutine(PreparePlayerModel());

        cStageManager.Instance.ChangeStage(cBaseStage.eSTAGE.eStage_Game);
    }

    IEnumerator PreparePlayerModel()
    {
        IEnumerator itor = null;
        if (m_Male == null)
        {
            m_Male = CreateRoleModel("Boy", true, (byte)m_SkinType);
            itor = CreateRolePhysics(m_Male, m_MaleRole);
            while (itor.MoveNext())
            {
                yield return null;
            }
        }
    }

    PlayerBase CreateRoleModel(string modelName, bool isBoy, byte skin)
    {
        BriefAttr attr = new BriefAttr();
        attr.m_strRoleName = modelName;
        attr.m_bIsBoy = isBoy;
        attr.m_nSkinColor = skin;

        //PlayerBase player = PlayerManager.CreateLogic(attr, true, null, null);
        PlayerBase player = PlayerManager.CreateMainPlayerLogic(attr);

        return player;
    }

    IEnumerator CreateRolePhysics(PlayerBase player, RoleCreate roleCreate)
    {
        if (player != null)
        {
            IEnumerator itor = player.CreatePhysics(false, PhysicsType.Player);
            while (itor.MoveNext())
            {
                yield return null;
            }

            player.CreateUIRoleCamera(roleCreate.m_ShowRectTL.position, roleCreate.m_ShowRectBR.position, m_ShowCamera, roleCreate.m_Layer);


            player.CurrentStyle = PlayerStyleType.Create;
        }
    }

}