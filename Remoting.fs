namespace SampleApp

open System
open WebSharper

module Server =
    
  [<Rpc>]
  let get_posts () =
    async {
      return Seq.toList <| Post.all ()
    }

  [<Rpc>]
  let add_post post =
    async {
      return Post.insert post
    }
