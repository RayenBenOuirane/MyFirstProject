﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Repositories
{
    
    public abstract class RepositoryBase
    {
        private readonly String _connectionString;
        public RepositoryBase()
        {
            _connectionString = "Server=LAPTOP-QNG4FGL3\\SQLEXPRESS; Database=Mybase; Integrated Security=true;";
        }
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
