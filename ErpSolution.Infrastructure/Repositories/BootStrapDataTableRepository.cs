using Dapper;
using ErpSolution.Application.DTOs.AuxiliaryModels;
using ErpSolution.Application.Interfaces;
using ErpSolution.Domain.Entities;
using MentokLibrary1_0;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSolution.Infrastructure.Repositories
{
    public class BootStrapDataTableRepository<T> : IBootStrapDataTableRepository<T>
    {
        private readonly IMentok mentok;
        private readonly IHttpContextAccessor httpContextAccessor;

        public BootStrapDataTableRepository(IMentok mentok,
                                            IHttpContextAccessor httpContextAccessor)
        {
            this.mentok = mentok;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> GetData(string condition = null, Dictionary<string, object> parameters = null)
        {
            List<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>> forms = httpContextAccessor.HttpContext.Request.Form.ToList();
            List<string> listColumn = forms.Where(item => item.Key.Contains("data")).Select(item => item.Value.ToString()).Where(item => item.ToUpper() != "ACTIONS").ToList();
            List<T> models = null;

            string OrderBy = listColumn[int.Parse(forms.FirstOrDefault(item => item.Key == "order[0][column]").Value.ToString())];

            string OrderDirection = forms.FirstOrDefault(item => item.Key == "order[0][dir]").Value.ToString();
            string SearchValue = forms.FirstOrDefault(item => item.Key == "search[value]").Value.ToString();
            string start = forms.FirstOrDefault(item => item.Key == "start").Value.ToString();
            string length = forms.FirstOrDefault(item => item.Key == "length").Value.ToString();
            string draw = forms.FirstOrDefault(item => item.Key == "draw").Value.ToString();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int? recordsTotal = 0;
            string whereCondition = string.Empty;
            DynamicParameters dynamicParameters = null;
            Dictionary<string, object> dictParam = new Dictionary<string, object>();          

            if (!string.IsNullOrEmpty(SearchValue))
            {
                if (condition == null && parameters == null)
                {
                    if (listColumn.Count > 1)
                    {
                        whereCondition = string.Join(" or ", listColumn.Select(item => $" { item } like '%'+@{ item }+'%'"));
                        whereCondition = " where (" + whereCondition + ")";


                        listColumn.ForEach(item =>
                        {
                            if (dictParam.ContainsKey($"@{item}") == false)
                                dictParam.Add($"@{item}", SearchValue);
                        });


                    }
                    else if (listColumn.Count == 1)
                        whereCondition = " where (" + $" { listColumn[0] } like '%'+@{ listColumn[0] }+'%') ";
                }
                else
                {
                    if (listColumn.Count > 1)
                    {
                        whereCondition = string.Join(" or ", listColumn.Select(item => $" { item } like '%'+@{ item }+'%'"));
                        whereCondition = " where (" + whereCondition + $") and ({ condition }) ";


                        listColumn.ForEach(item =>
                        {
                            if (dictParam.ContainsKey($"@{item}") == false)
                                dictParam.Add($"@{item}", SearchValue);
                        });

                        foreach (KeyValuePair<string, object> entry in parameters)
                        {
                            dictParam.Add(entry.Key, entry.Value);
                        }
                    }
                    else if (listColumn.Count == 1)
                    {
                        whereCondition = " where (" + $" { listColumn[0] } like '%'+@{ listColumn[0] }+'%') and ({ condition }) ";
                    }
                }
            }
            else
            {
                if (condition != null && parameters != null)
                {
                    whereCondition = $" where { condition }";

                    foreach (KeyValuePair<string, object> entry in parameters)
                    {
                        dictParam.Add(entry.Key, entry.Value);
                    }
                }
            }


            if (OrderBy.Contains("___datestring"))
            {
                string[] splitOrderBy = OrderBy.Split("___");
                OrderBy = splitOrderBy[0];
            }          

            if (dictParam.Count > 0)
                dynamicParameters = new DynamicParameters(dictParam);


            if (OrderBy.Contains("___datestring"))
            {
                string[] splitOrderBy = OrderBy.Split("___");
                OrderBy = splitOrderBy[0];
            }         


            StringBuilder sbSql = new StringBuilder();
            sbSql.Append($" select *,total_records = count(*) over() from { typeof(T).Name } ");
            sbSql.Append(whereCondition);
            sbSql.Append($" order by { OrderBy } { OrderDirection }");
            sbSql.Append($" offset { skip } rows");
            sbSql.Append($" fetch next { pageSize } rows only");

            if (dynamicParameters == null)
            {
                try
                {
                    models = (await mentok.CRUD.Read.GetRowsAsync<T>(sbSql.ToString(), null)).ToList();
                }
                catch (Exception ex)
                {

                    Debug.Write(ex.Message);
                }
            }
            else
            {
                models = (await mentok.CRUD.Read.GetRowsAsync<T>(sbSql.ToString(), dynamicParameters)).ToList();

            }


            if (models.Count() > 0)
            {
                Type type = typeof(T);

                var prop = type.GetProperty("total_records");
                object obj = prop.GetValue(models[0], null);
                recordsTotal = obj != null ? Convert.ToInt32(obj) : 0;
            }
            else
            {
                recordsTotal = 0;
            }

            return new OkObjectResult(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = models });
        }

    }
}
