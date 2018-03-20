namespace SampleApp

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Html

[<JavaScript>]
module Users =

    let Main() =
        let nameInput = Var.Create ""
        let ageInput = Var.Create 0
        let userInput =
            View.Map2
                (fun name age -> { id = 0; name = name; age = age })
                nameInput.View
                ageInput.View
        let submit = Submitter.CreateOption userInput
        let userList =
                submit.View.MapAsync <|
                    function
                    | None -> Server.GetUsers ()
                    | Some user -> Server.AddUser user

        div [] [
            form [] [
                div [attr.``class`` "form-group"] [
                    label [] [text "ユーザ名"]
                    Doc.Input [attr.``class`` "form-control"] nameInput
                ]
                div [attr.``class`` "form-group"] [
                    label [] [text "年齢"]
                    Doc.IntInputUnchecked [attr.``class`` "form-control"] ageInput
                ]
                Doc.Button "登録" [attr.``class`` "btn btn-primary"] submit.Trigger
            ]
            table [attr.``class`` "table"] [
                thead [] [
                    tr [] [
                        th [] [text "ID"]
                        th [] [text "ユーザ名"]
                        th [] [text "年齢"]
                    ]
                ]
                Doc.BindView ( fun userList ->
                    tbody [] [ 
                       for user in userList ->
                            tr [] [
                                td [] [text <| string user.id]
                                td [] [text user.name]
                                td [] [text <| string user.age]
                            ] :> Doc
                       ]
                ) userList
            ]
        ]