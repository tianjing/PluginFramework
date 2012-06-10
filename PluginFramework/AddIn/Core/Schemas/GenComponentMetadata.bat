@echo off
set sdkDir="C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A"

%sdkDir%\bin\xsd.exe ComponentMetadata.xsd /classes /l:cs /n:PluginFramework.AddIn.Core.Metadata /o:..\Metadata


pause
@echo on
