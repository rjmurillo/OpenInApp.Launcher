﻿using EnvDTE;
using EnvDTE80;
using OpenInApp.Common.Helpers;
using OpenInApp.Common.Helpers.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenInApp.Command
{
    public class MenuItemCallBackHelper
    {
        public PersistOptionsDto InvokeCommandCallBack(InvokeCommandCallBackDto dto)
        {
            var dte = (DTE2)dto.ServiceProvider.GetService(typeof(DTE));
            var persistOptionsDto = new PersistOptionsDto();

            try
            {
                var actualPathToExeExists = CommonFileHelper.DoesActualPathToExeExist(dto.ActualPathToExe);

                bool proceedToExecute = true;
                if (!actualPathToExeExists)
                {
                    proceedToExecute = false;
                    var fileHelper = new FilePrompterHelper(dto.Caption, dto.ExecutableFileToBrowseFor);
                    var badFilePath = string.IsNullOrEmpty(dto.ActualPathToExe) ? dto.ExecutableFileToBrowseFor : dto.ActualPathToExe;
                    persistOptionsDto = fileHelper.PromptForActualExeFile(badFilePath);

                    var newActualPathToExeExists = CommonFileHelper.DoesActualPathToExeExist(dto.ActualPathToExe);
                    if (newActualPathToExeExists)
                    {
                        proceedToExecute = true;
                    }
                    else
                    {
                        // User somehow managed to browse/select a new location for the exe that doesn't actually exist - impossible you'd think, but can happen if path to exe not found, user prompted to browse, they click "yes" to open the file browse dialog, then click "cancel"
                        OpenInAppHelper.InformUserMissingFile(dto.Caption, badFilePath);
                    }
                }
                if (proceedToExecute)
                {
                    var actualArtefactsToBeOpened = CommonFileHelper.GetArtefactNamesToBeOpened(dte, dto.CommandPlacement, dto.TypicalFileExtensions, dto.KeyToExecutableEnum);

                    var actualArtefactsToBeOpenedExist = DoArtefactsExist(actualArtefactsToBeOpened, dto.CommandPlacement, dto.ArtefactTypeToOpen);

                    if (!actualArtefactsToBeOpenedExist)
                    {
                        var missingFileName = CommonFileHelper.GetMissingFileName(actualArtefactsToBeOpened);
                        OpenInAppHelper.InformUserMissingFile(dto.Caption, missingFileName);
                    }
                    else
                    {
                        int fileQuantityWarningLimitInt;
                        var isInt = int.TryParse(dto.FileQuantityWarningLimit, out fileQuantityWarningLimitInt);
                        if (isInt)
                        {
                            proceedToExecute = false;
                            if (actualArtefactsToBeOpened.Count() > fileQuantityWarningLimitInt)
                            {
                                proceedToExecute = OpenInAppHelper.ConfirmProceedToExecute(dto.Caption, CommonConstants.ConfirmOpenFileQuantityExceedsWarningLimit);
                            }
                            else
                            {
                                proceedToExecute = true;
                            }
                            if (proceedToExecute)
                            {
                                var typicalFileExtensionAsList = CsvHelper.GetTypicalFileExtensionAsList(dto.TypicalFileExtensions);
                                var areTypicalFileExtensions = CommonFileHelper.AreTypicalFileExtensions(actualArtefactsToBeOpened, typicalFileExtensionAsList);
                                if (!areTypicalFileExtensions)
                                {
                                    if (dto.SuppressTypicalFileExtensionsWarning)
                                    {
                                        proceedToExecute = true;
                                    }
                                    else
                                    {
                                        proceedToExecute = OpenInAppHelper.ConfirmProceedToExecute(dto.Caption, CommonConstants.ConfirmOpenNonTypicalFile);
                                    }
                                }
                                if (proceedToExecute)
                                {
                                    OpenInAppHelper.InvokeCommand(actualArtefactsToBeOpened, 
                                        dto.ActualPathToExe, 
                                        dto.SeparateProcessPerFileToBeOpened, 
                                        dto.UseShellExecute,
                                        dto.ArtefactTypeToOpen);
                                }
                            }
                        }
                        else
                        {
                            Logger.Log(dto.Caption + " unexpected non-integer value found.");
                            OpenInAppHelper.ShowUnexpectedError(dto.Caption);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                OpenInAppHelper.ShowUnexpectedError(dto.Caption);
            }

            return persistOptionsDto;
        }

        private static bool DoArtefactsExist(IEnumerable<string> fullArtefactNames, CommandPlacement commandPlacement, ArtefactTypeToOpen artefactTypeToOpen)
        {
            if ((commandPlacement == CommandPlacement.IDM_VS_CTXT_FOLDERNODE ||
                 commandPlacement == CommandPlacement.IDM_VS_CTXT_PROJNODE) &&
                artefactTypeToOpen == ArtefactTypeToOpen.Folder)
            {
                artefactTypeToOpen = ArtefactTypeToOpen.Folder;
            }
            else
            {
                artefactTypeToOpen = ArtefactTypeToOpen.File;
            }

            return CommonFileHelper.DoArtefactsExist(fullArtefactNames, artefactTypeToOpen);
        }
    }
}