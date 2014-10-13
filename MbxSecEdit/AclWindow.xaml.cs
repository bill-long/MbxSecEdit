using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MbxSecEdit
{
    /// <summary>
    /// Interaction logic for AclWindow.xaml
    /// </summary>
    public partial class AclWindow : Window
    {
        public AclWindow(AclInfo acl, string title)
        {
            InitializeComponent();
            DataContext = acl;
            Title = title;
        }

        private void AddAce_OnClick(object sender, RoutedEventArgs e)
        {
            var newAceWindow = new NewAceWindow();
            if (newAceWindow.ShowDialog() == true)
            {
                var trustee = newAceWindow.TrusteeTextBox.Text;
                int accessMask = int.Parse(newAceWindow.AccessMaskTextBox.Text);
                int aceType = int.Parse(newAceWindow.AceTypeTextBox.Text);
                int aceFlags = int.Parse(newAceWindow.AceFlagsTextBox.Text);
                int flags = int.Parse(newAceWindow.FlagsTextBox.Text);

                var entry = new DirectoryEntry("LDAP://" + Title);
                var mbxSd = (ActiveDs.IADsSecurityDescriptor) entry.Properties["msExchMailboxSecurityDescriptor"][0];
                var dacl = (ActiveDs.AccessControlList) mbxSd.DiscretionaryAcl;
                var newAce = new ActiveDs.AccessControlEntry()
                {
                    AccessMask = accessMask,
                    AceType = aceType,
                    AceFlags = aceFlags,
                    Flags = flags,
                    Trustee = trustee
                };

                dacl.AddAce(newAce);
                mbxSd.DiscretionaryAcl = dacl;
                entry.Properties["msExchMailboxSecurityDescriptor"][0] = mbxSd;
                entry.CommitChanges();

                DataContext =
                    new AclInfo((ActiveDs.IADsSecurityDescriptor) entry.Properties["msExchMailboxSecurityDescriptor"][0]);
            }
        }
    }
}
