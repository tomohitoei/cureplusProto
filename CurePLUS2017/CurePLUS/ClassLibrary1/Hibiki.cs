using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurePLUSMailerLib;

#region "響からのメール"

[CurePLUSMailerLib.MailInformation(
Sender = CurePLUSMailerLib.Entity.Character.CharacterID.Hime,
Title = "test", // メールのタイトル
Content = // メール本文を下の行から記述，以下の行頭の「@"」は削除しないでください
@"test mail content", // 行末の「",」は削除しないでください
Stamp = "", // 空白でスタンプ無しになります
AdventurePart = "")] // アドベンチャーパートのシナリオ名．空白でジャンプボタン無しになります
public class 響001 : CurePLUSMailerLib.IMailManager
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


#endregion

