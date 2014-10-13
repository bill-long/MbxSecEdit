using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MbxSecEdit
{
    public class AclInfo
    {
        public IEnumerable<AceInfo> DaclAces { get; set; }

        public AclInfo(ActiveDs.IADsSecurityDescriptor sd)
        {
            var daclAces = new List<AceInfo>();
            foreach (ActiveDs.IADsAccessControlEntry ace in sd.DiscretionaryAcl)
            {
                daclAces.Add(new AceInfo()
                {
                    Trustee = ace.Trustee,
                    AccessMask = ace.AccessMask,
                    AceFlags = ace.AceFlags,
                    AceType = ace.AceType,
                    Flags = ace.Flags
                });
            }

            this.DaclAces = daclAces;
        }
    }

    public class AceInfo
    {
        public string Trustee { get; set; }
        public int AccessMask { get; set; }
        public int AceType { get; set; }
        public int AceFlags { get; set; }
        public int Flags { get; set; }
    }
}
