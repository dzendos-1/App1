using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    public class DataRow
    {
        public String fio { get; set; }
        public String post { get; set; }
        public String login { get; set; }
        public String password { get; set; }

        public DataRow() { }
        public DataRow(String _fio, String _post, String _login, String _psw)
        {
            fio = _fio;
            post = _post;
            login = _login;
            password = _psw;
        }

        ~DataRow()
        {

        }
    }
}
