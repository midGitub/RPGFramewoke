����˵��
1.һ������Դ����
����˵�����Build AssetBundle�˵��е�NameAssetBundleѡ��
�Զ��������߻������굱ǰѡ�е�AssetsĿ¼���еݹ�����
���������½�AssetLabels�пɿ���

2.һ���Զ������Դ
����˵�����Build AssetBundle�˵��е�Buidl For xxxѡ��
����ɹ�����Դ����Assetsƽ��Ŀ¼�µ�AssetBundles��

3.��Դ���ش�Ź���
	����ȥ����Manifest�����ļ�����ͬƽ̨���ֲ�һ����Windows��Android��IOS��
	������Դʱ�������ж�֮ǰ�Ƿ���ز��������Դ�е�Object�������������ֱ���û���ģ������������
	����assetbundleNameȥManifest�в鿴�Ƿ�����������Դ����������ȼ����������ټ�������Դ
	��������assetbundle�����assetName�õ�Object���ӵ������б��У�Ȼ��ص�callback��Object���д���
	���ж��assetbundle
	������Ǽ��س������������棩

4.�ȸ�����Դ����
	��test������Ϊ�����ԣ���Դ֮ǰ�Ѿ����ɺ����ݣ��Ա���ԡ�
	��Assets�ļ��е�ƽ��Ŀ¼��AssetBundles/Windows�½���2��Ŀ¼
	��Resources�ļ����µ�aaa.zip��ѹ��AssetBundles/Windows/Ŀ¼��
	��D�̵ĸ�Ŀ¼�½�һ��Res�ļ��У���bbb.zip��ѹ��Res�ļ����У�ģ��Զ�̷�����Ŀ¼��
	��ʼ���ԣ����Ƚ�AssetBundles/Windows/version.txt�и�Ϊversion=2,��ʱ�ǲ����µ����
	��Scenes�ļ����е�test�������У����"�ȸ�����Դ��ť"�����س�һ������Ů��ͷ���cube
	ֹͣ���У��ٽ�AssetBundles/Windows/version.txt�и�Ϊversion=1����ʱΪ׼�����µ�״̬
	�ٴ�����test�������У��ٴε��"�ȸ�����Դ��ť"��ť�����س�һ����������ͷ���cube
	�������

5.�ȸ��½ű�ͬ��Դ���Ʋ���

6.��Ϸ�����Դ����Ϸ������������Դ�����̣�����ֻ�ƽ̨�����£�
	��.�ͻ������Ȱ���Դȫ�����assetbundle���ŵ�AssetsĿ¼�µ�StreamingAssets�ļ����У������п�ͨ��Application.streamingAssetsPath��ȡ��Ŀ¼��
	  �����ȸ��½ű���Ҳ�ɰѽű��ŵ���Ŀ¼�£���lua�ļ���
	��.��Ϸ�״����������Ƚ�StreamingAssets�ļ����е������ļ�������persistentDataPath�ļ����У������п�ͨ��Application.persistentDataPath��ȡ��Ŀ¼��
	  ΪʲôҪ���������
	  ��Ϊunity�����ʱ��ὫStreamingAssets�ļ����е��������ݴ����APK��IPA���У����Ǹ�Ŀ¼Ϊ�ɶ�����д����ֻ��ͨ��WWW��ʽ���أ�
	  ��persistentDataPathĿ¼���ڿɶ���дĿ¼�����ط�ʽ���ޣ�����Ϊ����������Դ�ṩ����������Ŀ¼���ݿ��Ա��û��ֶ�����
	  Android��
		Application.streamingAssetsPath : jar:file:///data/app/xxx.xxx.xxx.apk/!/assets
		Application.persistentDataPath : /data/data/xxx.xxx.xxx/files
	  IOS:
		Application.streamingAssetsPath : Application/xxxxx/xxx.app/Data/Raw
		Application.persistentDataPath : Application/xxxxx/Documents
	��.��Ϸ�Ժ�ÿ�����������ȼ�������ļ�����Դ�汾�Ƿ�Ϊ���£�������¾�ȥ������أ���������Դ��ű����ػ��滻��persistentDataPathĿ¼�У������������ļ�
	��.��Ϸ��Դ������Ϻ�Ϳ�ʼ����persistentDataPathĿ¼�е���Դ����������Ϸ
	ע����demo��ʱ��δ����StreamingAssets�ļ����������ļ�������persistentDataPath�ļ��еĲ���
