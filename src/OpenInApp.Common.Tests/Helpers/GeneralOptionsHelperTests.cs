﻿using NUnit.Framework;
using OpenInApp.Common.Helpers;
using OpenInApp.Common.Helpers.Dtos;
using System.Linq;

namespace OpenInApp.Common.Tests.Helpers
{
    [TestFixture()]
    public class GeneralOptionsHelperTests
    {
        [Test()]
        [TestCase(ExeFileNameConstants.ChromeCanary, @"C:\Users\greg\AppData\Local\Google\Chrome SxS\Application\chrome.exe")]
        [TestCase(ExeFileNameConstants.Emacs, null)]
        [TestCase(ExeFileNameConstants.FirefoxDeveloperEdition, @"C:\Program Files\Firefox Developer Edition\firefox.exe")]
        [TestCase(ExeFileNameConstants.Gimp, @"C:\Program Files\GIMP 2\bin\gimp-2.8.exe")]
        [TestCase(ExeFileNameConstants.MarkdownMonster, @"C:\Program Files (x86)\Markdown Monster\MarkdownMonster.exe")]
        [TestCase(ExeFileNameConstants.MSPaint, @"C:\WINDOWS\system32\mspaint.exe")]
        [TestCase(ExeFileNameConstants.Opera, @"C:\Program Files (x86)\Opera\opera.exe")]
        [TestCase(ExeFileNameConstants.OperaDeveloperEdition, @"C:\Program Files\Opera developer\launcher.exe")]
        [TestCase(ExeFileNameConstants.PaintDotNet, @"C:\Program Files\Paint.NET\PaintDotNet.exe")]
        [TestCase(ExeFileNameConstants.Vivaldi, @"C:\Users\greg\AppData\Local\Vivaldi\Application\vivaldi.exe")]
        [TestCase(ExeFileNameConstants.VS2012, @"C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\devenv.exe")]
        [TestCase(ExeFileNameConstants.VS2013, @"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\devenv.exe")]
        [TestCase(ExeFileNameConstants.VS2015, @"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe")]
        [TestCase(ExeFileNameConstants.VS2017Community, @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.exe")]
        [TestCase(ExeFileNameConstants.VS2017Enterprise, @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\devenv.exe")]
        [TestCase(ExeFileNameConstants.VS2017Professional, @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\IDE\devenv.exe")]
        [TestCase(ExeFileNameConstants.XamarinStudio, @"C:\Program Files (x86)\Xamarin Studio\bin\XamarinStudio.exe")]
        [TestCase(ExeFileNameConstants.XMLSpy, @"C:\Program Files (x86)\Altova\XMLSpy2016\XMLSpy.exe")]
        [Category("U")]
        public void GetActualPathToExeTest(string executableFileToBrowseFor, string expected)
        {
            //Act
            var actual = GeneralOptionsHelper.GetSearchPaths(executableFileToBrowseFor);

            //Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [Test()]
        [Category("U")]
        public void GetMultipleYearPathsTest()
        {
            //Arrange
            var secondaryFilePathSegment = "abc2016d";

            //Act
            var actual = GeneralOptionsHelper.GetMultipleYearPaths("app.exe", InitialFolderType.ProgramFilesX86, secondaryFilePathSegment);

            //Assert
            
            Assert.IsFalse(actual.Contains(@"C:\Program Files (x86)\abc1995d\app.exe"));
            Assert.IsTrue(actual.Contains(@"C:\Program Files (x86)\abc1996d\app.exe"));
            Assert.IsTrue(actual.Contains(@"C:\Program Files (x86)\abc1996d\app.exe"));
            Assert.IsTrue(actual.Contains(@"C:\Program Files (x86)\abc2019d\app.exe"));
            Assert.IsTrue(actual.Contains(@"C:\Program Files (x86)\abc2020d\app.exe"));
            Assert.IsFalse(actual.Contains(@"C:\Program Files (x86)\abc2021d\app.exe"));
            Assert.AreEqual(25, actual.Count());
        }
    }
}