using System.Collections;
using UnityEngine;

namespace LoveDance.Client.Common
{
    public interface IScene
    {
        IEnumerator IEPlayerEnterScene(bool bNewStyle);

        void OnReleaseScene();
    }

    public interface ISpringCameraControl
    {
        Transform PlayerTran { set; }
    }

    public interface ISceneCamera
    {
        Camera TargetCamera { get; }

        Animation CameraAni { get; }

        Transform Trans { get; }
    }

    public interface IStandPlayerPosition
    {
        Transform[] PlayerPosition { get; }
    }

    public interface ISceneItem
    {
        byte DeviceID { get; }

        int ItemIndex { get; }

        Transform Center { get; }

        Transform TransCoupleEffect { get; }

        float Distance { get; }

        Vector3 CameraRotate { get; }

        IBehaviourOperation SceneBehaviour { get; }

        byte InviteType { get; set; }

        Transform Trans { get; }

        GameObject Go { get; }
    }

    public interface ISceneEventItem
    {
        byte EventID
        {
            get;
        }

        GameObject Go { get; }
    }

    public interface IBehaviourOperation
    {

        Transform GetPlayerMoveToTrans();

        string GetBoyAnimation();

        string GetGirlAnimation();

        string GetBoyCoupleAnimation(byte relationType);

        string GetGirlCoupleAnimation(byte relationType);

        ParticleSystem GetExpressLoveEffect();

        ParticleSystem GetShowLoveEffect();
    }
}

