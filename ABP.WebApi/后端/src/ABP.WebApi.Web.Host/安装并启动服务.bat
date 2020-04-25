@echo.服务启动......  
@echo off  
@echo 当前路径 %~dp0\JobsServer.exe
@sc create JSFWService binPath= "%~dp0\ABP.WebApi.Web.Host.exe"  
@net start JSFWService   
@sc config JSFWService  start= AUTO  
@echo off  
@echo.服务启动完毕！  
@pause