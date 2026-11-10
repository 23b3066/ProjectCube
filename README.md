# ProjectCube

**現実空間とVR空間を使った、2人対戦ゲーム。**

ProjectCubeは、一辺約30cmのキューブ型デバイスとVR空間を組み合わせた、2人用の非対称対戦ゲームです。2人は同じ「キューブ」を異なる視点から体験します。

- **VR側：** キューブ内部の仮想空間を探索し、妨害を乗り越えて脱出を目指す。
- **現実側：** キューブ上部のモニターからVR側の状況を俯瞰し、側面の9個のボタンで妨害を仕掛ける。

VR側の一人称視点と、現実側の俯瞰視点。**見えているものも、できることも違う2人の駆け引き**が特徴です。

## ゲームの仕組み

| VR側 | 現実側 |
| --- | --- |
| Meta Quest 3でキューブ内部の空間を体験 | 現実のキューブ型デバイスとモニターを操作 |
| 移動やアクションでステージを攻略 | ボタン操作でVR側の進行を妨害 |
| 妨害を避けながら脱出を目指す | 妨害を使い分けて脱出を阻止する |

### 現実の操作がVR側に届く

キューブ型デバイスに組み込んだボタンの入力を、**Raspberry Pi Pico WとPCのシリアル通信**を通じてUnityへ伝えます。ゲーム側では階層・進行状況に応じた妨害処理に利用します。

公開コードには、**照明の消灯・暴風・障害物**に関係する処理があり、時間経過による終了・再使用の制御も実装されています。

### 制作意図

物理的に触れられるキューブと、VRで体験するキューブを対応させることで、同じ空間を共有しながら異なる役割で対戦できる体験を目指しました。現実側はVR側の行動を俯瞰して妨害を考え、VR側はその妨害に対処しながら進みます。

## 実装・ソースコード

技術面を詳しく知りたい場合は、[主要システムの解説](docs/implementation.md)をご覧ください。

| システム | 主なソース | 内容 |
| --- | --- | --- |
| デバイス通信 | [SerialHandler.cs](Assets/Script/SerialHandler.cs) | シリアルポートを開き、受信情報をイベントで通知 |
| 妨害・進行制御 | [GimmickController.cs](Assets/Script/GimmickController.cs) | ボタン入力を受け、照明・暴風・障害物の処理を切り替える |
| 暴風の影響 | [WindBounce.cs](Assets/Script/WindBounce.cs) | パーティクル衝突時に対象へ力を加える |
| 現実側の表示 | [MultiCameraDisplay.cs](Assets/Script/MultiCameraDisplay.cs) | PC画面の表示を三人称視点カメラに切り替える |
| VR側のアクション | [GunShoot.cs](Assets/Script/GunShoot.cs)、[ShieldController.cs](Assets/Script/ShieldController.cs) | 射撃やシールドに関連する処理 |
| ゲーム進行 | [Timer.cs](Assets/Script/Timer.cs) | カウントダウンと時間切れのシーン遷移 |

この表は公開されたスクリプトの実装内容に基づいています。個々のスクリプトがどのシーンで使用されるか、デバイス接続時の一連の挙動については、実機・Unity Editorでの確認が別途必要です。

## 開発環境

| 項目 | 内容 |
| --- | --- |
| ゲームエンジン | **Unity 6000.0.24f1** |
| プログラミング | C# |
| VRデバイス | Meta Quest 3 |
| VR関連 | Meta XR Core SDK 71.0.0 / Meta XR Interaction SDK 71.0.0 |
| 描画 | Universal Render Pipeline 17.0.3 |
| 入力 | Unity Input System 1.11.1 |
| 物理デバイス | 約30cmのキューブ型デバイス / Raspberry Pi Pico W |
| 通信 | PCとのシリアル通信 |

Unityやパッケージのバージョンは、リポジトリ内の [ProjectVersion.txt](ProjectSettings/ProjectVersion.txt) と [manifest.json](Packages/manifest.json) に記録されています。

## リポジトリの構成

- [Assets/Script](Assets/Script)：ゲームロジック、デバイス連携、VR操作などのC#スクリプト
- [Assets/Scenes](Assets/Scenes)：開始画面、ステージ、クリア・ゲームオーバー画面などのシーン
- [Assets](Assets)：シーンで使用するPrefab、モデル、演出素材など
- [Packages](Packages)：Unityのパッケージ定義
- [ProjectSettings](ProjectSettings)：Unityプロジェクト設定
- [docs/implementation.md](docs/implementation.md)：主要な処理の解説

本リポジトリにはUnityプロジェクトのファイルが含まれますが、**専用の物理デバイスやVR機器が必要なため、取得するだけで展示と同じ体験ができるものではありません。** 使用環境によっては、シリアルポートなどの設定変更も必要です。

## 制作について

2024年9月から2025年9月にかけて、3人のチームで制作した作品です。作品のゲーム画面や動画など、視覚的な紹介は別のポートフォリオにまとめる方針のため、このREADMEではゲームの仕組みと実装の説明を中心にしています。

## 権利・利用に関する注意

プロジェクトにはMeta XR SDKなどの第三者製ソフトウェアや、外部アセットに由来する可能性のあるファイルが含まれています。**このリポジトリがPublicであることは、すべての素材やコードを自由に再配布・再利用できることを意味しません。** 個別のライセンス・公開条件を確認してください。
