﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Baker.Providers;
using Baker.Text;
using Baker.View;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Baker
{
    /// <summary>
    /// Represents a website project.
    /// </summary>
    public sealed partial class SiteProject : IDisposable
    {
        #region Private Fiels
        /// <summary>
        /// When for the last time the project was update?
        /// </summary>
        internal long LastUpdate = DateTime.UtcNow.Ticks;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the directory of this project.
        /// </summary>
        public DirectoryInfo Directory
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the language of the project.
        /// </summary>
        public string Language
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
        public IAssetProvider Assets
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the asset provider for this project.
        /// </summary>
        public ITranslationProvider Translations
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
        #endregion

        #region Static Members
        /// <summary>
        /// Creates a site project from disk.
        /// </summary>
        /// <param name="path">The path to the configuration file.</param>
        /// <returns>The project.</returns>
        public static SiteProject FromDisk(string path)
        {
            return FromDisk(new DirectoryInfo(path));
        }

        /// <summary>
        /// Creates a site project from disk.
        /// </summary>
        /// <param name="path">The path to the configuration file.</param>
        /// <returns>The project.</returns>
        public static SiteProject FromDisk(DirectoryInfo path, string language = null)
        {
            if (!path.Exists)
                throw new FileNotFoundException("Unable to load the project, as the directory specified does not exist. Directory: " + path.FullName);

            // Prepare a project
            var project = new SiteProject();

            // Set the path where the project lives
            project.Directory = path;
            project.Configuration = SiteConfig.Read(path);

            // If we don't have the language specified, find the first language
            // in the configuration file.
            if (language == null && project.Configuration.Languages != null)
                language = project.Configuration.Languages.FirstOrDefault();
            if (language == null)
                language = "default";

            // Set the destination to the sub-language destination
            if (language != "default")
                project.Configuration.Destination = Path.Combine(project.Configuration.Destination, language);

            // Set the language of the project
            project.Language = language;

            // Load translation provider
            project.Translations = new TranslationProvider(project);

            // Assign a provider
            project.Assets = new DiskAssetProvider(path);
            project.ViewEngine = new RazorViewEngine(project);

            // We have a project!
            return project;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Cleans up the resources.
        /// </summary>
        /// <param name="disposing">Whether we are disposing or finalizing.</param>
        public void Dispose(bool disposing)
        {

        }

        /// <summary>
        /// Cleans up the resources.
        /// </summary>
        ~SiteProject()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Cleans up the resources.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        #endregion
    }

    /// <summary>
    /// Represents the build mode.
    /// </summary>
    public enum BakeMode
    {
        /// <summary>
        /// Fast mode is used for 'serve' without any optimizations.
        /// </summary>
        Fast = 0,

        /// <summary>
        /// Takes longer to bake, but optimizes everything.
        /// </summary>
        Optimized = 1
    }

}
