using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSandboxLib;

public class HimeMail001_01 : WpfSandboxLib.IMailManager
{
    public bool canReceive(ApplicationContext context)
    {
        var d1 = context.GetValue<DateTime>("初回起動日時");
        var d2 = DateTime.Now;
        var dd = d2 - d1;
        return 5 < dd.TotalSeconds;
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

public class HimeMail001_02 : WpfSandboxLib.IMailManager
{
    public bool canReceive(ApplicationContext context)
    {
        var d1 = context.GetValue<DateTime>("初回起動日時");
        var d2 = DateTime.Now;
        var dd = d2 - d1;
        return 15 < dd.TotalSeconds;
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

public class HimeMail001_03 : WpfSandboxLib.IMailManager
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
