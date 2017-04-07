using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurePLUSMailerLib;

#region "サンプル1"

//スクリプト初期化のためのクラス
public class SampleMailThreadInitializer : CurePLUSMailerLib.IThreadDataInitializer
{
    // ゲームデータ初期化時にコールされます
    public void Initialize(ApplicationContext context)
    {
        // スレッドが進行するために必要なデータを設定します
        context.SetValue("リプライ説明スレッドステージ", 0);
        context.SetValue("SampleMailThreadParameter1", 1);
        context.SetValue("SampleMailThreadParameter2", "hoge");
    }
}

[CurePLUSMailerLib.MailInformation(
Sender = CurePLUSMailerLib.Entity.Character.CharacterID.Hime, // メールの送信者
Title = "サンプルメール１", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"test mail content

このメールはシステムからユーザへ
送信されるメールとしては最小単位のメールになります

ゲームパラメータの埋め込みは
以下のように指定します：
\[UserSettings/Username]
\[UserSettings/Nickname1]
\[UserSettings/Nickname2]
\[UserSettings/BirthMonth]
\[UserSettings/BirthDay]

スタンプに設定可能な画像名
hime-iya
hime-kanasii
hime-ko
hime-love
hime-okoru
hime-tanosii
hime-yorokobu
meroakireru
merolove
merook
merookoru
merotanosii
meroykanasii
meroyorokobi
", // 行末の「",」は削除しないでください
Stamp = "merolove", // 空白でスタンプ無しになります
AdventurePart = "sonzaisinai/scenario")] // アドベンチャーパートのシナリオ名．空白でジャンプボタン無しになります
public class SampleMail1 : CurePLUSMailerLib.IMailManager
{
    // メールが受信可能かどうかのチェックを行い、結果をbool型で返却します
    public bool canReceive(ApplicationContext context)
    {
        return 5 < context.初回起動からの経過秒(); // このスクリプトだとプログラム起動から約5秒後にメールが送信されます
    }

    // メールを受信したタイミングでコールされます
    public void onReceived(ApplicationContext context)
    {
        // パラメータの更新等
    }

    // 初めてメールを開いたタイミングでコールされます
    public void onRead(ApplicationContext context)
    {
        // パラメータの更新等
    }

    // アドベンチャーパートから復帰したタイミングでコールされます
    public void onDoneAdventurePart(ApplicationContext context)
    {
        // パラメータの更新等
    }
}

[CurePLUSMailerLib.MailInformation(
Sender = CurePLUSMailerLib.Entity.Character.CharacterID.Hime, // メールの送信者
Title = "サンプルメール２（クラス名）", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"test mail content

このようにメールを複数定義するとシステムはその数だけメールを認識します
ただし，メールを定義する際には，定義中の以下の部分の
public class SampleMail2 : WpfSandboxLib.IMailManager
「SampleMail2」の部分が全体を通して
ユニークになるようにする必要があります
（メールIDのような扱いになります）
", // 行末の「",」は削除しないでください
Stamp = "", // 空白でスタンプ無しになります
AdventurePart = "")] // アドベンチャーパートのシナリオ名．空白でジャンプボタン無しになります
public class SampleMail2 : CurePLUSMailerLib.IMailManager
{
    // メールが受信可能かどうかのチェックを行い、結果をbool型で返却します
    public bool canReceive(ApplicationContext context)
    {
        return 10 < context.初回起動からの経過秒();
    }
    
    // メールを受信したタイミングでコールされます
    public void onReceived(ApplicationContext context)
    {
        // パラメータの更新等
    }

    // 初めてメールを開いたタイミングでコールされます
    public void onRead(ApplicationContext context)
    {
        // パラメータの更新等
    }

    // アドベンチャーパートから復帰したタイミングでコールされます
    public void onDoneAdventurePart(ApplicationContext context)
    {
        // パラメータの更新等
    }
}

#endregion


#region "返信のできるメール"

[CurePLUSMailerLib.MailInformation(
Sender = CurePLUSMailerLib.Entity.Character.CharacterID.Hime, // メールの送信者
Title = "サンプルメール３（返信の定義）", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"test mail content

届いたメールに返信ができる状態を定義するサンプル
データ上は一つのメールに対して
いくつでも返信を定義できますが
プログラムでは最初の三つまでしか
認識しません
", // 行末の「",」は削除しないでください
Stamp = "", // スタンプ画像名を指定、空白でスタンプ無しになります
AdventurePart = "")] // アドベンチャーパートのシナリオ名．空白でジャンプボタン無しになります
public class SampleMail3HasReply : CurePLUSMailerLib.IMailManager
{
    // メールが受信可能かどうかのチェックを行い、結果をbool型で返却します
    public bool canReceive(ApplicationContext context)
    {
        return 5 < context.初回起動からの経過秒();
    }
    
    // メールを受信したタイミングでコールされます
    public void onReceived(ApplicationContext context)
    {
        // パラメータの更新等
    }

    // 初めてメールを開いたタイミングでコールされます
    public void onRead(ApplicationContext context)
    {
        // パラメータの更新等
    }

    // アドベンチャーパートから復帰したタイミングでコールされます
    public void onDoneAdventurePart(ApplicationContext context)
    {
        // パラメータの更新等
    }
}

[CurePLUSMailerLib.ReplyInformation(
Title = "サンプルメール３返信１", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"返信内容１", // 行末の「",」は削除しないでください
Parent = typeof(SampleMail3HasReply) // 返信元のメールを指定します
)]
public class SampleMail3HasReply_Rep1 : CurePLUSMailerLib.IReplyManager
{
    // 選択肢として表示できるかどうかをbool値で返却します
    public bool canSelect(ApplicationContext context)
    {
        return true; // ex.一定の好感度以上で選択できる項目の判定など
    }

    // 返信が選択され、送信されたタイミングでコールされます
    public void onSent(ApplicationContext context)
    {
        // パラメータの更新等
        context.SetValue("リプライ説明スレッドステージ", 1);
        context.SetValue("リプライ説明スレッド応答日時", DateTime.Now);
        context.SetValue("リプライ説明汎用パラメータ", "返信1");
    }
}

[CurePLUSMailerLib.ReplyInformation(
Title = "サンプルメール３返信２", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"返信内容２", // 行末の「",」は削除しないでください
Parent = typeof(SampleMail3HasReply) // 返信元のメールを指定します
)]
public class SampleMail3HasReply_Rep2 : CurePLUSMailerLib.IReplyManager
{
    // 選択肢として表示できるかどうかをbool値で返却します
    public bool canSelect(ApplicationContext context)
    {
        return true;
    }

    // 返信が選択され、送信されたタイミングでコールされます
    public void onSent(ApplicationContext context)
    {
        // パラメータの更新等
        context.SetValue("リプライ説明スレッドステージ", 1);
        context.SetValue("リプライ説明スレッド応答日時", DateTime.Now);
        context.SetValue("リプライ説明汎用パラメータ", "返信2");
    }
}

[CurePLUSMailerLib.ReplyInformation(
Title = "サンプルメール３返信３", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"返信内容３", // 行末の「",」は削除しないでください
Parent = typeof(SampleMail3HasReply) // 返信元のメールを指定します
)]
public class SampleMail3HasReply_Rep3 : CurePLUSMailerLib.IReplyManager
{
    // 選択肢として表示できるかどうかをbool値で返却します
    public bool canSelect(ApplicationContext context)
    {
        return true;
    }

    // 返信が選択され、送信されたタイミングでコールされます
    public void onSent(ApplicationContext context)
    {
        // パラメータの更新等
        context.SetValue("リプライ説明スレッドステージ", 1);
        context.SetValue("リプライ説明スレッド応答日時", DateTime.Now);
        context.SetValue("リプライ説明汎用パラメータ", "返信3");
    }
}

[CurePLUSMailerLib.MailInformation(
Sender = CurePLUSMailerLib.Entity.Character.CharacterID.Hime, // メールの送信者
Title = "サンプルメール４（返信に対する応答）", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"ユーザが選択した返信に対する応答

\[リプライ説明汎用パラメータ]が返信として選択されました

このメールに対して返信をすると分岐が生じます
", // 行末の「",」は削除しないでください
Stamp = "", // スタンプ画像名を指定、空白でスタンプ無しになります
AdventurePart = "")] // アドベンチャーパートのシナリオ名．空白でジャンプボタン無しになります
public class SampleMail4HasReply : CurePLUSMailerLib.IMailManager
{
    // メールが受信可能かどうかのチェックを行い、結果をbool型で返却します
    public bool canReceive(ApplicationContext context)
    {
        if (context.GetValue<int>("リプライ説明スレッドステージ")!=1) return false;

        return 5 < context.経過秒("リプライ説明スレッド応答日時"); // 返信があってから5秒後に送信
    }

    // メールを受信したタイミングでコールされます
    public void onReceived(ApplicationContext context)
    {
        // パラメータの更新等
    }

    // 初めてメールを開いたタイミングでコールされます
    public void onRead(ApplicationContext context)
    {
        // パラメータの更新等
    }

    // アドベンチャーパートから復帰したタイミングでコールされます
    public void onDoneAdventurePart(ApplicationContext context)
    {
        // パラメータの更新等
    }
}

[CurePLUSMailerLib.ReplyInformation(
Title = "サンプルメール４返信１", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"返信内容１

分岐１", // 行末の「",」は削除しないでください
Parent = typeof(SampleMail4HasReply) // 返信元のメールを指定します
)]
public class SampleMail4HasReply_Rep1 : CurePLUSMailerLib.IReplyManager
{
    // 選択肢として表示できるかどうかをbool値で返却します
    public bool canSelect(ApplicationContext context)
    {
        return true; // ex.一定の好感度以上で選択できる項目の判定など
    }

    // 返信が選択され、送信されたタイミングでコールされます
    public void onSent(ApplicationContext context)
    {
        // パラメータの更新等
        context.SetValue("リプライ説明スレッドステージ", 2);
        context.SetValue("リプライ説明スレッド応答日時", DateTime.Now);
        context.SetValue("リプライ説明汎用パラメータ", "返信1");
    }
}

[CurePLUSMailerLib.ReplyInformation(
Title = "サンプルメール４返信２", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"返信内容２

分岐２", // 行末の「",」は削除しないでください
Parent = typeof(SampleMail4HasReply) // 返信元のメールを指定します
)]
public class SampleMail4HasReply_Rep2 : CurePLUSMailerLib.IReplyManager
{
    // 選択肢として表示できるかどうかをbool値で返却します
    public bool canSelect(ApplicationContext context)
    {
        return true;
    }

    // 返信が選択され、送信されたタイミングでコールされます
    public void onSent(ApplicationContext context)
    {
        // パラメータの更新等
        context.SetValue("リプライ説明スレッドステージ", 3);
        context.SetValue("リプライ説明スレッド応答日時", DateTime.Now);
        context.SetValue("リプライ説明汎用パラメータ", "返信2");
    }
}

[CurePLUSMailerLib.MailInformation(
Sender = CurePLUSMailerLib.Entity.Character.CharacterID.Hime, // メールの送信者
Title = "サンプルメール５（分岐１）", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"ユーザが選択した返信に対する応答（分岐１、アドベンチャーパート）
", // 行末の「",」は削除しないでください
Stamp = "", // スタンプ画像名を指定、空白でスタンプ無しになります
AdventurePart = "sample/KickTest")] // アドベンチャーパートのシナリオ名．空白でジャンプボタン無しになります
public class SampleMail5Branch1 : CurePLUSMailerLib.IMailManager
{
    // メールが受信可能かどうかのチェックを行い、結果をbool型で返却します
    public bool canReceive(ApplicationContext context)
    {
        if (context.GetValue<int>("リプライ説明スレッドステージ") != 2) return false;

        return 5 < context.経過秒("リプライ説明スレッド応答日時"); // 返信があってから5秒後に送信
    }

    // メールを受信したタイミングでコールされます
    public void onReceived(ApplicationContext context)
    {
        // パラメータの更新等
    }

    // 初めてメールを開いたタイミングでコールされます
    public void onRead(ApplicationContext context)
    {
        // パラメータの更新等
    }

    // アドベンチャーパートから復帰したタイミングでコールされます
    public void onDoneAdventurePart(ApplicationContext context)
    {
        // アドベンチャーパート内で現在日時を設定できないのでこちらで設定します
        context.SetValue("リプライ説明スレッド応答日時", DateTime.Now);
    }
}

[CurePLUSMailerLib.MailInformation(
Sender = CurePLUSMailerLib.Entity.Character.CharacterID.Hime, // メールの送信者
Title = "サンプルメール５（分岐２、アドベンチャーパート）", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"
ユーザが選択した返信に対する応答（分岐２）

", // 行末の「",」は削除しないでください
Stamp = "", // スタンプ画像名を指定、空白でスタンプ無しになります
AdventurePart = "sample/KickTest")] // アドベンチャーパートのシナリオ名．空白でジャンプボタン無しになります
public class SampleMail5Branch2 : CurePLUSMailerLib.IMailManager
{
    // メールが受信可能かどうかのチェックを行い、結果をbool型で返却します
    public bool canReceive(ApplicationContext context)
    {
        if (context.GetValue<int>("リプライ説明スレッドステージ") != 3) return false;

        return 5 < context.経過秒("リプライ説明スレッド応答日時"); // 返信があってから5秒後に送信
    }

    // メールを受信したタイミングでコールされます
    public void onReceived(ApplicationContext context)
    {
        // パラメータの更新等
    }

    // 初めてメールを開いたタイミングでコールされます
    public void onRead(ApplicationContext context)
    {
        // パラメータの更新等
    }

    // アドベンチャーパートから復帰したタイミングでコールされます
    public void onDoneAdventurePart(ApplicationContext context)
    {
        // アドベンチャーパート内で現在日時を設定できないのでこちらで設定します
        context.SetValue("リプライ説明スレッド応答日時", DateTime.Now);
    }
}

[CurePLUSMailerLib.MailInformation(
Sender = CurePLUSMailerLib.Entity.Character.CharacterID.Hime, // メールの送信者
Title = "サンプルメール６（アドベンチャーパートとのやり取り）", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"
アドベンチャーパートで”\[アドベンチャーパートで設定したパラメータ]”が選択されました
", // 行末の「",」は削除しないでください
Stamp = "", // スタンプ画像名を指定、空白でスタンプ無しになります
AdventurePart = "")] // アドベンチャーパートのシナリオ名．空白でジャンプボタン無しになります
public class SampleMail6 : CurePLUSMailerLib.IMailManager
{
    // メールが受信可能かどうかのチェックを行い、結果をbool型で返却します
    public bool canReceive(ApplicationContext context)
    {
        if (context.GetValue<int>("リプライ説明スレッドステージ") != 4) return false;

        return 5 < context.経過秒("リプライ説明スレッド応答日時"); // アドベンチャーパート終了から5秒後に送信
    }

    // メールを受信したタイミングでコールされます
    public void onReceived(ApplicationContext context)
    {
        // パラメータの更新等
    }

    // 初めてメールを開いたタイミングでコールされます
    public void onRead(ApplicationContext context)
    {
        // パラメータの更新等
    }

    // アドベンチャーパートから復帰したタイミングでコールされます
    public void onDoneAdventurePart(ApplicationContext context)
    {
        // パラメータの更新等
    }
}

#endregion

