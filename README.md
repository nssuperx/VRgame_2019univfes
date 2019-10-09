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
* [GitLens — Git supercharged](https://marketplace.visualstudio.com/items?itemName=eamodio.gitlens)

## Unity 使用アセット
* [KinectForWindows_UnityPro_2.0.1410](https://developer.microsoft.com/ja-jp/windows/kinect)


## wslのgitをVScodeで使う
1. [wslgit.exe](https://github.com/andy-5/wslgit/releases) をダウンロード
2. `c:\opt` とかに保存
3. VScodeの Setting.json に　`"git.path": "C:\\opt\\wslgit.exe"` を追記


## おおまかなgitの始め方
1. 頑張ってunityのプロジェクトを置くフォルダまで移動してください。例

```sh
# wslの人
$ cd /mnt/c/Users/ユーザー名/unity/

# コマンドプロンプトの人
> cd unity/
```
2. git の設定
```sh
$ git config --global user.email "githubに登録したメールアドレス"
$ git config --global user.name "githubに登録したユーザー名"
```
ここで、余裕のある人はsshの設定を行ってください。

3. **自分**のリポジトリをクローン

```sh
$ git clone https://github.com/ユーザー名/VRgame_2019univfes.git
```

この時点でunityのプロジェクトファイルが `/VRgame_2019univfes`以下にできています。<br/>
Unity Hub でリストに追加すれば開けます。

## おおまかなgitの使い方
何かしらファイルを変更したら以下のようにしてコミット・プッシュします。
1. コミットするファイルを指定
```sh
$ git add *
```
2. コメントつけてコミットします
```sh
$ git commit -m "ここにコメント"
```
ここまででローカルリポジトリ（自分のパソコン）に作業履歴を作成できます。<br/>

3. githubにプッシュ
```sh
$ git push
```
これで**自分**のリモートリポジトリに作業履歴を保存できます。<br/>
変更を**Fork元**リポジトリに反映させたいときは pull request してください。

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
3. コンフリクト（競合）しなかったら、push。
```sh
$ git push
```
4. コンフリクトしてしまったら。

なんとか修正してください。

5. 修正したらコミットしてプッシュしてください。
```sh
$ git add *
$ git commit -m "ここにコメント"
$ git push
```

これでFork元のリポジトリの最新版に追従できる、はず。<br/>

6. pull request してください。

### 参考にしたサイト
* [[Unity3D] 画面表示を左右反転させる方法](https://blog.fujiu.jp/2015/09/unity3d.html)
* [KinectでUnityちゃんを動かす](https://qiita.com/yuzupon/items/0123bb6c268a41fcd708)
* [VScode・WSL・Git導入](https://qiita.com/Philosophistoria/items/48c4779739e6fafc63e0)

<br/><br/>

***

#### 以下 pull request の練習
nef0608

git はじめました
