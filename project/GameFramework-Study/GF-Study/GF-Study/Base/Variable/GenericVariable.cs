
namespace GameFramework
{
    public abstract class Variable<T> : Variable {
        private T m_Value;

        protected Variable() {
            m_Value = default(T);
        }

        protected Variable()
        {
            m_Value = default(T);
        }
        public abstract Type Type
        {
            get
            {
                return typeof(Type);
            }
        }

        public T Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
            }
        }

        public override object GetValue()
        {
            return m_Value;
        }
        public override object SetValue(object value)
        {
            m_Value = (T)value;
        }
        public override void Reset()
        {
            m_Value = default(T);
        }
        public override string ToString()
        {
            return (m_Value != null) ? m_Value.ToString() : "<Null>";
        }
    }

}