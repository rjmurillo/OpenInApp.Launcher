﻿using Microsoft.VisualStudio.Shell;
using OpenInApp.Common.Helpers;
using OpenInApp.Menu;
using OpenInTreeSizeFree.Options.TreeSizeFree;
using System;

namespace OpenInTreeSizeFree.Commands
{
    internal sealed class OpenInAppCommand
    {
        private readonly Package _package;

        private IServiceProvider serviceProvider
        {
            get { return _package; }
        }

        public OpenInAppCommand(Package package)
        {
            _package = package;
        }

        public void Initialize()
        {
            ////var cmdIdOpenInAppFolderNode = PackageIds.CmdIdOpenInAppFolderNode == int.MinValue ? null : (int?)PackageIds.CmdIdOpenInAppFolderNode;
            //var cmdIdOpenInAppProject = PackageIds.CmdIdOpenInAppProject == int.MinValue ? null : (int?)PackageIds.CmdIdOpenInAppProject;

            var menuCore = new MenuCore(
                Vsix.Name,
                Vsix.Version,
                PackageGuids.guidOpenInVsCmdSetString,
                PackageIds.CmdIdOpenInAppItemNode,
                PackageIds.CmdIdOpenInAppCodeWin,
                PackageIds.CmdIdOpenInAppFolderNode,
                PackageIds.CmdIdOpenInAppProject,
                GeneralOptions.keyToExecutableEnum,
                VSPackage.Options.ActualPathToExe,
                VSPackage.Options.FileQuantityWarningLimit,
                VSPackage.Options.SuppressTypicalFileExtensionsWarning,
                VSPackage.Options.TypicalFileExtensions,
                GeneralOptions.keyToExecutableEnum.Description(),
                serviceProvider,
                VSPackage.Options);

            menuCore.MenuCoreOpenInAppCommand(_package);
        }
    }
}