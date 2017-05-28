﻿using Microsoft.VisualStudio.Shell;
using OpenInApp.Command;
using OpenInApp.Common.Helpers;
using OpenInAbracadabra.Helpers;
using System;
using System.ComponentModel.Design;

namespace OpenInAbracadabra.Commands
{
    internal sealed class OpenInAppCommand
    {
        private string Caption { get { return ConstantsForApp.Caption; } }
        public readonly Guid CommandSet = new Guid(PackageGuids.guidOpenInVsCmdSetString);
        public OpenInAppCommand Instance { get; private set; }

        private readonly Package _package;
        private IServiceProvider ServiceProvider => _package;

        public OpenInAppCommand()
        {
        }

        public void Initialize(Package package)
        {
            Instance = new OpenInAppCommand(package);
        }

        private OpenInAppCommand(Package package)
        {
            Logger.Initialize(ServiceProvider, Caption);

            if (package == null)
            {
                Logger.Log(new ArgumentNullException(nameof(package)));
                OpenInAppHelper.ShowUnexpectedError(Caption);
            }
            else
            {
                _package = package;
                var commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
                if (commandService != null)
                {
                    AddMenuCommand(commandService, PackageIds.CmdIdOpenInAppFolderExplore, true);
                    AddMenuCommand(commandService, PackageIds.CmdIdOpenInAppCodeWin, false);
                }
            }
        }

        private void AddMenuCommand(OleMenuCommandService commandService, int packageId, bool isFromSolutionExplorer)
        {
            var menuCommandID = new CommandID(CommandSet, packageId);

            MenuCommand menuCommand;

            if (isFromSolutionExplorer)
            {
                menuCommand = new MenuCommand(MenuItemCallback_FolderExplore, menuCommandID);
            }
            else
            {
                menuCommand = new MenuCommand(MenuItemCallback_CodeWin, menuCommandID);
            }

            commandService.AddCommand(menuCommand);
        }

        private void MenuItemCallback_FolderExplore(object sender, EventArgs e)
        {
            MenuItemCallback(true);
        }

        private void MenuItemCallback_CodeWin(object sender, EventArgs e)
        {
            MenuItemCallback(false);
        }

        private void MenuItemCallback(bool isFromSolutionExplorer)
        {
            var menuItemCallBackHelper = new MenuItemCallBackHelper();

            var constantsForApp = new ConstantsForApp();

            var invokeCommandCallBackDto = constantsForApp.GetInvokeCommandCallBackDto(
                VSPackage.Options.ActualPathToExe,
                VSPackage.Options.FileQuantityWarningLimit,
                isFromSolutionExplorer,
                ServiceProvider,
                VSPackage.Options.SuppressTypicalFileExtensionsWarning,
                VSPackage.Options.TypicalFileExtensions);

            var persistOptionsDto = menuItemCallBackHelper.InvokeCommandCallBack(invokeCommandCallBackDto);

            if (persistOptionsDto.Persist)
            {
                VSPackage.Options.PersistVSToolOptions(persistOptionsDto.ValueToPersist);
            }
        }
    }
}