echo off

color 1a

echo windows ����װ��ж��  ִ�м���

@SET FrameworkDir=%WINDIR%\Microsoft.NET\Framework64
@SET FrameworkVersion=v4.0.30319

@SET PATH=%FrameworkDir%\%FrameworkVersion%;%WINDIR%\System32;%PATH%;

@SET JDir=%cd%
@SET Service=%cd%\Quartz.Console.exe




@echo off

:S

echo ���� 1 �󰴻س���, ��װ����
echo ���� 2 �󰴻س���, ж�ط���

echo ���� R �󰴻س���, ��������
echo ���� E �󰴻س���, ֹͣ����

echo ���� 3 �󰴻س���, �˳�����

rem echo %JDir%
rem echo %Service%
rem echo %PATH%

rem InstallUtil.exe C:\Users\Joson\Desktop\Quartz\Quartz.Console\bin\Debug\Quartz.Console.exe
rem InstallUtil.exe /u C:\Users\Joson\Desktop\Quartz\Quartz.Console\bin\Debug\Quartz.Console.exe
 

set /p start=��ѡ�� (1��2) �󰴻س���:

if "%start%"=="1" goto 1
if "%start%"=="2" goto 2
if "%start%"=="3" goto 3
if "%start%"=="R" goto R
if "%start%"=="E" goto E



:1
color 1f

rem  @SET PATH="C:\Program Files\Microsoft Visual Studio 11.0\VC";
rem  cd  C:\Program Files\Microsoft Visual Studio 11.0\VC

InstallUtil.exe /u  		"%Service%"   --ж�ط���
InstallUtil.exe  		"%Service%"   --��װ����
echo �����Ѿ�����
pause
goto S
exit

:2

InstallUtil.exe /u  		"%Service%"    --ж�ط���
echo �����Ѿ�ж��
pause
goto S
exit

:3 exit

:R
echo �÷������ڴ���ֹͣ״̬,��������
net start "��ά���� JosnJiang"
goto S
exit


:E
echo �÷������ڴ���ֹͣ״̬,��������
net stop "��ά���� JosnJiang"
goto S
exit


pause
echo