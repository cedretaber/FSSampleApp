namespace SampleApp

#nowarnn "62"

open System.Data.SqlClient
open System.Dynamic
open System.Collections.Generic
open Dapper
open Npgsql

module Database =

  let mutable conn : NpgsqlConnection = null

  let init () =
    let conn_string = "Host=127.0.0.1;Database=fssample;Username=postgres;Password="
    conn <- new NpgsqlConnection(conn_string)
    conn.Open

  let close () =
    conn.Close 

  let query (query : string) : 'r seq =
    conn.Query<'r> query
  
  let pquery (query : string) (param : obj) : 'r seq =
    conn.Query<'r> (query, param)

  let mquery (query : string) (param : (string, _) Map) : 'r seq =
    let exp = ExpandoObject () in
    let exp_dic = exp :> IDictionary<string, obj> in
    for KeyValue(k, v) in param do
      exp_dic.Add (k, v :> obj)
    pquery query exp

  let insert (query : string) (param : obj) : int =
    conn.Execute (query, param)