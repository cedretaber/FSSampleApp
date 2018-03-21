namespace SampleApp

open System
open WebSharper

module Server =

  let mutable number = 1
  let mutable post_list : Post list = []
    
  [<Rpc>]
  let get_posts () =
    async {
      return post_list
    }

  [<Rpc>]
  let add_post post =
    post_list <- { post with id = number } :: post_list
    number <- number + 1
    async {
      return number - 1
    }
