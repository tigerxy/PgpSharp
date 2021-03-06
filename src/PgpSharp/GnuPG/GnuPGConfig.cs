﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace PgpSharp.GnuPG
{
    /// <summary>
    /// Contains configuration values for using GnuPG.
    /// </summary>
    public static class GnuPGConfig
    {
        /// <summary>
        /// The GPG executable path key for appSettings.
        /// </summary>
        public const string GnuPGExePathKey = "pgpsharp:GnuPGExePath";

        static readonly string __defaultGpgExePath = TryFindGpgPath();
        static string __gpgExePath;

        /// <summary>
        /// Gets or sets the GPG executable path.
        /// </summary>
        /// <value>
        /// The GPG executable path.
        /// </value>
        public static string GnuPGExePath
        {
            get { return string.IsNullOrEmpty(__gpgExePath) ? __defaultGpgExePath : __gpgExePath; }
            set
            {
                if (File.Exists(value))
                {
                    __gpgExePath = value;
                }
            }
        }

        private static string TryFindGpgPath()
        {
            // try config first, otherwise search typical gpg install folder
            var exe = ConfigurationManager.AppSettings[GnuPGExePathKey];

            if (!File.Exists(exe))
            {
                var folder = GetDefaultGpgInstallFolder();

                exe = Path.Combine(folder, "gpg2.exe");
                if (!File.Exists(exe))
                {
                    exe = Path.Combine(folder, "gpg.exe");
                    if (!File.Exists(exe))
                    {
                        return null;
                    }
                }
            }
            return exe;
        }

        private static string GetDefaultGpgInstallFolder()
        {
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Gnu\\GnuPG");
            if (IntPtr.Size == 8 && !Directory.Exists(folder))
            {
                // try x86 version on 64bit OS
                folder = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), "Gnu\\GnuPG");
            }
            return folder;
        }

    }
}
