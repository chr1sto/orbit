using Orbit.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.Generic
{
    public class GenericObject : Entity
    {
        public GenericObject(Guid id,DateTime createdOn, string type, string valueType, string value, bool visible)
        {
            Id = id;
            CreatedOn = createdOn;
            Type = type;
            ValueType = valueType;
            Value = value;
            Visible = visible;
        }

        protected GenericObject() { }

        public DateTime CreatedOn { get; private set; }
        public string Type { get; private set; }
        public string ValueType { get; private set; }
        public string Value { get; private set; }
        public bool Visible { get; private set; }
    }
}
