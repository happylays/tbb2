
namespace GameFramework.UI
{
    internal partial class UIManager
    {
        private sealed class UIFormInstanceObject : ObjectBase
        {
            private readonly object m_UIFormAsset;
            private readonly IUIFormHelper m_UIFormHelper;

            public UIFormInstanceObject(string name, object uiFormAsset, object uiFormInstance, IUIFormHelper helper)
                : base(name, uiFormInstance)
            {
                if (uiFormAsset)
                {

                }
                if (uiFormHelper)
                {

                }

                m_UIFormAsset = uiFormAsset;
                m_UIFormHelper = m_UIFormHelper;
            }

            protected internal override void Release()
            {
                m_UIFormHelper.ReleaseUIForm(m_UIFormHelper, Target);
            }
        }

    }