﻿using OpenInApp.Common.Helpers;
using OpenInApp.Common.Helpers.Dtos;
using System.Collections.Generic;

namespace OpenInEmacs.Helpers
{
    public class ConstantsForApp 
    {
        internal static KeyToExecutableEnum KeyToExecutableEnum = KeyToExecutableEnum.Emacs;
        public static ActualPathToExeDto ActualPathToExeDto = new ActualPathToExeHelper().GetActualPathToExeDto(KeyToExecutableEnum);
        private const string KeyToExecutableConstant = KeyToExecutableString.Emacs;

        public IEnumerable<string> GetDefaultTypicalFileExtensions()
        {
            return ActualPathToExeDto.DefaultTypicalFileExtensions;
        }

        internal static string Caption = Vsix.Name + " " + Vsix.Version;
        internal const string CommonActualPathToExeOptionLabel = CommonConstants.ActualPathToExeOptionLabelPrefix + KeyToExecutableConstant;
        internal static bool SeparateProcessPerFileToBeOpened = ActualPathToExeDto.SeparateProcessPerFileToBeOpened;//true;
        internal static bool UseShellExecute = ActualPathToExeDto.UseShellExecute;//true;
    }
}
