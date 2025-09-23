<div id="top"></div>

## 使用技術一覧

<!-- シールド一覧 -->
<!-- 該当するプロジェクトの中から任意のものを選ぶ-->
<p style="display: inline">
  <!-- フレームワーク一覧 -->
  <img alt="Static Badge" src="https://img.shields.io/badge/build-nothing-green?style=flat"> <img alt="Static Badge" src="https://img.shields.io/badge/-Unity-%23444444?logo=Unity">  <img alt="Static Badge" src="https://img.shields.io/badge/C%23-512BD4?logo=csharp"> <img alt="Static Badge" src="https://img.shields.io/badge/Mathematica-red">
</p>

## 目次

1. [プロジェクトについて](#プロジェクトについて)
2. [環境](#環境)
3. [環境構築](#環境構築)
<!--4. [開発環境構築](#開発環境構築)-->
<!--5. [トラブルシューティング](#トラブルシューティング)-->

## プロジェクト名

ProjectCubeのリポジトリ

<!-- プロジェクトについて -->

## プロジェクトについて

ProjectCubeはUnity 6000.0.24で開発された3Dゲームです。
このゲームはリアルとバーチャルという異なる空間にいる二人のプレイヤーが、一辺30ｃｍのキューブ型デバイスを介して相互に干渉しながら対戦する新感覚ゲームです。
Raspberry Pi Pico Wを用いてPCとシリアル通信を行っています。

<!-- プロジェクトの概要を記載 -->

  <p align="left">
    <br />
    <!-- プロジェクト詳細にBacklogのWikiのリンク -->
    <!--　<a href="Backlogのwikiリンク"><strong>プロジェクト詳細 »</strong></a>　-->
    <br />
    <br />

<p align="right">(<a href="#top">トップへ</a>)</p>


## 環境

<!-- 言語、フレームワーク、ミドルウェア、インフラの一覧とバージョンを記載 -->

| フレームワーク  　　　|  バージョン  |
| --------------------- | ------------ |
| Unity                 | 6000.0.24f1  |

|       アセット        |  バージョン  |
| --------------------- | ------------ |
|    Meta XR Core SDK   |    71.0.0    |
|Meta XR Interaction ​SDK|    71.0.0    |

その他のパッケージのバージョンは Unity内のパッケージマネージャーを確認してください

<p align="right">(<a href="#top">トップへ</a>)</p>

<!-- 環境構築 -->

## 環境構築

Project Settingsの左タブからPlayerを選択し、Other Settingを開きます。  
そのうちの Api Comaptibility Level*  を  NET Framework  に変更してください

![スクリーンショット 2024-11-23 180444](https://github.com/user-attachments/assets/47c7c07e-a528-4ef2-beb9-e9d88496e546)
![スクリーンショット 2024-11-23 181710](https://github.com/user-attachments/assets/20edc99b-4b3a-4254-b8fe-ae49209b1538)

