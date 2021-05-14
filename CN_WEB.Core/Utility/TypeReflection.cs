using System;
using System.Collections.Generic;
using System.Text;

namespace CN_WEB.Core.Utility
{
    public class TypeReflection
    {
        public object this[string property]
        {
            get
            {
                try
                {
                    _value = GetType().GetProperty(property).GetValue(this);
                    return _value;
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                GetType().GetProperty(property).SetValue(this, _value);
            }
        }

        private object _value;
    }
}
