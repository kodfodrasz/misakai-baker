﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Baker.View;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Baker
{
    /// <summary>
    /// Represents a website project.
    /// </summary>
    public sealed partial class SiteProject
    {
        /// <summary>
        /// Gets the directory of this project.
        /// </summary>
        public DirectoryInfo Directory
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the site configuration.
        /// </summary>
        public SiteConfig Configuration
        {
            get; 
            private set;
        }

        /// <summary>
        /// Gets the asset provider for this project.
        /// </summary>
        public IAssetProvider Provider
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the view engine used by the project.
        /// </summary>
        public IViewEngine ViewEngine
        {
            get;
            private set;
        }

    }



}
