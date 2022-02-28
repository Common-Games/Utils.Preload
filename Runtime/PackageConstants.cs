using System;

using JetBrains.Annotations;

namespace CGTK.Utils.Preload
{
    [PublicAPI]
    internal static class PackageConstants
    {
        public const String PACKAGE_COMPANY = "com.common-games.";
        public const String PACKAGE_GROUP   = "utils.";
        
        public const String PACKAGE_NAME    = PACKAGE_COMPANY + PACKAGE_GROUP + "preload";
        public const String PACKAGE_PATH    = "Packages/" + PACKAGE_NAME + "/";
        
        public const String SETTINGS_FOLDER = "Assets/Settings/Resources/CGTK/";
        public const String SETTINGS_NAME   = "Utils.Preload.PreloadSettings.asset";
        public const String SETTINGS_PATH   = SETTINGS_FOLDER + SETTINGS_NAME;
    }
}
