using System;
using System.ComponentModel;
using System.Globalization;

namespace TGE.SimpleCommandLine
{
    public class SimpleCommandLineParser
    {
        public static SimpleCommandLineParser Default { get; } = new SimpleCommandLineParser();

        public T Parse<T>( string[] args )
        {
            var options = Activator.CreateInstance<T>();
            var lookup = CommandLineOptionsHelper.BuildOptionNameLookup( options );

            for ( int i = 0; i < args.Length; i++ )
            {
                var opt = args[i];

                bool TryGetNextArg( out string arg )
                {
                    arg = null;

                    if ( ( i + 1 ) > ( args.Length - 1 ) )
                        return false;

                    arg = args[++i];
                    return true;
                }

                string GetNextArg()
                {
                    if ( !TryGetNextArg( out var arg ) )
                        throw new MissingRequiredArgumentException( $"Missing required argument for option: {opt}" ) { Option = opt };

                    return arg;
                }

                if ( !lookup.TryGetValue( opt, out CommandLineOptionsHelper.OptionInfo optionInfo ) )
                {
                    continue;
                }

                if ( optionInfo.PropertyInfo.PropertyType == typeof( bool ) )
                {
                    // Toggle
                    optionInfo.PropertyInfo.SetValue( optionInfo.Object, true );
                }
                else
                {
                    // Simple value assignment
                    var arg = GetNextArg();
                    var converter = TypeDescriptor.GetConverter(optionInfo.PropertyInfo.PropertyType);
                    var value =  converter.ConvertFromString(null, CultureInfo.InvariantCulture, arg);
                    optionInfo.PropertyInfo.SetValue( optionInfo.Object, value );
                }

                optionInfo.Assigned = true;
            }

            foreach ( var lookupKvp in lookup )
            {
                var objProperty = lookupKvp.Value;

                if ( !objProperty.Assigned && ( objProperty.OptionAttribute != null ) )
                {
                    if ( objProperty.OptionAttribute.Required )
                        throw new MissingRequiredOptionException( $"Missing required option: {lookupKvp.Key}" ) { Option = lookupKvp.Key };

                    if ( objProperty.OptionAttribute.DefaultValue != null )
                        objProperty.PropertyInfo.SetValue( objProperty.Object, objProperty.OptionAttribute.DefaultValue );
                }
            }

            return options;
        }
    }
}
