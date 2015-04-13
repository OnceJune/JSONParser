using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONParser
{
    public class JSON
    {
        private JSON parent;
        private JSON previous;
        private JSON next;
        private List<JSON> children;
        
        private string name;
        private ValueType valueType;
        private string value;

        public JSON Parent { get { return this.parent; } set { this.parent = value; } }
        public JSON Previous { get { return this.previous; } set { this.previous = value; } }
        public JSON Next { get { return this.next; } set { this.next = value; } }
        private bool boolValue { get { if (this.value.ToLower().Equals("true")) return true; else return false; } }
        private double digitValue { get { return double.Parse(this.value); } }

        public JSON Find(string name)
        {
            foreach (JSON child in this.children)
                if (child.name.Equals(name)) return child;

            return null;
        }

        public T GetValue<T>()
        {
            switch (this.valueType)
            {
                case ValueType.IsString:
                    return (T)(object)this.value;
                case ValueType.IsBool:
                    return (T)(object)this.boolValue;
                case ValueType.IsNull:
                    return (T)(object)null;
                case ValueType.IsObject:
                    return (T)(object)this.children;
                default:
                    break;
            }

            throw new Exception(string.Format("Item {0} is array and cannot be returned as a value", this.name));
        }

        public JSON this[int index]
        {
            get
            {
                if (index < this.children.Count && index >= 0) return this.children[index];
                else throw new Exception("Index over range");
            }
        }

        public void Append(JSON node)
        {
            this.children.Add(node);
        }

        public void Drop(JSON node)
        {
            if (this.children.Contains(node)) this.children.Remove(node);
        }
    }

    public static enum ValueType
    {
        IsString,
        IsBool,
        IsDigit,
        IsArray,
        IsObject,
        IsNull,
    }
}
