﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace WebAutoCodeOnline.MySqlDAL
{
    public class OpLogDAL
    {

        public bool AddOpLog(OpLog model)
        {
            string insertSql = "insert OpLog(OpType ,OpContent ,OpIP ,OpTime) values (@OpType ,@OpContent ,@OpIP ,@OpTime)";
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            listParams.Add(new MySqlParameter("@OpType", MySqlDbType.Int32) { Value = model.OpType });
            listParams.Add(new MySqlParameter("@OpContent", MySqlDbType.VarChar) { Value = model.OpContent });
            listParams.Add(new MySqlParameter("@OpIP", MySqlDbType.VarChar) { Value = model.OpIP });
            listParams.Add(new MySqlParameter("@OpTime", MySqlDbType.DateTime) { Value = model.OpTime });

            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                return MySqlHelper2.ExecuteNonQuery(sqlcn, CommandType.Text, insertSql, listParams.ToArray()) > 0;
            }
        }

        public bool UpdateOpLog(OpLog model)
        {
            string updateSql = "update top(1) OpLog set  where  Id=@Id ";
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            listParams.Add(new MySqlParameter("@Id", MySqlDbType.Int32) { Value = model.Id });

            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                return MySqlHelper2.ExecuteNonQuery(sqlcn, CommandType.Text, updateSql, listParams.ToArray()) > 0;
            }
        }

        public bool BatUpdateOpLog(List<string> list, OpLog model)
        {
            var array = (from f in list
                         select "'" + f + "'").ToArray();
            string idStr = string.Join(",", array);
            string updateSql = string.Format("update  OpLog set  where  Id in ({0})", idStr);
            List<MySqlParameter> listParams = new List<MySqlParameter>();
            listParams.Add(new MySqlParameter("@Id", MySqlDbType.Int32) { Value = model.Id });

            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                return MySqlHelper2.ExecuteNonQuery(sqlcn, CommandType.Text, updateSql, listParams.ToArray()) > 0;
            }
        }

        public bool DeleteOpLog(List<string> list)
        {
            var array = (from f in list
                         select "'" + f + "'").ToArray();
            string idStr = string.Join(",", array);
            string deleteSql = string.Format("delete from OpLog  where Id in ({0})", idStr);
            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                return MySqlHelper2.ExecuteNonQuery(sqlcn, CommandType.Text, deleteSql, null) > 0;
            }
        }

        public List<OpLog> QueryList(int page, int pageSize)
        {
            string whereStr = string.Empty;
            List<MySqlParameter> listParams = new List<MySqlParameter>();

            string selectSql = string.Format(@"select * from
	        (select top 100 percent *,ROW_NUMBER() over(order by Id) as rownumber from
	        OpLog where 1=1 {0}) as T
	        where rownumber between {1} and {2};", whereStr, ((page - 1) * pageSize + 1), page * pageSize);

            List<OpLog> result = new List<OpLog>();
            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                using (MySqlDataReader sqldr = MySqlHelper2.ExecuteDataReader(sqlcn, CommandType.Text, selectSql, listParams.ToArray()))
                {
                    while (sqldr.Read())
                    {
                        OpLog model = new OpLog();
                        model.Id = sqldr["Id"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["Id"]);
                        model.OpType = sqldr["OpType"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["OpType"]);
                        model.OpContent = sqldr["OpContent"] == DBNull.Value ? string.Empty : sqldr["OpContent"].ToString();
                        model.OpIP = sqldr["OpIP"] == DBNull.Value ? string.Empty : sqldr["OpIP"].ToString();
                        model.OpTime = sqldr["OpTime"] == DBNull.Value ? DateTime.Parse("1970-1-1") : Convert.ToDateTime(sqldr["OpTime"]);

                        result.Add(model);
                    }
                }
            }

            return result;
        }

        public int QueryListCount()
        {
            string whereStr = string.Empty;
            List<MySqlParameter> listParams = new List<MySqlParameter>();

            string selectCountSql = string.Format("select count(0) from OpLog where 1=1 {0}", whereStr);
            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                return Convert.ToInt32(MySqlHelper2.ExecuteScalar(sqlcn, CommandType.Text, selectCountSql, listParams.ToArray()));
            }
        }

        public List<OpLog> GetPartAll(List<string> idList)
        {
            var idArrayStr = string.Join(",", (from f in idList
                                               select "'" + f + "'").ToArray());
            string whereStr = string.Empty;
            List<MySqlParameter> listParams = new List<MySqlParameter>();

            string selectSql = string.Format(@"select * from
            OpLog where Id in ({1})
            {0};", whereStr, idArrayStr);

            List<OpLog> result = new List<OpLog>();
            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                using (MySqlDataReader sqldr = MySqlHelper2.ExecuteDataReader(sqlcn, CommandType.Text, selectSql, listParams.ToArray()))
                {
                    while (sqldr.Read())
                    {
                        OpLog model = new OpLog();
                        model.Id = sqldr["Id"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["Id"]);
                        model.OpType = sqldr["OpType"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["OpType"]);
                        model.OpContent = sqldr["OpContent"] == DBNull.Value ? string.Empty : sqldr["OpContent"].ToString();
                        model.OpIP = sqldr["OpIP"] == DBNull.Value ? string.Empty : sqldr["OpIP"].ToString();
                        model.OpTime = sqldr["OpTime"] == DBNull.Value ? DateTime.Parse("1970-1-1") : Convert.ToDateTime(sqldr["OpTime"]);

                        result.Add(model);
                    }
                }
            }

            return result;
        }

        public List<OpLog> GetAll()
        {
            string whereStr = string.Empty;
            List<MySqlParameter> listParams = new List<MySqlParameter>();

            string selectAllSql = string.Format("select * from OpLog where 1=1 {0}", whereStr);
            List<OpLog> result = new List<OpLog>();
            using (MySqlConnection sqlcn = ConnectionFactory.AliDb)
            {
                using (MySqlDataReader sqldr = MySqlHelper2.ExecuteDataReader(sqlcn, CommandType.Text, selectAllSql, listParams.ToArray()))
                {
                    while (sqldr.Read())
                    {
                        OpLog model = new OpLog();
                        model.Id = sqldr["Id"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["Id"]);
                        model.OpType = sqldr["OpType"] == DBNull.Value ? 0 : Convert.ToInt32(sqldr["OpType"]);
                        model.OpContent = sqldr["OpContent"] == DBNull.Value ? string.Empty : sqldr["OpContent"].ToString();
                        model.OpIP = sqldr["OpIP"] == DBNull.Value ? string.Empty : sqldr["OpIP"].ToString();
                        model.OpTime = sqldr["OpTime"] == DBNull.Value ? DateTime.Parse("1970-1-1") : Convert.ToDateTime(sqldr["OpTime"]);

                        result.Add(model);
                    }
                }
            }

            return result;
        }

    }
}