using System;

namespace SimpleCommandLine
{
    [AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false )]
    public sealed class OptionAttribute : Attribute
    {
        public string ShortName { get; }
        public string LongName { get; }
        public string ArgumentDescription { get; }
        public string Description { get; }
        public bool Required { get; set; }
        public object DefaultValue { get; set; }

        public OptionAttribute( string shortName, string longName, string description )
        {
            ShortName = shortName;
            LongName = longName;
            ArgumentDescription = null;
            Description = description;
        }

        public OptionAttribute( string shortName, string longName, string argumentDescription, string description )
        {
            ShortName = shortName;
            LongName = longName;
            ArgumentDescription = argumentDescription;
            Description = description;
        }
    }
}
