@echo off
title Deploy to Web server
cls

set FTP_URL=www.lentfoehrden-cms2.de
set FTP_USERNAME=DeployUser
set FTP_PASSWORD=8sd674bvcrbfb3q48gxr8gezfX
set ARCHIVER="C:\Program Files\7-zip\7z"
set PASSWORD=Rd8f76g#sdf897g#6sd!87f6g8sd9s78df6g
set CONTAINER_FILENAME=calculator_publish.zip


pushd bin
if exist publish.zip del publish.zip
%ARCHIVER%  a -r -p%PASSWORD%  %CONTAINER_FILENAME%   publish\*
popd


if exist %TEMP%\DeployHelperScript.txt   del %TEMP%\DeployHelperScript.txt   	>NUL
echo open %FTP_URL%>>%TEMP%\DeployHelperScript.txt
echo %FTP_USERNAME%>>%TEMP%\DeployHelperScript.txt
echo %FTP_PASSWORD%>>%TEMP%\DeployHelperScript.txt
echo put bin\%CONTAINER_FILENAME%>>%TEMP%\DeployHelperScript.txt
echo quit>>%TEMP%\DeployHelperScript.txt

ftp -s:%TEMP%\DeployHelperScript.txt
del %TEMP%\DeployHelperScript.txt >NUL
set FTP_URL= >NUL
set FTP_USERNAME= >NUL
set FTP_PASSWORD= >NUL
set ARCHIVER= >NUL
set PASSWORD= >NUL
set CONTAINER_FILENAME= >NUL
