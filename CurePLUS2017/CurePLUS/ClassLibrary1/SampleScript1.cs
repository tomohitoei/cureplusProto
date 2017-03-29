using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurePLUSMailerLib;

#region "サンプル1"

[CurePLUSMailerLib.MailInformation(
Sender = CurePLUSMailerLib.Entity.Character.CharacterID.Hime, // メールの送信者
Title = "sample mail 1", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"test mail content

このメールはシステムからユーザへ
送信されるメールとしては最小単位のメールになります
", // 行末の「",」は削除しないでください
Stamp = "", // 空白でスタンプ無しになります
AdventurePart = "")] // アドベンチャーパートのシナリオ名．空白でジャンプボタン無しになります
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
}

[CurePLUSMailerLib.MailInformation(
Sender = CurePLUSMailerLib.Entity.Character.CharacterID.Hime, // メールの送信者
Title = "sample mail 1", // メールのタイトル
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
}

#endregion


#region "返信のできるメール"

[CurePLUSMailerLib.MailInformation(
Sender = CurePLUSMailerLib.Entity.Character.CharacterID.Hime, // メールの送信者
Title = "sample mail reply 1", // メールのタイトル
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
public class SampleMailReply1 : CurePLUSMailerLib.IMailManager
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
}

[CurePLUSMailerLib.ReplyInformation(
Title = "返信１", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"返信内容１", // 行末の「",」は削除しないでください
Parent = typeof(SampleMailReply1) // 返信元のメールを指定します
)]
public class SampleMailReply1_Rep1 : CurePLUSMailerLib.IReplyManager
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
    }
}

[CurePLUSMailerLib.ReplyInformation(
Title = "返信２", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"返信内容２", // 行末の「",」は削除しないでください
Parent = typeof(SampleMailReply1) // 返信元のメールを指定します
)]
public class SampleMailReply1_Rep2 : CurePLUSMailerLib.IReplyManager
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
    }
}

[CurePLUSMailerLib.ReplyInformation(
Title = "返信３", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"返信内容３", // 行末の「",」は削除しないでください
Parent = typeof(SampleMailReply1) // 返信元のメールを指定します
)]
public class SampleMailReply1_Rep3 : CurePLUSMailerLib.IReplyManager
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
    }
}

#endregion

