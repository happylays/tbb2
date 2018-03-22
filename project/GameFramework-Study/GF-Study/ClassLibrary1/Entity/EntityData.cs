using UnityEngine;

namespace StarForce
{
    [Serializable]
    public abstract class EntityData
    {
        [SerializeField]
        private int m_Id = 0;
        private int m_TypeId = 0;
        private Vector3 m_Position = Vector3.zero;
        private Quaternion m_Rotation = Quaternion.identity;
    }
}