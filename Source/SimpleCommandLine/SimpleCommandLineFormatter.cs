using System;
using System.Reflection;
using System.Text;

namespace SimpleCommandLine
{
    public class SimpleCommandLineFormatter
    {
        public static SimpleCommandLineFormatter Default { get; } = new SimpleCommandLineFormatter();

        public string FormatAbout<T>( string author, string description )
        {
            var assemblyName = Assembly.GetCallingAssembly().GetName();
            return FormatAbout<T>( assemblyName.Name, 
                $"{assemblyName.Version.Major}.{assemblyName.Version.Minor}.{assemblyName.Version.Revision}", 
                author, description );
        }

        public string FormatAbout<T>( string appName, string appVersion, string author, string description )
        {
            var aboutBuilder = new StringBuilder();
            aboutBuilder.AppendLine(
$@"
{appName} {appVersion} by {author} ({DateTime.Now.Year})
{description}

Usage:" );
            aboutBuilder.AppendLine( FormatUsage<T>( appName ) );
            return aboutBuilder.ToString();
        }

        public string FormatUsage<T>( string appName )
        {
            var usageBuilder = new StringBuilder();
            usageBuilder.AppendLine(
$@"{appName} <option> <args> [optional parameters]

Options:" );

            foreach ( var optionInfo in CommandLineOptionsHelper.EnumerateTypeOptionPropertyInfo( null, typeof( T ), null ) )
            {
                if ( optionInfo.FormattedShortName != null )
                {
                    usageBuilder.Append( optionInfo.FormattedShortName );
                    if ( optionInfo.FormattedLongName != null )
                        usageBuilder.AppendLine();
                }

                if ( optionInfo.FormattedLongName != null )
                    usageBuilder.Append( optionInfo.FormattedLongName );

                if ( optionInfo.OptionAttribute.ArgumentDescription != null )
                    usageBuilder.Append( $" <{optionInfo.OptionAttribute.ArgumentDescription}>" );

                if ( optionInfo.OptionAttribute.Required )
                    usageBuilder.Append( " (required)" );

                usageBuilder.AppendLine();

                if ( optionInfo.OptionAttribute.Description != null )
                {
                    usageBuilder.AppendLine( optionInfo.OptionAttribute.Description );

                    if ( optionInfo.OptionAttribute.DefaultValue != null )
                    {
                        usageBuilder.AppendLine( $"Default is {optionInfo.OptionAttribute.DefaultValue}." );
                    }
                }

                usageBuilder.AppendLine();
            }

            return usageBuilder.ToString();
        }
    }
}
