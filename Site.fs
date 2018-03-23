namespace SampleApp

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI
open WebSharper.UI.Server

type EndPoint =
  | [<EndPoint "/">] Home

module Templating =
  open WebSharper.UI.Html

  type MainTemplate = Templating.Template<"Main.html">

  let main ctx action (title: string) (body: Doc list) =
    Content.Page(
      MainTemplate()
        .Title(title)
        .Body(body)
        .Doc()
    )

module Site =
  open WebSharper.UI.Html

  let HomePage ctx =
    Templating.main ctx EndPoint.Home "Home" [
      h1 [] [text "Board"]
      div [] [client <@ Client.main () @>]
    ]

  [<Website>]
  let main =
    Application.MultiPage(
      fun ctx endpoint ->
        match endpoint with
          EndPoint.Home -> HomePage ctx
    )
