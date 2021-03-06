﻿using OfficeDevPnP.PowerShell.Commands.Base;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WebParts;
using System.Management.Automation;
using OfficeDevPnP.Core.Entities;

namespace OfficeDevPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Add, "SPOWebPartToWikiPage")]
    public class AddWebPartToWikiPage : SPOWebCmdlet
    {
        [Parameter(Mandatory = true)]
        public string PageUrl = string.Empty;

        [Parameter(Mandatory = true, ParameterSetName = "XML")]
        public string Xml = string.Empty;

        [Parameter(Mandatory = true, ParameterSetName = "FILE")]
        public string Path = string.Empty;

        [Parameter(Mandatory = true)]
        public string ZoneId;

        [Parameter(Mandatory = true)]
        public int ZoneIndex;

        protected override void ExecuteCmdlet()
        {
            WebPartEntity wp = null;

            if (ParameterSetName == "FILE")
            {
                if (System.IO.File.Exists(Path))
                {
                    System.IO.StreamReader fileStream = new System.IO.StreamReader(Path);
                    string webPartString = fileStream.ReadToEnd();
                    fileStream.Close();

                    wp = new WebPartEntity();
                    wp.WebPartZone = ZoneId;
                    wp.WebPartIndex = ZoneIndex;
                    wp.WebPartXml = webPartString;
                }
            }
            else if (ParameterSetName == "XML")
            {
                wp = new WebPartEntity();
                wp.WebPartZone = ZoneId;
                wp.WebPartIndex = ZoneIndex;
                wp.WebPartXml = Xml;
            }
            if (wp != null)
            {
                this.SelectedWeb.AddWebPartToWebPartPage(PageUrl, wp);
            }
        }

        public enum WPPageType
        {
            WikiPage,
            WebPartPage
        }
    }
}
