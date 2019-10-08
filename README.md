# VRgame_2019univfes
2019年宮崎大学大学祭VRゲーム制作

***

## 開発環境
* Unity 2019.1.14f1
* [VScode](https://code.visualstudio.com/)
* [Kinect for Windows SDK 2.0](https://developer.microsoft.com/ja-jp/windows/kinect)

## VScode拡張機能
* [Japanese Language Pack for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=MS-CEINTL.vscode-language-pack-ja)
* [C#](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)
* [Debugger for Unity](https://marketplace.visualstudio.com/items?itemName=Unity.unity-debug)

## Unity 使用アセット
* [KinectForWindows_UnityPro_2.0.1410](https://developer.microsoft.com/ja-jp/windows/kinect)


## wslのgitをVScodeで使う
1. [wslgit.exe](https://github.com/andy-5/wslgit/releases) をダウンロード
2. `c:\opt` とかに保存
3. VScodeの Setting.json に　`"git.path": "C:\\opt\\wslgit.exe"` を追記


### 参考にしたサイト
* [[Unity3D] 画面表示を左右反転させる方法](https://blog.fujiu.jp/2015/09/unity3d.html)
* [KinectでUnityちゃんを動かす](https://qiita.com/yuzupon/items/0123bb6c268a41fcd708)
* [VScode・WSL・Git導入](https://qiita.com/Philosophistoria/items/48c4779739e6fafc63e0)

## pull request するとき
はじめに、Fork元のリポジトリの設定をします。
```sh
$ git remote add upstream https://github.com/nssuperx/VRgame_2019univfes
```
以下、Fork元リポジトリに追従していきます
1. フェッチ（変更をとってくる）
```sh
$ git fetch upstream
```
2. マージ（Fork元と自分のをくっつける）
```sh
$ git merge upstream/master
```
3. コンフリクトしたら、修正する

これでFork元のリポジトリの最新版に追従できる、はず。<br/>
これ以降は自分のリモートリポジトリにpushして pull request してください。

<br/><br/>

***

#### 以下 pull request の練習
nef0608

git はじめました
