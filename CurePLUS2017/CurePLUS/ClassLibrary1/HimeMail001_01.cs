using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurePLUSMailerLib;

#region "ヒメからの最初のメールと返信"

// NPCから送信されるメールの内容は以下に記述します（ダブルクォーテーション（"）、行末のカンマ等は削除しないでください）
[CurePLUSMailerLib.MailInformation(
Sender = CurePLUSMailerLib.Entity.Character.CharacterID.Hime,
Title = "今日ね", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"やっほー
今日ね、公園の前でゆうゆうにばったり会ったよ
その時に新作の飴を作ったって一つもらったんだけど
とってもおいしかったよ！

長い行ああああああああああああああああああああああああああああああああああいいいいいいいいいいいいいい
絵文字\{mark_face_laugh}\{mark_face_jito}

\{mark_face_laugh}\{mark_face_jito}\{mark_face_laugh}\{mark_face_jito}\{mark_face_laugh}\{mark_face_jito}\{mark_face_laugh}\{mark_face_jito}\{mark_face_laugh}\{mark_face_jito}\{mark_face_laugh}\{mark_face_jito}

縦
に
長
い
ド
キ
ュ
メ
ン
ト

縦
に
長
い
ド
キ
ュ
メ
ン
ト

縦
に
長
い
ド
キ
ュ
メ
ン
ト",
Stamp = "hime_love", // 空白でスタンプ無しになります
AdventurePart = "adventure_part_name")
] public class HimeMail001_01 : CurePLUSMailerLib.IMailManager // 各種タイミングでパラメータ等の確認変更を行うスクリプトは以下に記述
{
    // メールを受信可能かどうかをbool値で返却します
    public bool canReceive(ApplicationContext context)
    {
        return false;
        //return 5 < context.起動からの経過秒(); // 必ず値を返却してください
    }

    // メールを受信したタイミングでコールされます
    public void onReceived(ApplicationContext context)
    {
        //
    }

    // メールを選択したタイミングでコールされます
    public void onRead(ApplicationContext context)
    {
        //
    }
}

[CurePLUSMailerLib.ReplyInformation(
Title ="返信１", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"返信内容１", // 行末の「",」は削除しないでください
Parent =typeof(HimeMail001_01) // 返信元のメールを指定します
)]
public class HimeMail001_01_Reply1 : CurePLUSMailerLib.IReplyManager
{
    // 選択肢として表示できるかどうかをbool値で返却します
    public bool canSelect(ApplicationContext context)
    {
        return true;
    }

    // 返信が選択され、送信されたタイミングでコールされます
    public void onSent(ApplicationContext context)
    {
        //
    }
}

[CurePLUSMailerLib.ReplyInformation(
Title = "返信２", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"返信内容２", // 行末の「",」は削除しないでください
Parent = typeof(HimeMail001_01) // 返信元のメールを指定します
)]
public class HimeMail001_01_Reply2 : CurePLUSMailerLib.IReplyManager
{
    public bool canSelect(ApplicationContext context)
    {
        return true;
    }

    public void onSent(ApplicationContext context)
    {
        //
    }
}

[CurePLUSMailerLib.ReplyInformation(
Title = "返信３", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"返信内容３", // 行末の「",」は削除しないでください
Parent = typeof(HimeMail001_01) // 返信元のメールを指定します
)]
public class HimeMail001_01_Reply3 : CurePLUSMailerLib.IReplyManager
{
    public bool canSelect(ApplicationContext context)
    {
        return true;
    }

    public void onSent(ApplicationContext context)
    {
        //
    }
}

#endregion

[CurePLUSMailerLib.MailInformation(
Sender = CurePLUSMailerLib.Entity.Character.CharacterID.Hime,
Title = "test", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"test mail content", // 行末の「",」は削除しないでください
Stamp = "", // 空白でスタンプ無しになります
AdventurePart = "")] // アドベンチャーパートのシナリオ名．空白でジャンプボタン無しになります
public class HimeMail001_02 : CurePLUSMailerLib.IMailManager
{
    public bool canReceive(ApplicationContext context)
    {
        return false;
        //return 15 < context.起動からの経過秒();
    }

    public void onRead(ApplicationContext context)
    {
        //
    }

    public void onReceived(ApplicationContext context)
    {
        //
    }
}

public class HimeMail001_03 : CurePLUSMailerLib.IMailManager
{
    public bool canReceive(ApplicationContext context)
    {
        return false;
    }

    public void onReceived(ApplicationContext context)
    {
    }

    public void onRead(ApplicationContext context)
    {
    }
}
