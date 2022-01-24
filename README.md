## MSF 消息服务框架--测试程序

本解决方案程序演示了MSF Host和MSF客户端的使用，以及MSF服务组件的使用，详细信息，请参考博客链接：
http://www.cnblogs.com/bluedoctor/p/7605737.html

源码使用
-----------
* 1，打开VS2017/2019/2022，在IDE中选中解决方案视图中的MSFTest 项目，右键调试运行。
此时会提示是否启动服务宿主，单击选择“是”,启动运行 MSF Host程序。
* 2，运行TestClient项目，根据提示输入相关信息，然后回车运行。
* 3，运行完毕TestClient项目，程序退出。可以反复运行TestClient ，观察服务端的输出有利于你了解MSF的运行机制。

当前程序一共测试了MSF下面几种功能：
* 请求-响应 模式调用
* 发布-订阅 模式调用
* 服务端事件（分布式事件）
* 服务端多任务处理
* 客户端-服务的文本消息互通

测试程序使用了MSC模式:Model-Service-Client ，以闹铃服务为例，
* Model:  AlarmClockModel--服务模型
* Service:AlarmClockService--将服务模型进行封装调用，暴露提供对外的服务方法。
* Client: MSFClient--使用ServiceProxy对象，调用MSF的服务。

## 获取MSF
可以从Nuget获取使用MSF需要的包，在你的解决方案中，需要下面三种类型的解决方案项目：
* 1，客户端项目，比如WinForms\WPF\ASP.NET MVC. 需要在客户端项目安装下面的Nuget包：

```
Install-Package PDF.Net.MSF.Client -Version 1.2.3
```

* 2，服务端项目，这是一个类库项目，要求.NET 4.7.1以上。需要安装下面的Nuget包：

```
Install-Package PDF.Net.MSF.Service -Version 1.2.1
```


* 3，一个辅助程序，以后每次启动它即可启动MSF Host 来作为MSF服务组件的宿主。需要安装下面的Nuget包：

```
Install-Package PDF.Net.MSF.Service.Host -Version 1.2.6
```




## [MSF：一切都是消息](https://www.cnblogs.com/bluedoctor/p/7605737.html)

* 消息不都是队列暂存，也可以是实时的
* 命令是消息，事件也是消息
* 每种不同的消息可以看做是对象的不同方法
* 服务是消息的生产者，客户是消息的消费者

与常见的消息队列不同，MSF是一个实时消息处理框架，所以又称为iMSF-- immediately Message Service Framework。更多信息，请参考：
[“一切都是消息”--iMSF（即时消息服务框架）入门简介](https://www.cnblogs.com/bluedoctor/p/7605737.html)





