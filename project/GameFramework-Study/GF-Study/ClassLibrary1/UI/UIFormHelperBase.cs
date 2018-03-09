
using GameFramework.UI;
using UnityEngine;

public abstract class UiFormHelperBase : MonoBehaviour, IUIFormHelper
{
    public abstract object InstantiateUIForm(object uiFormAsset);
    public abstract IUIForm CreateUIForm(object uiFormInstance, IUIGroup uig, object userData);
    public abstract void ReleaseUIForm(object uiFormAsset, object uiFormInstance);
}