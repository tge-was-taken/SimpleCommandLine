using System;
using System.Collections.Generic;
using System.Reflection;

namespace TGE.SimpleCommandLine
{
    internal class CommandLineOptionsHelper
    {
        public class OptionInfo
        {
            public object Object { get; }

            public PropertyInfo PropertyInfo { get; }

            public OptionAttribute OptionAttribute { get; }

            public GroupAttribute GroupAttribute { get; }

            public string FormattedShortName { get; }

            public string FormattedLongName { get; }

            public bool Assigned { get; set; }

            public OptionInfo( object obj, PropertyInfo propertyInfo, OptionAttribute optionAttribute, GroupAttribute groupAttribute, string formattedShortName, string formattedLongName )
            {
                Object = obj;
                PropertyInfo = propertyInfo;
                OptionAttribute = optionAttribute;
                GroupAttribute = groupAttribute;
                FormattedShortName = formattedShortName;
                FormattedLongName = formattedLongName;
            }
        }
        public static Dictionary<string, OptionInfo> BuildOptionNameLookup( object parentObj, string path = null )
        {
            var lookup = new Dictionary<string, OptionInfo>();
            foreach ( var item in EnumerateTypeOptionPropertyInfo( parentObj, parentObj.GetType(), path ) )
            {
                lookup[item.FormattedShortName] = item;
                lookup[item.FormattedLongName] = item;
            }

            return lookup;
        }

        public static IEnumerable<OptionInfo> EnumerateTypeOptionPropertyInfo( object obj, Type type, string path )
        {
            var properties = type.GetProperties();
            foreach ( var property in properties )
            {
                var groupAttrib = property.GetCustomAttribute<GroupAttribute>();
                if ( groupAttrib != null )
                {
                    object groupObj = null;
                    if ( obj != null )
                    {
                        groupObj = property.GetValue( obj );
                        if ( groupObj == null )
                        {
                            groupObj = Activator.CreateInstance( property.PropertyType );
                            property.SetValue( obj, groupObj );
                        }
                    }

                    var groupOptions = EnumerateTypeOptionPropertyInfo( groupObj, property.PropertyType, path != null ?
                        path + "-" + groupAttrib.Name :
                        groupAttrib.Name );

                    foreach ( var item in groupOptions )
                        yield return item;

                    continue;
                }

                yield return GetOptionPropertyInfo( property, obj, path, groupAttrib );
            }
        }

        public static OptionInfo GetOptionPropertyInfo( PropertyInfo property, object parentObj, string path, GroupAttribute groupAttrib )
        {
            string shortName = null;
            string longName = null;

            var optionAttrib = property.GetCustomAttribute<OptionAttribute>();
            if ( optionAttrib != null )
            {
                shortName = optionAttrib.ShortName;
                longName = optionAttrib.LongName;
            }
            else
            {
                shortName = null;
                longName = property.Name.ToLower();
            }

            var optionPath = string.Empty;
            if ( path != null )
                optionPath = path + "-";

            if ( shortName != null )
                shortName = "-" + optionPath + shortName;

            if ( longName != null )
                longName = "--" + optionPath + longName;

            return new OptionInfo( parentObj, property, optionAttrib, groupAttrib, shortName, longName );
        }
    }
}
