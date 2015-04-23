using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyWebApiProject.Models
{
    interface IDial800Repository
    {
        IEnumerable<Dial800_Call> GetAll();
        Dial800_Call Get(int id);
        Dial800_Call Add(Dial800_Call dial800_call);
        void Remove(int id);
        bool Update(Dial800_Call dial800_call);
    }
}
