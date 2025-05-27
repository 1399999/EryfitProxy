namespace EryfitProxy.Kernel.Tools.DocGen
{
    public class ActionDescriptionLine
    {
        public ActionDescriptionLine(string propertyName, string type, string description, string defaultValue)
        {
            PropertyName = propertyName;
            Type = type;
            Description = description;
            DefaultValue = defaultValue;
        }

        public string PropertyName { get;  }

        public string Type { get;  } 

        public string Description { get;  }

        public string DefaultValue { get;  }
    }
}
