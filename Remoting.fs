namespace SampleApp

open WebSharper

module Server =

    let mutable number : int = 1
    let mutable users : User list = []

    [<Rpc>]
    let DoSomething input =
        let R (s: string) = System.String(Array.rev(s.ToCharArray()))
        async {
            return R input
        }
    
    [<Rpc>]
    let GetUsers () =
        async {
            return users
        }

    [<Rpc>]
    let AddUser user =
        users <- { user with id = number } :: users
        number <- number + 1
        async {
            return users
        }
