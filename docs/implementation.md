# ProjectCube：主要システムの実装

ProjectCubeは、現実のキューブ型デバイスとVR空間を対応させた2人対戦ゲームです。ここでは公開中のC#スクリプトで確認できる処理を整理します。UnityのInspector設定や実機での接続結果をすべて保証する資料ではありません。

## 1. 物理デバイスからUnityへの入力

[SerialHandler.cs](../Assets/Script/SerialHandler.cs) では、`System.IO.Ports.SerialPort` を使って通信ポートを開き、バックグラウンドスレッドで受信した行を保持します。Unityの `Update()` では、受信メッセージを `OnDataReceived` イベントで通知します。

- 受信イベントを通じて、ゲーム側のスクリプトへ入力を渡せる設計です。
- スクリプトの初期設定は `COM4`、`9600` baudですが、使用PCに応じて変更が必要です。
- 接続機器としてRaspberry Pi Pico Wを使用します。ファームウェアや配線・各入力の対応はこの資料だけでは再現できません。

## 2. 現実側の妨害操作

[GimmickController.cs](../Assets/Script/GimmickController.cs) では、シリアル受信イベントを購読し、`Pressed:` に続く数値を妨害入力として受け取る処理が実装されています。キーボード入力による確認経路も用意されています。

`FloorCount` の値に応じて処理が切り替わります。

| `FloorCount` | コード上の処理 |
| --- | --- |
| 0 | 対応する照明グループを消灯し、設定時間後に復帰 |
| 1 | 暴風に関係するパーティクルを再生し、警告表示と終了処理を制御 |
| 2 | 妨害用のオブジェクトを生成・移動し、警告表示を制御 |
| 3 | `GameClear` シーンへ遷移 |

シリアル入力値・列の対応、実際の効果時間などはソースとInspector設定の両方で確認してください。

## 3. 妨害がVR側に与える影響

[WindBounce.cs](../Assets/Script/WindBounce.cs) は、パーティクルの衝突を受けた対象に対し、Rigidbodyへ力を加える仕組みです。

VR側のアクションには、[GunShoot.cs](../Assets/Script/GunShoot.cs) の射撃処理、[ShieldController.cs](../Assets/Script/ShieldController.cs) の向きと武器状態に応じたシールド制御などがあります。

これらのスクリプトはゲーム内の仕組みを説明するための参照です。特定ステージでの使用状況や動作結果はシーン・実機での確認が必要です。

## 4. 異なる視点の表示

[MultiCameraDisplay.cs](../Assets/Script/MultiCameraDisplay.cs) では、PC側のゲーム画面を三人称視点カメラに切り替える処理を確認できます。VR側の一人称視点に対して、現実側が状況を俯瞰する仕組みの一部です。

## 5. 時間とステージ進行

[Timer.cs](../Assets/Script/Timer.cs) には、残り時間の表示と、時間切れになった際の `GameOver` シーンへの遷移が実装されています。

[EditorBuildSettings.asset](../ProjectSettings/EditorBuildSettings.asset) には、`Start`、`stage1`、`stage2`、`stage1_hard`、`GameClear`、`GameOver` が有効なシーンとして登録されています。

## ソースの扱い

公開フォルダには、ゲーム固有のスクリプトに加えて、Metaなど第三者が著作権を持つコード・アセットも含まれています。ファイルの存在だけで自作と判断しないでください。実ファイルを再配布する場合はライセンス確認が必要です。
