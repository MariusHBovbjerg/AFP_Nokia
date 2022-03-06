namespace WaveFormWeb.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

type AboutController (logger : ILogger<AboutController>) =
    inherit Controller()

    member this.About () = 
        this.View()

