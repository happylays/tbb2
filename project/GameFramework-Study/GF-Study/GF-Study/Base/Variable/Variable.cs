
namespace GameFramework
{
    class abstract class Variable {
        protected Variable() { }
        public abstract Type Type
        {
            get;
        }

        public abstract object GetValue();
        public abstract object SetValue(object value);
        public abstract void Reset();
    }

}