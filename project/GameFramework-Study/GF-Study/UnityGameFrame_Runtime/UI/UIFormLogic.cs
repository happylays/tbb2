
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public abstract class UIFormLogic : MonoBehaviour
    {
        public UIForm UiForm
        {
            get { return GetComponent<UiForm>(); }
        }

        public string name
        {
            get { return gameObject.name; }
            set
            {
                gameObject.name = value;
            }
        }

        public Transform CachedTransform
        {
            get;
            private set;
        }

        protected internal virtual void OnInit(object userData)
        {
            CachedTransform = transform;
        }
        protected internal virtual void OnOpen(object userData)
        {
            gameObject.SetActive(true);
        }

            
    }
}