使用说明
====
项目基于.net core 3.1 数据库sqlserver2012
确保安装正确的环境

===
演示地址账号密码: test Admin123...

第一个文件夹是后台代码,包含后端接口和前端程序
第二个文件夹是identityserver4项目
使用时先用脚本创建数据库和初始化数据

框架基于asf修改和移植而来,改动比较大,若要使用非identityserver版本可以使用原作者的(https://github.com/ASF-Framework/ASF-Vue)

identityserver使用开源项目,IdentityServer4.Admin(https://github.com/skoruba/IdentityServer4.Admin)

前台组件可以参考网站底部的链接信息

后台任务使用的hangfire7.2

运行
====
先启动identityserver
abp的后台项目发布后可以安装为windows服务,或者采用docker部署,因为有后台任务,也可以后台任务单独部署为服务,
项目部署到iis
运行前台:
使用前先安装依赖: npm install
运行: npm run serve
打包:
npm run build

