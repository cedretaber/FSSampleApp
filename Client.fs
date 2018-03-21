namespace SampleApp

open System
open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

[<JavaScript>]
module Client =

  let main () =
    let name_input = Var.Create "" in
    let title_input = Var.Create "" in
    let text_input = Var.Create "" in
    let submit =
      Submitter.CreateOption <|
        View.Map3
          (fun name title text ->
            { id = 0
            ; author = name
            ; title = title
            ; text = text
            ; created_at = DateTime.Now }
          )
          name_input.View
          title_input.View
          text_input.View in
    let posts =
      submit.View.MapAsync <|
        function
          None ->
            Server.get_posts ()
        | Some post ->
            async {
              let! _ = Server.add_post post
              let! list = Server.get_posts ()
              return list
            } in
    let make_card post : Doc =
      let date =
        new Date(int <| post.created_at.ToString ())
      div [attr.``class`` "uk-card uk-card-default"] [
        div [attr.``class`` "uk-card-header"] [
          h3 [attr.``class`` "uk-card-title"] [text post.title]
        ]
        div [attr.``class`` "uk-card-body"] [
          p [] [text post.text]
        ]
        div [attr.``class`` "uk-card-footer"] [
          div [attr.``class`` "uk-grid"] [
            div [] [
              p [] [text post.author]
            ]
            div [] [
              p [attr.``class`` "uk-text-meta"] [
                time [attr.datetime <| date.ToISOString ()] [
                  text <| date.ToLocaleString ()
                ]
              ]
            ]
          ]
        ]
      ] :> Doc
    div [] [
      form [] [
        div [attr.``class`` "uk-margin"] [
          Doc.Input [attr.``class`` "uk-input"; attr.placeholder "名前"] name_input
        ]
        div [attr.``class`` "uk-margin"] [
          Doc.Input [attr.``class`` "uk-input uk-margin"; attr.placeholder "タイトル"] title_input
        ]
        div [attr.``class`` "uk-margin"] [
          Doc.InputArea [attr.``class`` "uk-textarea uk-margin"] text_input
        ]
        Doc.Button "投稿" [attr.``class`` "uk-button uk-button-primary uk-margin"] submit.Trigger
      ]
      Doc.BindView
        (fun posts ->
          ul [attr.``class`` "uk-list"] [
            for post in posts ->
              li [attr.``class`` "uk-margin"] [
                make_card post
              ] :> Doc
          ]
        )
        posts
    ]
