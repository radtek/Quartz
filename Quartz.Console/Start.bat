echo off
color 18

echo windows服务安装及卸载  执行即可

@SET FrameworkDir=%WINDIR%\Microsoft.NET\Framework64
@SET FrameworkVersion=v4.0.30319

@SET PATH=%FrameworkDir%\%FrameworkVersion%;%WINDIR%\System32;%PATH%;

@SET JDir=%cd%
@SET Service=%cd%\Quartz.Console.exe



@echo off

:S

echo 输入 1 后按回车键, 安装服务
echo 输入 2 后按回车键, 卸载服务

echo 输入 R 后按回车键, 启动服务
echo 输入 E 后按回车键, 停止服务

echo 输入 3 后按回车键, 退出操作

rem echo %JDir%
rem echo %Service%



set /p start=请选择 (1、2) 后按回车键:

if "%start%"=="1" goto 1
if "%start%"=="2" goto 2
if "%start%"=="3" goto 3
if "%start%"=="R" goto R
if "%start%"=="E" goto E


:1
color 1a
InstallUtil.exe /u  	"%Service%"   --卸载服务
InstallUtil.exe  		"%Service%"    --安装服务
echo 服务已经启动
pause
goto S
exit

:2
InstallUtil.exe /u  	"%Service%"    --卸载服务
echo 服务已经卸载
pause
goto S
exit

:3 exit

:R
echo 该服务现在处理停止状态,将进行启
net start "创维数字 JosnJiang"
goto S
exit


:E
echo 该服务现在处理停止状态,将进行启
net stop "创维数字 JosnJiang"
goto S
exit


pause
echo