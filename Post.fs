namespace SampleApp

open System

module Post =

  type t =
    { id : int
    ; author : string
    ; title : string
    ; text : string
    ; created_at : DateTime
    }

  let create author title text =
    { id = 0
    ; author = author
    ; title = title
    ; text = text
    ; created_at = DateTime.Now
    }

  let all () : t seq =
    Database.query "SELECT * FROM posts"

  let insert (post : t) : int =
    let query = @"
    INSERT INTO posts
    ( author
    , title
    , text
    , created_at
    )
    VALUES
    ( @author
    , @title
    , @text
    , now())
    " in
    Database.insert query post in