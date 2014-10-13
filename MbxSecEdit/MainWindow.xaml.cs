using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MbxSecEdit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dn = DnTextBox.Text;
            try
            {
                var entry = new DirectoryEntry("LDAP://" + dn);
                var masterAccountName = "Unknown";
                var masterAccountSidString = "";
                var objectClass = entry.Properties["objectClass"][0].ToString();
                if (entry.Properties.Contains("msExchMasterAccountSid"))
                {
                    var sidBytes = (byte[]) entry.Properties["msExchMasterAccountSid"][0];
                    var masterAccountSid = new SecurityIdentifier(sidBytes, 0);
                    masterAccountSidString = masterAccountSid.ToString();
                    try
                    {
                        masterAccountName = ((NTAccount) masterAccountSid.Translate(typeof (NTAccount))).ToString();
                    }
                    catch
                    {
                    }
                }

                var mbxSd = (ActiveDs.IADsSecurityDescriptor)entry.Properties["msExchMailboxSecurityDescriptor"][0];
                var window = new AclWindow(new AclInfo(mbxSd), dn);
                window.Show();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error: " + exc.Message);
            }
        }
    }
}
