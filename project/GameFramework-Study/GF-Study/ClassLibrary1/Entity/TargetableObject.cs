using GameFramework;
using UnityEngine;

namespace StarForce
{
    public abstract class TargetableObject : Entity
    {
        [SerializeField]
        private TargetableObjectData m_TargetableObjectData = null;

        public bool IsDead
        {
            get
            {
                return m_TargetableObjectData.HP <= 0;
            }
        }

        public abstract ImpactData GetImpactData();

        public void ApplyDamage(Entity attacker, int damageHP)
        {
            float fromHPRatio = m_TargetableObjectData.HPRatio;
            m_TargetableObjectData.HP -= damageHP;
            GameEntry.HPBar.ShowHPBar(this, fromHPRatio, toHPRatio);

            OnDead(attacker);
        }

        private void OnTriggerEnter(Collider other)
        {
            Entity entity = other.gameObject.GetComponent<Entity>();

            AIUtility.PerformCollision(this, entity);
        }
    }
}