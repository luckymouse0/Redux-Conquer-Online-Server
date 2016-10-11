using System.Collections;
using System;
using System.Collections.Generic;
using Redux.Database.Domain;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using Redux.Structures;

namespace Redux.Database.Repositories
{
    public class BugReportRepository : Repository<uint, DbBugReport>
    {
    }
}

