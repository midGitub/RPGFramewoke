功能说明
1.一键给资源命名
点击菜单栏中Build AssetBundle菜单中的NameAssetBundle选项
自动命名工具会根据鼠标当前选中的Assets目录进行递归命名
名字在右下角AssetLabels中可看到

2.一键自动打包资源
点击菜单栏中Build AssetBundle菜单中的Buidl For xxx选项
打包成功后资源放在Assets平级目录下的AssetBundles中

3.资源加载大概规则
	首先去加载Manifest配置文件，不同平台名字不一样（Windows，Android，IOS）
	加载资源时，首先判断之前是否加载并缓存过资源中的Object，如果存在了则直接用缓存的，否则继续下面
	根据assetbundleName去Manifest中查看是否有依赖的资源，如果有则先加载依赖，再加载主资源
	加载完主assetbundle后根据assetName得到Object，加到缓存列表中，然后回调callback对Object进行处理
	最后卸载assetbundle
	（如果是加载场景，不作缓存）

4.热更新资源测试
	打开test场景，为做测试，资源之前已经生成好两份，以便测试。
	在Assets文件夹的平级目录按AssetBundles/Windows新建这2个目录
	将Resources文件夹下的aaa.zip解压到AssetBundles/Windows/目录下
	在D盘的根目录新建一个Res文件夹，将bbb.zip解压到Res文件夹中（模拟远程服务器目录）
	开始测试：首先将AssetBundles/Windows/version.txt中改为version=2,此时是不更新的情况
	打开Scenes文件夹中的test场景运行，点击"热更新资源按钮"，加载出一个贴有女人头像的cube
	停止运行，再将AssetBundles/Windows/version.txt中改为version=1，此时为准备更新的状态
	再次运行test场景运行，再次点击"热更新资源按钮"按钮，加载出一个贴有男人头像的cube
	测试完毕

5.热更新脚本同资源类似测试

6.游戏打包资源到游戏启动并更新资源的流程（针对手机平台）如下：
	①.客户端首先把资源全部打成assetbundle包放到Assets目录下的StreamingAssets文件夹中（程序中可通过Application.streamingAssetsPath获取该目录）
	  如需热更新脚本，也可把脚本放到该目录下（如lua文件）
	②.游戏首次启动后首先将StreamingAssets文件夹中的所有文件拷贝到persistentDataPath文件夹中（程序中可通过Application.persistentDataPath获取该目录）
	  为什么要拷贝到这里？
	  因为unity打包的时候会将StreamingAssets文件夹中的所有内容打包到APK或IPA包中，但是该目录为可读不可写，且只能通过WWW方式加载，
	  而persistentDataPath目录属于可读可写目录，加载方式不限，于是为加载网络资源提供了条件。该目录内容可以被用户手动清理。
	  Android：
		Application.streamingAssetsPath : jar:file:///data/app/xxx.xxx.xxx.apk/!/assets
		Application.persistentDataPath : /data/data/xxx.xxx.xxx/files
	  IOS:
		Application.streamingAssetsPath : Application/xxxxx/xxx.app/Data/Raw
		Application.persistentDataPath : Application/xxxxx/Documents
	③.游戏以后每次启动后首先检查配置文件，资源版本是否为最新，如需更新就去网络加载，将最新资源或脚本下载或替换到persistentDataPath目录中，并更新配置文件
	④.游戏资源更新完毕后就开始加载persistentDataPath目录中的资源进行正常游戏
	注：该demo暂时还未做将StreamingAssets文件夹中所有文件拷贝到persistentDataPath文件夹的操作
