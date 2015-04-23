using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmptyWebApiProject.Models
{
    public class Dial800Repository : IDial800Repository
    {
        private List<Dial800_Call> _dial800_calls = new List<Dial800_Call>();
        private int _fakeDatabaseID = 1;

        public Dial800Repository()
        {
            Dial800_ani _ani = new Dial800_ani { city = "Reisterstown", number = "443-825-5040", state = "MD" };
            Dial800_target _target = new Dial800_target { duration = 60, status = "Connected", target = "555-555-5555" };
            List<Dial800_target> _ltargets = new List<Dial800_target>();
            _ltargets.Add(_target);

            this.Add(new Dial800_Call {
                id = 123456,
                ani = _ani,
                dnis = "555-555-5555",
                targets = _ltargets,
                start = "2014-04-17T22:21:22",
                end = "2014-04-17T22:22:22",
                recording = "" 
            });
        }

        public IEnumerable<Dial800_Call> GetAll() { return _dial800_calls; }
        public Dial800_Call Get(int id) { return _dial800_calls.Find(p => p.id == id); }
        public void Remove(int id) { _dial800_calls.RemoveAll(p => p.id == id); }

        public Dial800_Call Add(Dial800_Call dial800_call)
        {
            if (dial800_call == null) { throw new ArgumentNullException("dial800_call"); }
            dial800_call.id = _fakeDatabaseID++;
            _dial800_calls.Add(dial800_call);
            return dial800_call;
        }

        public bool Update(Dial800_Call dial800_call)
        {
            if (dial800_call == null) { throw new ArgumentNullException("dial800_call"); }
            int index = _dial800_calls.FindIndex(p => p.id == dial800_call.id);
            if (index == -1) { return false; }
            _dial800_calls.RemoveAt(index);
            _dial800_calls.Add(dial800_call);
            return true;
        }
    }
}