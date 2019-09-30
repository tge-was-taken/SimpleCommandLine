using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCommandLine
{

    [AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false )]
    public sealed class GroupAttribute : Attribute
    {
        public string Name { get; }

        public GroupAttribute( string name )
        {
            Name = name;
        }
    }
}
