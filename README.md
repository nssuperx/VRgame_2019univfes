# VRgame_2019univfes
2019年宮崎大学大学祭VRゲーム制作<br/><br/>
チームメンバーの人はこのリポジトリをフォーク(Fork)してください。<br/>
ページ上部右側にフォークするボタンがあります。

***

## <div id="index">もくじ</div>
#### 開発環境
* [開発環境](#section1)
* [VScode拡張機能](#section2)
* [Unity 使用アセット](#section3)
* [wslのgitをVScodeで使う](#section4)
#### git関連
* [おおまかなgitの始め方](#section5)
* [おおまかなgitの使い方](#section6)
* [pull request するとき](#section7)
* [戻したい](#section8)
#### 参考にさせてもらったもの
* [参考にしたサイト](#section9)

***

## <div id="section1">開発環境</div>
* Unity 2019.1.14f1
    * Android Build Support
        * Android SDK & NDK Tools
* [VScode](https://code.visualstudio.com/)
* [Kinect for Windows SDK 2.0](https://developer.microsoft.com/ja-jp/windows/kinect)


## <div id="section2">VScode拡張機能</div>
* [Japanese Language Pack for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=MS-CEINTL.vscode-language-pack-ja)
* [C#](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)
* [Debugger for Unity](https://marketplace.visualstudio.com/items?itemName=Unity.unity-debug)
* [GitLens — Git supercharged](https://marketplace.visualstudio.com/items?itemName=eamodio.gitlens)


## <div id="section3">Unity 使用アセット</div>
* [KinectForWindows_UnityPro_2.0.1410](https://developer.microsoft.com/ja-jp/windows/kinect)
* [Oculus Integration](https://assetstore.unity.com/packages/tools/integration/oculus-integration-82022)


## <div id="section4">wslのgitをVScodeで使う</div>
1. [wslgit.exe](https://github.com/andy-5/wslgit/releases) をダウンロード
2. `c:\opt` とかに保存
3. VScodeの Setting.json に　`"git.path": "C:\\opt\\wslgit.exe"` を追記


## <div id="section5">おおまかなgitの始め方</div>
1. 頑張ってunityのプロジェクトを置くフォルダまで移動してください。以下は例です。

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

この時に使用するURLはリポジトリのページにある `Clone or download` をクリックすると確認できます。<br/>
この時点でunityのプロジェクトファイルが `/VRgame_2019univfes`以下にできています。<br/>
Unity Hub でリストに追加すれば開けます。

<a href='#index'>もくじ</a>

## <div id="section6">おおまかなgitの使い方</div>
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

<a href='#index'>もくじ</a>

## <div id="section7">pull request するとき</div>
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

6. pull request してください。ブラウザからできます。

<a href='#index'>もくじ</a>

## <div id="section8">戻したい</div>
* addする前
```sh
# 指定したファイルの編集内容がなくなる
$ git checkout [ファイル名]
# 全部の編集内容がなくなる
$ git checkout .
```
* addしてないことにしたい
```sh
# 指定したファイルをaddしてないことにする、編集内容は残る
$ git reset [ファイル名]
# 全部addしてないことにする
$ git reset
# 指定したファイルを最後にコミットした状態に戻す
$ git checkout HEAD -- [ファイル名]
# addしてないことにして、編集内容も消す
$ git reset --hard HEAD
```
* コミットのバージョンを昔のに戻したい

まず戻したいところのコミットidを確認する
```sh
$ git log
```
確認したら、以下のどちらかのコマンドを実行
```sh
# 全部戻したい
$ git reset --hard [コミットid]
# 特定のファイルのみ戻したい
$ git checkout [コミットid] [ファイルパス]
```

* 新しいファイル追加したんだけど、やっぱり消したい（add前）

gitコマンドを使用しなくてもそのまま削除すればよいです。

<a href='#index'>もくじ</a>

<br/><br/>

### <div id="section9">参考にしたサイト</div>
* [[Unity3D] 画面表示を左右反転させる方法](https://blog.fujiu.jp/2015/09/unity3d.html)
* [KinectでUnityちゃんを動かす](https://qiita.com/yuzupon/items/0123bb6c268a41fcd708)
* [VScode・WSL・Git導入](https://qiita.com/Philosophistoria/items/48c4779739e6fafc63e0)
* [[git] 戻したい時よく使っているコマンドまとめ](https://qiita.com/rch1223/items/9377446c3d010d91399b)

<a href='#index'>もくじ</a>

<br/><br/>

***

#### 以下 pull request の練習
nef0608

git はじめました
