using Contacts.Domain.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Application.BL
{
    public interface ILogic
    {
        public IEnumerable<ContactListDTO> List();
        public IEnumerable<ContactListDTO> Search(ListSearchDTO entity);
        public ContactDTO Get(UInt64 ContactId);
        public UInt64 Add(ContactDTO entity);
        public uint Update(ContactDTO entity);
        public uint Delete(UInt64 Id);    
    }
}
