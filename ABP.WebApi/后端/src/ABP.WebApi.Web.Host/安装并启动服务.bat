@echo.��������......  
@echo off  
@echo ��ǰ·�� %~dp0\JobsServer.exe
@sc create JSFWService binPath= "%~dp0\ABP.WebApi.Web.Host.exe"  
@net start JSFWService   
@sc config JSFWService  start= AUTO  
@echo off  
@echo.����������ϣ�  
@pause