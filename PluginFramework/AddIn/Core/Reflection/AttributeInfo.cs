using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginFramework.AddIn.Reflection
{
    [Serializable]
    public class AttributeInfo
    {
        private Type owner;

        public Type Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        private Attribute value;

        public Attribute Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public AttributeInfo(Type type, Attribute attribute)
        {
            this.owner = type;
            this.value = attribute;
        }
    }
}
