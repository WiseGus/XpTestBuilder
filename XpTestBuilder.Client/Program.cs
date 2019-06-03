using System;
using System.ServiceModel;
using System.Windows.Forms;
using XpTestBuilder.Common;

namespace XpTestBuilder.Client
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainF = new MainF();

            var context = new InstanceContext(mainF);
            using (var _proxyFactory = new DuplexChannelFactory<ICommandService>(context, "CommandService"))
            {
                var proxy = _proxyFactory.CreateChannel();
                mainF.Proxy = proxy;

                Application.Run(mainF);
            }
        }
    }
}